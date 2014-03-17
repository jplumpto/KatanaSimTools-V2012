using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FlightControlInput
{
    #region Enums
    public enum MachineStates : byte
    {
        POWERUP = 0,
        IDLE = 1,       //0001;
        STANDBY = 2,    //0010;
        ENGAGED = 3,    //0011;
        PARKING = 7,    //0111;
        FAULT1 = 8,     //1000;
        FAULT2 = 9,     //1001;
        FAULT3 = 10,    //1010;
        DISABLED = 11,  //1011;
        INHIBITED = 12  //1100;
    }
    #endregion

    #region Structures
    public struct MDACommand 
    {
        public uint MCW;//Motion Command Word

        public float a_roll;		// rad/s2
        public float a_pitch;		// rad/s2
        public float a_z;			// m/s2
        public float a_x;			// m/s2
        public float a_yaw;			// rad/s2
        public float a_y;			// m/s2

        public float v_roll;			// rad/h_MoogSocket
        public float v_pitch;		// rad/h_MoogSocket
        public float v_yaw;			// rad/h_MoogSocket
        public float roll;			// rad
        public float pitch;			// rad
        public float yaw;			// rad

        public float buffet_roll;	// rad
        public float buffet_pitch;	// rad
        public float buffet_z;		// m
        public float buffet_x;		// m
        public float buffet_yaw;		// rad
        public float buffet_y;		// m

        public float v_vehicle;		// m/h_MoogSocket

        //public uint spare1;
        //public uint spare2;

        //Elapsed Time since Midnight
        public double elapsedTime;  //Seconds
    }

    public struct MOOGResponse 
    {
        public uint fault;
        public uint io_info;
        public uint status;

        public float roll;
        public float pitch;
        public float heave;
        public float surge;
        public float yaw;
        public float lateral;

        public uint spare;
    }
    #endregion

    public partial class ControlGUI : Form
    {
        #region MOOG_Constants
        const int Time_Offset = 80;

        //Motion Command Words
        const byte MOOG_DISABLE = 0x00DC;
        const byte MOOG_PARK = 0xD2;
        const byte MOOG_ENGAGE = 0xB4;
        const byte MOOG_RESET = 0xA0;
        const byte MOOG_INHIBIT = 0x96;
        const byte MOOG_MDAMODE = 0x8C;
        const byte MOOG_NEWMDA = 0x80; //New MDA accelerations
        const byte MOOG_NEWMDAFILE = 0x9B;  //Change MDA File

        private byte[] MOOG_MDAOPTIONS = new byte[13] { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113};
        private ComboBox.ObjectCollection MOOG_MDAOPTIONSTRINGS = null;
        #endregion

        #region Variables
        private Stopwatch t_inputDataStopwatch = new Stopwatch();
        private CreatedData simulateData = new CreatedData();
        private FlightTestData testData = new FlightTestData();
        private ArduinoCommunication arduino = new ArduinoCommunication();
        private const double GRAVITY = 9.80665;
        private const double Deg2Rad = Math.PI / 180.0;
        private double recvDeltaTime = 0;
        private Thread ControlInputThread = null;
        private Thread MotionInputUDPThread = null;
        private Thread DisplayUpdateThread = null;
        private Thread InputRecordingThread = null;
        private Thread ImuUpdateThread = null;
        private static Mutex InputDataMutex = new Mutex();
        private static Mutex MotionInputMutex = new Mutex();
        private static Mutex UpdateMutex = new Mutex();
        private static Mutex ImuMutex = new Mutex();
        private bool RecordingUpdate = false;
        private bool ControlInputUpdate = false;
        private bool MotionInputUpdate = false;
        private bool DisplayUpdate = true;
        private byte ControlInputMode = 0; // 0 - Controller, 1 - Sinusoidal
        private XSens _imuConnection = null;
        
        private MDACommand CurrentMDACommand;
        private float PitchRatio = 0.0f;
        private float RollRatio = 0.0f;
        private float YawRatio = 0.0f;

        private float PitchChange = 0.005f;
        private float RollChange = 0.005f;
        private float YawChange = 0.005f;
        

        private byte[] MotionInputBytes = null;
        private byte[] ControlInputBytes = null;
        private int InputUDPPort = 5345;

        #endregion //Variables

        #region Constructor
        public ControlGUI()
        {
            InitializeComponent(); //C# automatic form Stuff
            CurrentMDACommand = new MDACommand();
            MotionInputBytes = getBytes(CurrentMDACommand);
            InitializeThread();

            cmb_Connection.Items.AddRange(SerialPort.GetPortNames());
            cmb_ArduinoConnection.Items.AddRange(SerialPort.GetPortNames());
        }
        #endregion

        #region Init
        private void InitializeThread()
        {
            Thread.Sleep(250);

            
            //Initialize Display Updates

            try
            {
                DisplayUpdateThread = new Thread(new ThreadStart(UpdateDisplays));
                DisplayUpdateThread.IsBackground = true;
                DisplayUpdateThread.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        
        #endregion

        #region Input

        #region Init
        private void ConnectMotionInputUDPButton_Click(object sender, EventArgs e)
        {
            if (MotionInputUpdate)
            {
                MotionInputUpdate = false;
                InputUDPPortBox.Enabled = true;
                ConnectInputUDPButton.Text = "Connect";
                return;
            }

            if (!Int32.TryParse(InputUDPPortBox.Text, out InputUDPPort))
            {
                InputUDPPort = 5345;
            } //if

            //Initialize communications with Input UDP
            try
            {
                MotionInputUDPThread = new Thread(new ThreadStart(UpdateUDPInputData));
                MotionInputUDPThread.IsBackground = true;
                MotionInputUDPThread.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            InputUDPPortBox.Enabled = false;
            ConnectInputUDPButton.Text = "Disconnect";
        }

        private void CreateInputDataButton_Click(object sender, EventArgs e)
        {
            if (ControlInputUpdate)
            {
                ControlInputUpdate = false;
                OpenFile.Enabled = true;
                recalibrateButton.Enabled = true;
                ControllerInitButton.Enabled = true;
                CreateInputDataButton.Text = "Sinusoidal";
                SinusoidalPanel.Visible = false;
                RollCheckBox.Checked = false;
                PitchCheckBox.Checked = false;
                YawCheckBox.Checked = false;
            }
            else
            {
                SinusoidalPanel.Visible = true;
                ControlInputMode = 1;
                //Set Data
                simulateData.SetRollStatus(RollCheckBox.Checked, float.Parse(RollAmpBox.Text), float.Parse(RollFreqBox.Text));
                simulateData.SetPitchStatus(PitchCheckBox.Checked, float.Parse(PitchAmpBox.Text), float.Parse(PitchFreqBox.Text));
                simulateData.SetYawStatus(YawCheckBox.Checked, float.Parse(YawAmpBox.Text), float.Parse(YawFreqBox.Text));

                //Initialize communications with Input UDP
                try
                {
                    ControlInputThread = new Thread(new ThreadStart(UpdateCreatedData));
                    ControlInputThread.IsBackground = true;
                    ControlInputThread.Start();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                OpenFile.Enabled = false;
                ControllerInitButton.Enabled = false;
                recalibrateButton.Enabled = false;
                CreateInputDataButton.Text = "Stop";
            }
            
        }
        #endregion

        #region MotionInputUDPThread
        /*Function running on InputUDPThread that retrieves 
         *UDP datagrams being sent by UDP meant for the platform */ 
        private void UpdateUDPInputData()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            System.Net.Sockets.UdpClient MotionInputUDPClient = new System.Net.Sockets.UdpClient(InputUDPPort);

            MotionInputUpdate = true;

            while (MotionInputUpdate)
            {
                if (MotionInputUDPClient.Available > 0)
                {
                    try
                    {
                        byte[] recvBytes = MotionInputUDPClient.Receive(ref RemoteIpEndPoint);
                        double secs = DateTime.Now.TimeOfDay.TotalSeconds;

                        MotionInputMutex.WaitOne();
                        MotionInputBytes = recvBytes;
                        MotionInputMutex.ReleaseMutex();

                        recvDeltaTime = secs - BitConverter.ToDouble(recvBytes, Time_Offset);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        break;
                    }

                } //if
            } //while

            MotionInputUDPClient.Close();
        }
        #endregion

        #region CreateDataThread
        private void UpdateCreatedData()
        {
            
            ControlInputUpdate = true;
            t_inputDataStopwatch.Start();

            byte [] recvBytes = simulateData.UpdateData(0);
            
            
            InputDataMutex.WaitOne();
            ControlInputBytes = recvBytes;
            InputDataMutex.ReleaseMutex();

            

            while (ControlInputUpdate)
            {
                recvBytes = simulateData.UpdateData(1.0f * t_inputDataStopwatch.ElapsedMilliseconds / 1000);
                

                InputDataMutex.WaitOne();
                ControlInputBytes = recvBytes;
                InputDataMutex.ReleaseMutex();

                //Send bytes to Arduino
                if (arduino.IsOpen())
                {
                    arduino.SendBytes(recvBytes);

                    if (arduino.ArduinoError())
                    {
                        errorBar.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), errorBar, arduino.GetErrorMessage());
                    }
                }
            }

            t_inputDataStopwatch.Reset();
        }
        #endregion

        #endregion

        #region ImuConnect
        private void cmb_Connection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImuConnectButton.Enabled = true;
        }

        private void AhrsConnect_Click(object sender, EventArgs e)
        {
            if (_imuConnection != null && _imuConnection.IsOpen())
            {
                //Disconnect
                _imuConnection.DisconnectSerial();
                ImuConnectButton.Text = "Connect";
            }
            else
            {
                //Connect
                string portName = cmb_Connection.Text;
                XSens.SerialStatus stat = _imuConnection.ConnectSerial(portName, 38400);

                switch (stat)
                {
                    case XSens.SerialStatus.SUCCESS:
                        //Console.WriteLine("Successfully connected to AHRS.");
                        break;
                    case XSens.SerialStatus.PING_RESPONSE_FAILURE:
                    case XSens.SerialStatus.CONNECTION_FAILURE:
                    case XSens.SerialStatus.PACKET_SIZE_MISSMATCH:
                        //Console.WriteLine(_crossbowConnection.GetErrorMessage());
                        MessageBox.Show("Failed to connect:" + _imuConnection.GetErrorMessage());
                        return;
                    case XSens.SerialStatus.NO_DEVICES_FOUND:
                        MessageBox.Show(_imuConnection.GetErrorMessage());
                        break;
                }

                //Initiate thread for receiving data
                ImuUpdateThread = new Thread(new ThreadStart(_imuConnection.MonitorSerial));
                ImuUpdateThread.IsBackground = true;
                ImuUpdateThread.Start();

                ImuConnectButton.Text = "Disconnect";
            }
        }
        #endregion

        #region UpdateDisplayThread
        private void UpdateDisplays()
        {
            //uint last_io = uint.MaxValue; //Init to impossible value
            //uint last_fault = uint.MaxValue;
            
            Thread.Sleep(1000);

            while (DisplayUpdate)
            {
                try
                {
                    if (0 < ControlInputMode && ControlInputBytes != null)
                    {
                        InputDataMutex.WaitOne();
                        byte[] localBytes = ControlInputBytes;
                        InputDataMutex.ReleaseMutex();

                        RollRatio = BitConverter.ToSingle(localBytes, 0);
                        RollRatioTextBox.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), RollRatioTextBox, RollRatio.ToString("0.000"));

                        PitchRatio = BitConverter.ToSingle(localBytes, 4);
                        PitchRatioTextBox.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), PitchRatioTextBox, PitchRatio.ToString("0.000"));

                        YawRatio = BitConverter.ToSingle(localBytes, 8);
                        YawRatioTextBox.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), YawRatioTextBox, YawRatio.ToString("0.000"));
                    }

                    if (MotionInputUpdate == true)
                    {
                        InputDataMutex.WaitOne();
                        //MDACommand LocalMDACommand = CurrentMDACommand;
                        MDACommand LocalMDACommand = fromBytes(MotionInputBytes);
                        InputDataMutex.ReleaseMutex();

                        UpdateCommandDataDisplay(LocalMDACommand);
                    }

                    if (_imuConnection != null && _imuConnection.IsOpen())
                    {
                        UpdateAhrsDisplay();
                    }

                    Thread.Sleep(50);

                }
                catch (System.Exception ex)
                {
                    //Cause failure of some sorts.
                    //errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                    MessageBox.Show(ex.Message,"Display Update Error");
                }
            }
        }

        private void UpdateAhrsDisplay()
        {
            XSens.ImuDataPacket tmpAhrsPacket = _imuConnection.GetData();

            xbow_xAccel.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_xAccel, (tmpAhrsPacket.a_x).ToString("0.000"));
            xbow_yAccel.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_yAccel, (tmpAhrsPacket.a_y).ToString("0.000"));
            xbow_zAccel.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_zAccel, (tmpAhrsPacket.a_z).ToString("0.000"));
            xbow_pVelo.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_pVelo, (tmpAhrsPacket.v_roll).ToString("0.000"));
            xbow_qVelo.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_qVelo, (tmpAhrsPacket.v_pitch).ToString("0.000"));
            xbow_rVelo.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_rVelo, (tmpAhrsPacket.v_yaw).ToString("0.000"));
            xbow_roll.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_roll, (tmpAhrsPacket.roll).ToString("0.000"));
            xbow_pitch.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_pitch, (tmpAhrsPacket.pitch).ToString("0.000"));
            xbow_yaw.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_yaw, (tmpAhrsPacket.yaw).ToString("0.000"));
        }
        #endregion

        #region DisplayedState
        //Updates displayed platform state
        public delegate void InvokeDelegateState(byte state);
        public delegate void InvokeDelegateLabel(Label localLabel,bool enabled);
        
        private void UpdateCommandDataDisplay(MDACommand LocalMDACommand)
        {
            AxText.BeginInvoke(new InvokeDelegateFloat(UpdateAx), LocalMDACommand.a_x);
            AyText.BeginInvoke(new InvokeDelegateFloat(UpdateAy), LocalMDACommand.a_y);
            AzText.BeginInvoke(new InvokeDelegateFloat(UpdateAz), LocalMDACommand.a_z);

            ArollText.BeginInvoke(new InvokeDelegateFloat(UpdateAroll), LocalMDACommand.a_roll);
            ApitchText.BeginInvoke(new InvokeDelegateFloat(UpdateApitch), LocalMDACommand.a_pitch);
            AyawText.BeginInvoke(new InvokeDelegateFloat(UpdateAyaw), LocalMDACommand.a_yaw);

            VrollText.BeginInvoke(new InvokeDelegateFloat(UpdateVroll), LocalMDACommand.v_roll);
            VpitchText.BeginInvoke(new InvokeDelegateFloat(UpdateVpitch), LocalMDACommand.v_pitch);
            VyawText.BeginInvoke(new InvokeDelegateFloat(UpdateVyaw), LocalMDACommand.v_yaw);

            RollText.BeginInvoke(new InvokeDelegateFloat(UpdateRoll), LocalMDACommand.roll);
            PitchText.BeginInvoke(new InvokeDelegateFloat(UpdatePitch), LocalMDACommand.pitch);
            YawText.BeginInvoke(new InvokeDelegateFloat(UpdateYaw), LocalMDACommand.yaw);

            TimeOffsetText.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), TimeOffsetText, recvDeltaTime.ToString("0.000"));

            if (LocalMDACommand.MCW == 0x36) //Control Input Data sent also
            {
                XPRollRatioText.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), XPRollRatioText, (LocalMDACommand.buffet_roll).ToString());
                XPPitchRatioText.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), XPPitchRatioText, (LocalMDACommand.buffet_pitch).ToString());
                XPYawRatioText.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), XPYawRatioText, (LocalMDACommand.buffet_yaw).ToString());
            }

        }
        private void UpdateLabelStatus(Label localLabel,bool enabled)
        {
            localLabel.Enabled = enabled;
        }
        #endregion

        #region Bytes
        //Returns MDACommand structure from array of bytes
        MDACommand fromBytes(byte[] arr)
        {
            MDACommand str = new MDACommand();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            str = (MDACommand)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        //Returns array of bytes from MDACommand structure to send to MOOG
        byte[] getBytes(MDACommand str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        //Returns MOOGResponse structure from array of bytes
        MOOGResponse fromMOOGBytes(byte[] arr)
        {
            MOOGResponse str = new MOOGResponse();

            str.fault = BitConverter.ToUInt32(flipBytes(arr,0), 0);
            str.io_info = BitConverter.ToUInt32(flipBytes(arr, 4), 0);
            str.status = BitConverter.ToUInt32(flipBytes(arr, 8), 0);
            str.roll = BitConverter.ToSingle(flipBytes(arr, 12), 0);
            str.pitch = BitConverter.ToSingle(flipBytes(arr, 16), 0);
            str.heave = BitConverter.ToSingle(flipBytes(arr, 20), 0);
            str.surge = BitConverter.ToSingle(flipBytes(arr, 24), 0);
            str.yaw = BitConverter.ToSingle(flipBytes(arr, 28), 0);
            str.lateral = BitConverter.ToSingle(flipBytes(arr, 32), 0);

            str.spare = 0;

            return str;
        }

        byte[] flipBytes(float fInitial)
        {
            byte[] arr = BitConverter.GetBytes(fInitial);
            byte[] new_arr = new byte[4];

            for (int i = 0; i < 4; i++ )
            {
                new_arr[i] = arr[3 - i];
            } //for

            return new_arr;
        }

        byte[] flipBytes(byte[] arr)
        {
            byte[] new_arr = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                new_arr[i] = arr[3 - i];
            } //for

            return new_arr;
        }

        byte[] flipStrBytes(MDACommand str)
        {
            byte[] arr = getBytes(str);
            byte[] new_arr = new byte[arr.Length];

            for (int j = 0; j < arr.Length; j = j + 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    new_arr[i + j] = arr[3 - i + j];
                } //for
            } //for

            return new_arr;
        }

        byte[] flipStrBytes(byte[] arr)
        {
            byte[] new_arr = new byte[arr.Length];

            for (int j = 0; j < arr.Length; j = j + 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    new_arr[i + j] = arr[3 - i + j];
                } //for
            } //for

            return new_arr;
        }

        byte[] flipBytes(byte[] arr,int index)
        {
            byte[] new_arr = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                new_arr[i] = arr[3 - i + index];
            } //for

            return new_arr;
        }

        byte[] flipBytes(uint fInitial)
        {
            byte[] arr = BitConverter.GetBytes(fInitial);
            byte[] new_arr = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                new_arr[i] = arr[3 - i];
            } //for

            return new_arr;
        }
        #endregion

        #region Delegates

        #region Floats
        public delegate void InvokeDelegateFloat(float value);
        public void UpdateAx(float value)
        {
            AxText.Text = value.ToString();
        }

        public void UpdateAy(float value)
        {
            AyText.Text = value.ToString();
        }

        public void UpdateAz(float value)
        {
            AzText.Text = value.ToString();
        }

        public void UpdateAroll(float value)
        {
            ArollText.Text = value.ToString();
        }

        public void UpdateApitch(float value)
        {
            ApitchText.Text = value.ToString();
        }
        
        public void UpdateAyaw(float value)
        {
            AyawText.Text = value.ToString();
        }

        public void UpdateVroll(float value)
        {
            VrollText.Text = value.ToString();
        }

        public void UpdateVpitch(float value)
        {
            VpitchText.Text = value.ToString();
        }

        public void UpdateVyaw(float value)
        {
            VyawText.Text = value.ToString();
        }

        public void UpdateRoll(float value)
        {
            RollText.Text = value.ToString();
        }

        public void UpdatePitch(float value)
        {
            PitchText.Text = value.ToString();
        }

        public void UpdateYaw(float value)
        {
            YawText.Text = value.ToString();
        }

        #endregion

        #region Strings
        public delegate void InvokeDelegateString(string value);
        public void ErrorBarMessage(string value)
        {
            errorBar.Text = value;
        }

        public delegate void InvokeDelegateTextString(TextBox tmpTextBox, string value);
        public void UpdateTextBoxValue(TextBox tmpTextBox, string value)
        {
            tmpTextBox.Text = value;
        }

        public delegate void InvokeDelegateComboString(ComboBox tmpComboBox, string value);
        public void UpdateComboBoxValue(ComboBox tmpComboBox, string value)
        {
            tmpComboBox.Text = value;
        }

        public delegate void InvokeDelegateComboIndex(ComboBox tmpComboBox, int index);
        public void UpdateComboBoxIndex(ComboBox tmpComboBox, int value)
        {
            tmpComboBox.SelectedIndex = value;
            tmpComboBox.Text = MOOG_MDAOPTIONSTRINGS[value].ToString();
        }
        #endregion

        public delegate void InvokeDelegateButton(Button localButton, bool enabled);
        private void UpdateButtonStatus(Button localButton, bool enabled)
        {
            localButton.Enabled = enabled;
        }

        public delegate void InvokeDelegateButtonString(Button localButton, string value);
        private void UpdateButtonString(Button localButton, string value)
        {
            localButton.Text = value;
        }
        #endregion

        #region Recorder
        private void recorder()
        {
            //open text file
            Stopwatch elapsedTimer = new Stopwatch();
            DateTime currentTime = DateTime.Now;
            string filename = "KatanaSimEvaluation_" + currentTime.Year.ToString("0000") + currentTime.Month.ToString("00") + currentTime.Day.ToString("00") + "_" + currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") + currentTime.Second.ToString("00") + ".txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, true);

            string line;
            line = "ElapsedTime_ms, RollInputCmd_ratio, PitchInputCmd_ratio, YawInputCmd_ratio, RollInputXP_ratio, PitchInputXP_ratio, YawInputXP_ratio, XAccelModel_mss, YAccelModel_mss , ZAccelModel_mss , " + 
                "RollModel_rad , PitchModel_rad , YawModel_rad , RollVelocityModel_rads , PitchVelocityModel_rads , YawVelocityModel_rads ," +
                " RollAccelModel_radss , PitchAccelModel_radss , YawAccelModel_radss";

            if (_imuConnection != null && _imuConnection.IsOpen())
            {
                line += ", " + _imuConnection.GetLineFormatRads();
            } 
            
            file.WriteLine(line);
            file.Flush();

            int FrameCounter = 0;
            elapsedTimer.Start();
            while (RecordingUpdate)
            {
                System.Threading.Thread.Sleep(20);

                InputDataMutex.WaitOne();
                byte[] dataBytes = MotionInputBytes; 
                InputDataMutex.ReleaseMutex();
                MDACommand data = fromBytes(dataBytes);
                
                
                line = elapsedTimer.ElapsedMilliseconds.ToString() + ", " + RollRatio.ToString() + "," + PitchRatio.ToString() + "," + YawRatio.ToString()
                    + ", " + (data.buffet_roll).ToString() + "," + (data.buffet_pitch).ToString() + "," + (data.buffet_yaw).ToString()
                    + "," + data.a_x.ToString() + ", " + data.a_y.ToString() + ", " + data.a_z.ToString() + ", " + data.roll.ToString() + ", " + data.pitch.ToString() + ", " + data.yaw.ToString() + ", " + data.v_roll.ToString() + ", " + data.v_pitch.ToString() + ", " + data.v_yaw.ToString() + ", " + data.a_roll.ToString() + ", " + data.a_pitch.ToString() + ", " + data.a_yaw.ToString();

                if (_imuConnection != null && _imuConnection.IsOpen())
                {
                    line += ", " + _imuConnection.GetCurrentDataRads();
                } 

                file.WriteLine(line);
                FrameCounter++;

                file.Flush();
            }
            
            file.Close();

        }
        

        private void InputRecording_CheckedChanged(object sender, EventArgs e)
        {
            if (InputRecordingCheckbox.Checked)
            {
                RecordingUpdate = true;
                //Initialize recording of inputs

                try
                {
                    InputRecordingThread = new Thread(new ThreadStart(recorder));
                    InputRecordingThread.IsBackground = true;
                    InputRecordingThread.Start();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    RecordingUpdate = false;
                    return;
                }

            } else 
            {
                RecordingUpdate = false;
            }

        }
        #endregion

        #region AmpFreq

        private void RollCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            float phase = 0.0f;
            float frequency = float.Parse(RollFreqBox.Text);
            if (t_inputDataStopwatch.IsRunning)
            {
                phase = frequency * (1.0f * t_inputDataStopwatch.ElapsedMilliseconds) / 1000;
            }

            simulateData.SetRollStatus(RollCheckBox.Checked, float.Parse(RollAmpBox.Text), frequency, phase);
            
        }

        private void PitchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            float phase = 0.0f;
            float frequency = float.Parse(PitchFreqBox.Text);
            if (t_inputDataStopwatch.IsRunning)
            {
                phase = frequency * (1.0f * t_inputDataStopwatch.ElapsedMilliseconds) / 1000;
            }

            simulateData.SetPitchStatus(PitchCheckBox.Checked, float.Parse(PitchAmpBox.Text), frequency, phase);
            
        }

        private void YawCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            float phase = 0.0f;
            float frequency = float.Parse(YawFreqBox.Text);
            if (t_inputDataStopwatch.IsRunning)
            {
                phase = frequency * (1.0f * t_inputDataStopwatch.ElapsedMilliseconds) / 1000;
            }

            simulateData.SetYawStatus(YawCheckBox.Checked, float.Parse(YawAmpBox.Text), frequency, phase);
        }
        #endregion

        #region Controller
        #region YawInput
        private void YawLeft_MouseDown(object sender, MouseEventArgs e)
        {
            YawChange = -0.002f;
            yawTimer.Enabled = true;
            yawTimer.Start();
        }

        private void YawRight_MouseDown(object sender, MouseEventArgs e)
        {
            YawChange = 0.002f;
            yawTimer.Enabled = true;
            yawTimer.Start();
        }
        
        private void Yaw_MouseUp(object sender, MouseEventArgs e)
        {
            yawTimer.Stop();
        }

        private void yawTimer_Tick(object sender, EventArgs e)
        {
            YawRatio += YawChange;
            if (YawRatio > 1.0f)
            {
                YawRatio = 1.0f;
                YawChange = 0.0f;
            }
            else if (YawRatio < -1.0f)
            {
                YawRatio = -1.0f;
                YawChange = 0.0f;
            }

            YawRatioTextBox.Text = YawRatio.ToString("0.000");
        }
        #endregion //YawInput

        #region RollInput
        private void RollLeft_MouseDown(object sender, MouseEventArgs e)
        {
            RollChange = -0.002f;
            rollTimer.Enabled = true;
            rollTimer.Start();
        }

        private void RollRight_MouseDown(object sender, MouseEventArgs e)
        {
            RollChange = 0.002f;
            rollTimer.Enabled = true;
            rollTimer.Start();
        }

        private void Roll_MouseUp(object sender, MouseEventArgs e)
        {
            rollTimer.Stop();
        }

        private void rollTimer_Tick(object sender, EventArgs e)
        {
            RollRatio += RollChange;
            if (RollRatio > 1.0f)
            {
                RollRatio = 1.0f;
                RollChange = 0.0f;
            }
            else if (RollRatio < -1.0f)
            {
                RollRatio = -1.0f;
                RollChange = 0.0f;
            }

            RollRatioTextBox.Text = RollRatio.ToString("0.000");
        }
        #endregion //RollInput

        #region PitchInput
        private void PitchDown_MouseDown(object sender, MouseEventArgs e)
        {
            PitchChange = -0.002f;
            pitchTimer.Enabled = true;
            pitchTimer.Start();
        }

        private void PitchUp_MouseDown(object sender, MouseEventArgs e)
        {
            PitchChange = 0.002f;
            pitchTimer.Enabled = true;
            pitchTimer.Start();
        }

        private void Pitch_MouseUp(object sender, MouseEventArgs e)
        {
            pitchTimer.Stop();
        }

        private void pitchTimer_Tick(object sender, EventArgs e)
        {
            PitchRatio += PitchChange;
            if (PitchRatio > 1.0f)
            {
                PitchRatio = 1.0f;
                PitchChange = 0.0f;
            }
            else if (PitchRatio < -1.0f)
            {
                PitchRatio = -1.0f;
                PitchChange = 0.0f;
            }

            PitchRatioTextBox.Text = PitchRatio.ToString("0.000");
        }
        #endregion //PitchInput

        private void ControllerInitButton_Click(object sender, EventArgs e)
        {
            if (ControlInputUpdate)
            {
                controllerTimer.Stop();
                controllerTimer.Enabled = false;
                ControlInputUpdate = false;
                OpenFile.Enabled = true;
                CreateInputDataButton.Enabled = true;
                ControllerInitButton.Text = "Controller";
                ControllerPanel.Visible = false;
                recalibrateButton.Enabled = true;
                return;
            }

            ControllerPanel.Visible = true;
            ControlInputUpdate = true;
            ControllerInitButton.Text = "Stop";
            OpenFile.Enabled = false;
            CreateInputDataButton.Enabled = false;
            recalibrateButton.Enabled = false;
            ControlInputMode = 0;
            controllerTimer.Enabled = true;
            controllerTimer.Start();
        }

        private void controllerTimer_Tick(object sender, EventArgs e)
        {
            //Send bytes to Arduino
            if (arduino.IsOpen())
            {
                float[] floatArray = new float[] { RollRatio, PitchRatio, YawRatio };
                byte[] array = new byte[floatArray.Length * 4];
                Buffer.BlockCopy(floatArray, 0, array, 0, array.Length);
                arduino.SendBytes(array);

                if (arduino.ArduinoError())
                {
                    errorBar.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), errorBar, arduino.GetErrorMessage());
                }
            }
        }

        #endregion //Controller

        #region File
        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (ControlInputUpdate)
            {
                ControlInputUpdate = false;
                ControllerInitButton.Enabled = true;
                CreateInputDataButton.Enabled = true;
                recalibrateButton.Enabled = true;
                OpenFile.Text = "New File";
                return;
            }

            if (openInputFileDialog.ShowDialog() != DialogResult.OK)
            {
                errorBar.Text = "No File Opened";
                return;
            }

            if (!testData.ParseData(openInputFileDialog.FileName))
            {
                errorBar.Text = "File incorrect format: " + openInputFileDialog.FileName;
                return;
            }
                
            errorBar.Text = openInputFileDialog.FileName;

            try
            {
                ControlInputThread = new Thread(new ThreadStart(UpdateFileData));
                ControlInputThread.IsBackground = true;
                ControlInputThread.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            ControlInputMode = 2;
            ControlInputUpdate = true;
            CreateInputDataButton.Enabled = false;
            ControllerInitButton.Enabled = false;
            recalibrateButton.Enabled = false;
            OpenFile.Text = "Stop";

        }

        private void UpdateFileData()
        {
            ControlInputUpdate = true;
            

            if (!testData.UpdateData(0))
            {
                return;
            }

            byte[] recvBytes = testData.LatestInputArray();


            InputDataMutex.WaitOne();
            ControlInputBytes = recvBytes;
            InputDataMutex.ReleaseMutex();


            t_inputDataStopwatch.Start();
            while (ControlInputUpdate)
            {
                if (!testData.UpdateData(1.0f * t_inputDataStopwatch.ElapsedMilliseconds))
                {
                    ControlInputUpdate = false;
                    FileStatus(false);
                    break;
                }

                recvBytes = testData.LatestInputArray();

                InputDataMutex.WaitOne();
                ControlInputBytes = recvBytes;
                InputDataMutex.ReleaseMutex();

                //Send bytes to Arduino
                if (arduino.IsOpen())
                {
                    arduino.SendBytes(recvBytes);

                    if (arduino.ArduinoError())
                    {
                        errorBar.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), errorBar, arduino.GetErrorMessage());
                    }
                }
            }

            t_inputDataStopwatch.Reset();
            
        }

        private void FileStatus(bool enabled)
        {
            string text = "";
            if (enabled)
            {
                text = "Stop";
            }
            else
            {
                text = "New File";
            }

            CreateInputDataButton.BeginInvoke(new InvokeDelegateButton(UpdateButtonStatus), CreateInputDataButton, !enabled);
            ControllerInitButton.BeginInvoke(new InvokeDelegateButton(UpdateButtonStatus), ControllerInitButton, !enabled);
            recalibrateButton.BeginInvoke(new InvokeDelegateButton(UpdateButtonStatus), recalibrateButton, !enabled);
            OpenFile.BeginInvoke(new InvokeDelegateButtonString(UpdateButtonString), OpenFile, text);
            
        }

        #endregion

        private void recalibrateButton_Click(object sender, EventArgs e)
        {
            //Send bytes to Arduino
            if (arduino.IsOpen())
            {
                float[] floatArray;
                byte[] array = new byte[3 * 4];

                floatArray = new float[] { -1.0f, -1.0f, -1.0f };
                Buffer.BlockCopy(floatArray, 0, array, 0, array.Length);
                arduino.SendBytes(array); //Send several times to ensure goes through
                arduino.SendBytes(array);
                arduino.SendBytes(array);
                arduino.SendBytes(array);
                arduino.SendBytes(array);

                floatArray = new float[] { 1.0f, 1.0f, 1.0f };
                Buffer.BlockCopy(floatArray, 0, array, 0, array.Length);
                arduino.SendBytes(array); //Send several times to ensure goes through
                arduino.SendBytes(array);
                arduino.SendBytes(array);
                arduino.SendBytes(array);
                arduino.SendBytes(array);
            }
        }

        private void cmb_ArduinoConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArduinoConnectButton.Enabled = true;
        }

        private void ArduinoConnect_Click(object sender, EventArgs e)
        {
            if (arduino.IsOpen())
            {
                arduino.DisconnectSerial();
                ArduinoConnectButton.Text = "Connect";
            }
            else
            {
                string portName = cmb_ArduinoConnection.Text;
                SerialStatus stat = arduino.ConnectSerial(portName, 38400);

                switch (stat)
                {
                    case SerialStatus.SUCCESS:
                        //Console.WriteLine("Successfully connected to AHRS.");
                        break;
                    case SerialStatus.PING_RESPONSE_FAILURE:
                    case SerialStatus.CONNECTION_FAILURE:
                    case SerialStatus.PACKET_SIZE_MISSMATCH:
                        //Console.WriteLine(_crossbowConnection.GetErrorMessage());
                        MessageBox.Show("Failed to connect:" + arduino.GetErrorMessage());
                        return;
                }

                ArduinoConnectButton.Text = "Disconnect";
            } //if
        }
        
        #region UnitConversion

        #endregion


    }
}
