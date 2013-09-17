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

namespace MotionPlatformControl
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

        private byte[] MOOG_MDAOPTIONS = new byte[6] { 101, 102, 103, 104, 105, 106};
        private ComboBox.ObjectCollection MOOG_MDAOPTIONSTRINGS = null;
        #endregion

        #region Variables
        private const double GRAVITY = 9.80;
        private const double Deg2Rad = Math.PI / 180.0;
        private double recvDeltaTime = 0;
        private double sendDeltaTime = 0;
        private double meanDeltaTime = 0;
        private Label[] DiscreteIO = new Label[8];
        private Label[] Faults = new Label[16];
        private Thread InputUDPThread = null;
        private Thread PlatformUDPThread = null;
        private Thread DisplayUpdateThread = null;
        private Thread InputRecordingThread = null;
        private Thread CrossbowUpdateThread = null;
        private static Mutex InputDataMutex = new Mutex();
        private static Mutex MOOGMutex = new Mutex();
        private static Mutex UpdateMutex = new Mutex();
        private static Mutex CrossbowMutex = new Mutex();
        private bool RecordingUpdate = false;
        private bool InputUpdate = true;
        private bool PlatformUpdate = true;
        private uint PlatformIO = 0;
        private uint PlatformFault = 0;
        private byte PlatformState = 0;
        private byte PlatformLastState = 16;
        private byte RequestedState = MOOG_NEWMDAFILE;
        private byte RequestedMDAFile = 102;
        private MDACommand CurrentMDACommand;
        private MOOGResponse CurrentMOOGResponse;
        private AhrsSerialData _crossbowConnection = null;

        private int MoogSendStateCount = 0;
        private Stopwatch MoogElapsedSendState = new Stopwatch();
        private string MOOGhostname = "255.255.255.255";
        private int MOOGdestport = 992; //Platform sends and receives on different ports
        private IPEndPoint MOOGRemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 991);
        private System.Net.Sockets.UdpClient MOOGPlatformRcvUDP = new System.Net.Sockets.UdpClient(992);
        private int MoogSendTimeOutMS = 16; //ms

        private byte[] XplaneBytes = null;

        #endregion //Variables

        #region Constructor
        public ControlGUI()
        {
            InitializeComponent(); //C# automatic form Stuff
            CurrentMDACommand = new MDACommand();
            XplaneBytes = getBytes(CurrentMDACommand);
            InitLabelArrays();
            InitializeThread();

            this.splitContainer1.SplitterDistance = crossbowCheck.Checked ? 0 : 180;
            cmb_Connection.Items.AddRange(SerialPort.GetPortNames());
            _crossbowConnection = new AhrsSerialData(CrossbowMutex);
        }
        #endregion

        #region Init
        private void InitializeThread()
        {
            Thread.Sleep(250);

            //Initialize communications with X-Plane
            try
            {
                InputUDPThread = new Thread(new ThreadStart(UpdateUDPInputData));
                InputUDPThread.IsBackground = true;
                InputUDPThread.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            
            //Initialize Communications with Platform
            
            try
            {
                PlatformUDPThread = new Thread(new ThreadStart(PlatformCommuncation));
                PlatformUDPThread.IsBackground = true;
                PlatformUDPThread.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

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

            MoogElapsedSendState.Start();
        }

        private void InitLabelArrays()
        {
            //Sixteen Fault Labels
            Faults[0] = Fault_0;
            Faults[1] = Fault_1;
            Faults[2] = Fault_2;
            Faults[3] = Fault_3;
            Faults[4] = Fault_4;
            Faults[5] = Fault_5;
            Faults[6] = Fault_6;
            Faults[7] = Fault_7;
            Faults[8] = Fault_8;
            Faults[9] = Fault_9;
            Faults[10] = Fault_10;
            Faults[11] = Fault_11;
            Faults[12] = Fault_12;
            Faults[13] = Fault_13;
            Faults[14] = Fault_14;
            Faults[15] = Fault_15;

            //Eight Discrete IO Labels
            DiscreteIO[0] = Discrete_0;
            DiscreteIO[1] = Discrete_1;
            DiscreteIO[2] = Discrete_2;
            DiscreteIO[3] = Discrete_3;
            DiscreteIO[4] = Discrete_4;
            DiscreteIO[5] = Discrete_5;
            DiscreteIO[6] = Discrete_6;
            DiscreteIO[7] = Discrete_7;

            //Update MDA String list
            MOOG_MDAOPTIONSTRINGS = MDAFileBox.Items;
        }
        #endregion

        #region InputUDPThread
        /*Function running on InputUDPThread that retrieves 
         *UDP datagrams being sent by UDP meant for the platform */ 
        private void UpdateUDPInputData()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            System.Net.Sockets.UdpClient InputUDPClient = new System.Net.Sockets.UdpClient(5345);

            //Stopwatch freqTimer = new Stopwatch();
            //double currentFrequency = 0.0;
            //int recvCount = 0;

            UpdateMutex.WaitOne();
            byte LocalRequestedState = RequestedState;
            UpdateMutex.ReleaseMutex();

            //freqTimer.Start();

            while (InputUpdate)
            {
                if (InputUDPClient.Available > 0)
                {
                    try
                    {
                        byte[] recvBytes = InputUDPClient.Receive(ref RemoteIpEndPoint);
                        double secs = DateTime.Now.TimeOfDay.TotalSeconds;

                        if (LocalRequestedState == MOOG_NEWMDA && MoogElapsedSendState.ElapsedMilliseconds >= MoogSendTimeOutMS)
                        {
                            //Send updated state
                            SendState(MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport, recvBytes);
                            MoogSendStateCount++;
                            MoogElapsedSendState.Reset();
                            MoogElapsedSendState.Start();
                        }

                        InputDataMutex.WaitOne();
                        XplaneBytes = recvBytes;
                        InputDataMutex.ReleaseMutex();

                        recvDeltaTime = secs - BitConverter.ToDouble(recvBytes, Time_Offset);

                        //recvCount++;
                        //if (recvCount >= 500)
                        //{
                        //    long elapsedTime = freqTimer.ElapsedMilliseconds;
                        //    currentFrequency = 1000.0 * recvCount / elapsedTime;
                        //    recvCount = 0;
                        //    freqTimer.Reset();
                        //    freqTimer.Start();
                        //    errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), "Freq: " + currentFrequency.ToString("000.00") + " Hz");
                        //}
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        break;
                    }

                } //if
                else
                {
                    UpdateMutex.WaitOne();
                    LocalRequestedState = RequestedState;
                    UpdateMutex.ReleaseMutex();
                }
            } //while

            InputUDPClient.Close();
        }

        
        #endregion

        #region PlatformThread
        /*Function running on PlatformUDPThread that communicates
         *with MOOG platform by UDP datagrams*/
        private void PlatformCommuncation()
        {
            Stopwatch freqTimer = new Stopwatch();
            Stopwatch elapsedTimer = new Stopwatch();
            double currentFrequency = 0.0;
            int sendCount = 0;
            

            MDACommand platformCommand = new MDACommand();
            platformCommand.MCW = (uint) MOOG_ENGAGE;
            byte[] sendBytes = flipStrBytes(platformCommand);

            MOOGhostname = MOOGRemoteIpEndPoint.Address.ToString();
            MOOGdestport = MOOGRemoteIpEndPoint.Port; //Platform sends and receives on different ports

            MOOGPlatformRcvUDP.Send(sendBytes, sendBytes.Length, MOOGhostname,MOOGdestport);

            //bool bSetMode = false;
            byte lastMDAFile = 101;
            freqTimer.Start();
            elapsedTimer.Start();

            #region Loop
            while (PlatformUpdate)
            {
                #region RcvPlatformPacket
                if (MOOGPlatformRcvUDP.Available > 0)
                {
                    try
                    {
                        byte[] recvBytes = MOOGPlatformRcvUDP.Receive(ref MOOGRemoteIpEndPoint);
                        MOOGResponse platformResp = fromMOOGBytes(recvBytes);

                        //check for faults?
                        
                        //Update Status
                        //UpdateFeedbackState(platformResp);
                        MOOGMutex.WaitOne();
                        CurrentMOOGResponse = platformResp;
                        MOOGMutex.ReleaseMutex();

                        byte[] RespStatus = BitConverter.GetBytes(platformResp.status);
                        if (RespStatus[1] != lastMDAFile && errorBar != null)
                        {
                            lastMDAFile = RespStatus[1];
                            try
                            {
                                int index = Array.FindIndex(MOOG_MDAOPTIONS, item => item == lastMDAFile);
                                MDAFileBox.BeginInvoke(new InvokeDelegateComboIndex(UpdateComboBoxIndex), MDAFileBox, index);
                                //MDAFileBox.BeginInvoke(new InvokeDelegateComboString(UpdateComboBoxValue), MDAFileBox, lastMDAFile.ToString("000"));
                                //errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), "Current MDA File: MDA" + lastMDAFile.ToString("000") + ".in");
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Change of MDA File Error");
                            }
                        }
                        

                        PlatformFault = platformResp.fault;
                        PlatformIO = platformResp.io_info;
                        PlatformState = (byte)(platformResp.status % 16);

                        //Update displayed state
                        if (PlatformState != PlatformLastState && StatusText != null)
                        {
                            try 
                            {
                                StatusText.BeginInvoke(new InvokeDelegateState(UpdatePlatformState), PlatformState);
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Status Update Error");
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Platform Receiving Communication Error");
                        break;
                    }
                } //if
                #endregion

                UpdateMutex.WaitOne();
                byte LocalRequestedState = RequestedState;
                UpdateMutex.ReleaseMutex();

                #region SendPlatformPacket
                //Only run if significant time has passed
                if (elapsedTimer.ElapsedMilliseconds >= MoogSendTimeOutMS || (LocalRequestedState == MOOG_NEWMDA && MoogElapsedSendState.ElapsedMilliseconds >= MoogSendTimeOutMS))
                {
                    try
                    {

                        if (LocalRequestedState == MOOG_DISABLE)
                        {
                            //Send Disable Command
                            SendCommand(MOOG_DISABLE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                        }

                        switch (PlatformState)
                        {
                            case (byte)MachineStates.ENGAGED:
                                //Send updated state
                                if (LocalRequestedState == MOOG_NEWMDA && MoogElapsedSendState.ElapsedMilliseconds >= MoogSendTimeOutMS)
                                {
                                    //Send updated state
                                    InputDataMutex.WaitOne();
                                    byte[] localBytes = XplaneBytes;
                                    InputDataMutex.ReleaseMutex();
                                    SendState(MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport,localBytes);
                                    //SendCommand(MOOG_MDAMODE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                    MoogSendStateCount++;
                                    MoogElapsedSendState.Reset();
                                    MoogElapsedSendState.Start();
                                }
                                else if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                else if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    SendCommand(MOOG_ENGAGE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }//if
                                else if (LocalRequestedState == MOOG_MDAMODE)
                                {
                                    SendCommand(MOOG_MDAMODE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                } //Keep alive

                                //Display feedback data
                                break;
                            case (byte)MachineStates.POWERUP:
                                //Send MDA Mode command
                                if (LocalRequestedState == MOOG_INHIBIT)
                                {
                                    //Send Inhibit Command
                                    SendCommand(MOOG_INHIBIT, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                else if (LocalRequestedState == MOOG_NEWMDAFILE)
                                {
                                    //Request new mda file
                                    SendMDAFileChange(MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                    if (RequestedMDAFile == lastMDAFile)
                                    {
                                        UpdateMutex.WaitOne();
                                        RequestedState = MOOG_MDAMODE;
                                        UpdateMutex.ReleaseMutex();
                                    }
                                }
                                else //Keep connection alive
                                {
                                    SendCommand(MOOG_MDAMODE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                break;
                            case (byte)MachineStates.PARKING:
                                if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                break;
                            case (byte)MachineStates.IDLE:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    SendCommand(MOOG_ENGAGE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }//if
                                else if (LocalRequestedState == MOOG_INHIBIT)
                                {
                                    //Send Inhibit Command
                                    SendCommand(MOOG_INHIBIT, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                else if (LocalRequestedState == MOOG_NEWMDAFILE)
                                {
                                    //Request new mda file
                                    SendMDAFileChange(MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                    if (RequestedMDAFile == lastMDAFile)
                                    {

                                        UpdateMutex.WaitOne();
                                        RequestedState = MOOG_MDAMODE;
                                        UpdateMutex.ReleaseMutex();
                                    }
                                }
                                else //Keep connection alive
                                {
                                    SendCommand(MOOG_MDAMODE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                break;
                            case (byte)MachineStates.STANDBY:
                                if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                else if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    //SendState(PlatformRcvUDP, hostname, dest_port);
                                    SendCommand(MOOG_ENGAGE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }//if
                                else //Keep connection alive
                                {
                                    SendCommand(MOOG_MDAMODE, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                break;
                            case (byte)MachineStates.FAULT2:
                            case (byte)MachineStates.INHIBITED:
                                //Need to tell user to issue Reset Command
                                if (LocalRequestedState == MOOG_RESET)
                                {
                                    //Send Reset command
                                    SendCommand(MOOG_RESET, MOOGPlatformRcvUDP, MOOGhostname, MOOGdestport);
                                }
                                break;
                        }

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Platform Sending Communication Error");
                        break;
                    }

                    #region UpdateTimeout
                    sendCount++;
                    if (sendCount >= 500)
                    {
                        //Ensures Consistent Platform Communications
                        long elapsedTime = freqTimer.ElapsedMilliseconds;
                        currentFrequency = 1000.0 * sendCount / elapsedTime;
                        sendCount = 0;

                        if (currentFrequency > 100.0) //Going far too fast
                        {
                            MoogSendTimeOutMS += 3;
                        }
                        else if (currentFrequency > 70.0) //Going too fast
                        {
                            MoogSendTimeOutMS += 2;
                        }
                        else if (currentFrequency > 62.0) //Going too fast
                        {
                            MoogSendTimeOutMS += 1;
                        }
                        else if (currentFrequency < 58.0) //Can speed up a bit
                        {
                            MoogSendTimeOutMS -= 1;
                        }

                        //Update Send State Frequency
                        if (MoogSendStateCount > 100)
                        {
                            currentFrequency = 1000.0 * MoogSendStateCount / elapsedTime;
                            errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), "Send State Freq: " + currentFrequency.ToString("000.00") + " Hz");

                            meanDeltaTime = sendDeltaTime / MoogSendStateCount;
                            SendDeltaTimeTextbox.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), SendDeltaTimeTextbox, meanDeltaTime.ToString("0.000"));
                        }
                        MoogSendStateCount = 0;
                        sendDeltaTime = 0;

                        freqTimer.Reset();
                        freqTimer.Start();
                    }
                    #endregion

                    elapsedTimer.Reset();
                    elapsedTimer.Start();
                } //if timer elapsed
                #endregion
            }//while
            #endregion 
        }
        #endregion

        #region UpdateDisplayThread
        private void UpdateDisplays()
        {
            uint last_io = uint.MaxValue; //Init to impossible value
            uint last_fault = uint.MaxValue;
            
            Thread.Sleep(1000);

            while (PlatformUpdate)
            {
                try
                {

                    MOOGMutex.WaitOne();
                    MOOGResponse local_resp = CurrentMOOGResponse;
                    MOOGMutex.ReleaseMutex();

                    if (last_fault != local_resp.fault)
                    {
                        UpdateFaults(local_resp.fault);
                        last_fault = local_resp.fault;
                    }

                    if (last_io != local_resp.io_info)
                    {
                        UpdateDiscreteIO(local_resp.io_info);
                        last_io = local_resp.io_info;
                    }

                    UpdateFeedbackState(local_resp);

                    InputDataMutex.WaitOne();
                    //MDACommand LocalMDACommand = CurrentMDACommand;
                    MDACommand LocalMDACommand = fromBytes(XplaneBytes);
                    InputDataMutex.ReleaseMutex();

                    UpdateCommandDataDisplay(LocalMDACommand);

                    if (_crossbowConnection != null && _crossbowConnection.IsOpen())
                    {
                        UpdateCrossbowDisplay();
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

        private void UpdateDiscreteIO(uint local_io)
        {
            for (int i = 0; i < 8; i++ )
            {
                bool enabled = 0 < (local_io & (uint)(1 << i));
                DiscreteIO[i].BeginInvoke(new InvokeDelegateLabel(UpdateLabelStatus), DiscreteIO[i], enabled);
            }
        }

        private void UpdateFaults(uint local_fault)
        {
            for (int i = 0; i < 16; i++)
            {
                bool enabled = 0 < (local_fault & (uint)(1 << i));
                Faults[i].BeginInvoke(new InvokeDelegateLabel(UpdateLabelStatus), Faults[i], enabled);
            }
        }

        private void UpdateCrossbowDisplay()
        {
            AHRSDataPacket tmpCrossbowPacket = _crossbowConnection.GetData();

            xbow_xAccel.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_xAccel, (tmpCrossbowPacket.a_x*GRAVITY).ToString("0.000"));
            xbow_yAccel.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_yAccel, (tmpCrossbowPacket.a_y*GRAVITY).ToString("0.000"));
            xbow_zAccel.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_zAccel, (tmpCrossbowPacket.a_z*GRAVITY).ToString("0.000"));
            xbow_pVelo.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_pVelo, (tmpCrossbowPacket.v_roll * Deg2Rad).ToString("0.000"));
            xbow_qVelo.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_qVelo, (tmpCrossbowPacket.v_pitch*Deg2Rad).ToString("0.000"));
            xbow_rVelo.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_rVelo, (tmpCrossbowPacket.v_yaw*Deg2Rad).ToString("0.000"));
            xbow_roll.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_roll, (tmpCrossbowPacket.roll*Deg2Rad).ToString("0.000"));
            xbow_pitch.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_pitch, (tmpCrossbowPacket.pitch*Deg2Rad).ToString("0.000"));
            xbow_yaw.BeginInvoke(new InvokeDelegateTextString(UpdateTextBoxValue), xbow_yaw, (tmpCrossbowPacket.yaw*Deg2Rad).ToString("0.000"));
        }
        #endregion

        #region SendToMoog
        //Sends most recent MDA command
        private void SendState(System.Net.Sockets.UdpClient PlatformSendUDP, string hostname, int port, byte[] localBytes)
        {
            byte[] sendBytes = flipStrBytes(localBytes);

            try
            {
                sendDeltaTime += DateTime.Now.TimeOfDay.TotalSeconds - BitConverter.ToDouble(localBytes, Time_Offset);
                PlatformSendUDP.Send(sendBytes, sendBytes.Length, hostname, port);
            }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.Message,"Send State Error");
            }
        }

        //Sends desired command
        private void SendCommand(byte command, System.Net.Sockets.UdpClient PlatformSendUDP, string hostname, int port)
        {
            MDACommand platformCommand = new MDACommand();

            platformCommand.MCW = (uint) command;

            byte[] sendBytes = flipStrBytes(platformCommand);

            try
            {
                PlatformSendUDP.Send(sendBytes, sendBytes.Length, hostname, port);
                //PlatformSendUDP.Send(sendBytes, sendBytes.Length);
            }
            catch (System.Exception ex)
            {
                //Cause failure of some sorts.
                //errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                MessageBox.Show(ex.Message,"Send Command Error");
            }
        }

        //Sends requested MDA File
        private void SendMDAFileChange(System.Net.Sockets.UdpClient PlatformSendUDP, string hostname, int port)
        {
            MDACommand platformCommand = new MDACommand();

            platformCommand.MCW = BitConverter.ToUInt32(new byte[4] {MOOG_NEWMDAFILE, RequestedMDAFile, 0, 0 }, 0);

            byte[] sendBytes = flipStrBytes(platformCommand);

            try
            {
                PlatformSendUDP.Send(sendBytes, sendBytes.Length, hostname, port);
                //PlatformSendUDP.Send(sendBytes, sendBytes.Length);
            }
            catch (System.Exception ex)
            {
                //Cause failure of some sorts.
                //errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                MessageBox.Show(ex.Message, "Send Command Error");
            }
        }
        #endregion

        #region DisplayedState
        //Updates displayed platform state
        public delegate void InvokeDelegateState(byte state);
        public delegate void InvokeDelegateLabel(Label localLabel,bool enabled);
        public void UpdatePlatformState(byte state)
        {
            string stateString = "";
            StatusText.BackColor = Color.White;

            switch (state)
            {
                case (byte)MachineStates.ENGAGED:
                    stateString = "Engaged";
                    break;
                case (byte)MachineStates.POWERUP:
                    stateString = "Powering Up";
                    break;
                case (byte)MachineStates.PARKING:
                    stateString = "Parking";
                    break;
                case (byte)MachineStates.IDLE:
                    stateString = "Idle";
                    break;
                case (byte)MachineStates.STANDBY:
                    stateString = "Standby";
                    break;
                case (byte)MachineStates.FAULT2:
                case (byte)MachineStates.INHIBITED:
                    stateString = "Press Reset!";
                    StatusText.BackColor = Color.Yellow;
                    break;
                case (byte)MachineStates.DISABLED:
                    stateString = "Disabled; Cycle Power";
                    StatusText.BackColor = Color.Red;
                    break;
            }

            StatusText.Text = stateString;
        }
        public void UpdateFeedbackState(MOOGResponse platformResp)
        {
            PPitchText.BeginInvoke(new InvokeDelegateFloat(UpdatePPitch), platformResp.pitch);
            PRollText.BeginInvoke(new InvokeDelegateFloat(UpdatePRoll), platformResp.roll);
            PYawText.BeginInvoke(new InvokeDelegateFloat(UpdatePYaw), platformResp.yaw);

            PSurgeText.BeginInvoke(new InvokeDelegateFloat(UpdatePSurge), platformResp.surge);
            PSwayText.BeginInvoke(new InvokeDelegateFloat(UpdatePSway), platformResp.lateral);
            PHeaveText.BeginInvoke(new InvokeDelegateFloat(UpdatePHeave), platformResp.heave);
        }
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

        public void UpdatePRoll(float value)
        {
            PRollText.Text = value.ToString();
        }

        public void UpdatePPitch(float value)
        {
            PPitchText.Text = value.ToString();
        }

        public void UpdatePYaw(float value)
        {
            PYawText.Text = value.ToString();
        }

        public void UpdatePSurge(float value)
        {
            PSurgeText.Text = value.ToString();
        }

        public void UpdatePSway(float value)
        {
            PSwayText.Text = value.ToString();
        }

        public void UpdatePHeave(float value)
        {
            PHeaveText.Text = value.ToString();
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

        #endregion

        #region PlatformState
        private void PlatformStatusChange(object sender, EventArgs e)
        {
            MOOGMutex.WaitOne();
            byte LocalPlatformState = PlatformState;
            MOOGMutex.ReleaseMutex();

            switch (LocalPlatformState)
            {
                case (byte)MachineStates.ENGAGED:
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = true;
                    InhibitButton.Enabled = false;
                    RunButton.Enabled = true;
                    MDAFileBox.Enabled = false;
                    //Display feedback data
                    break;
                case (byte)MachineStates.POWERUP:
                    //Do nothing and wait
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    InhibitButton.Enabled = true;
                    RunButton.Enabled = false;
                    MDAFileBox.Enabled = true;
                    RunButton.Text = "Run";
                    break;
                case (byte)MachineStates.PARKING:
                    //Do nothing and wait
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    InhibitButton.Enabled = false;
                    RunButton.Enabled = false;
                    MDAFileBox.Enabled = false;
                    RunButton.Text = "Run";
                    break;
                case (byte)MachineStates.IDLE:
                    //Send updated state
                    EngageButton.Enabled = true;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    InhibitButton.Enabled = true;
                    RunButton.Enabled = false;
                    MDAFileBox.Enabled = true;
                    RunButton.Text = "Run";
                    break;
                case (byte)MachineStates.STANDBY:
                    //Send updated state
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = true;
                    InhibitButton.Enabled = false;
                    RunButton.Enabled = false;
                    MDAFileBox.Enabled = false;
                    RunButton.Text = "Run";
                    break;
                case (byte)MachineStates.FAULT2:
                case (byte)MachineStates.INHIBITED:
                    //Need to tell user to issue Reset Command
                    ResetButton.Enabled = true;
                    EngageButton.Enabled = false;
                    ParkButton.Enabled = false;
                    InhibitButton.Enabled = false;
                    RunButton.Enabled = false;
                    MDAFileBox.Enabled = false;
                    RunButton.Text = "Run";
                    break;
                case (byte)MachineStates.DISABLED:
                    //Warn user a hard reset is required
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    InhibitButton.Enabled = false;
                    RunButton.Enabled = false;
                    MDAFileBox.Enabled = false;
                    RunButton.Text = "Run";
                    break;
            }
        }

        private void EngageButton_Click(object sender, EventArgs e)
        {
            UpdateMutex.WaitOne();
            RequestedState = MOOG_ENGAGE;
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "Moving to Engage";
        }

        private void ParkButton_Click(object sender, EventArgs e)
        {
            UpdateMutex.WaitOne();
            RequestedState = MOOG_PARK;
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "Moving to Park";
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            UpdateMutex.WaitOne();
            RequestedState = MOOG_RESET;
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "Moving to Reset";
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (RequestedState == MOOG_NEWMDA)
            {   //Pause
                UpdateMutex.WaitOne();
                RequestedState = MOOG_MDAMODE;
                UpdateMutex.ReleaseMutex();

                RStatusState.Text = "Moving to Pause";
                RunButton.Text = "Run";
            }
            else
            {
                UpdateMutex.WaitOne();
                RequestedState = MOOG_NEWMDA;
                UpdateMutex.ReleaseMutex();
                RStatusState.Text = "Moving to Run";
                RunButton.Text = "Pause";
            }

            
        }

        private void InhibitButton_Click(object sender, EventArgs e)
        {
            UpdateMutex.WaitOne();
            RequestedState = MOOG_INHIBIT;
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "Moving to Inhibit";
        }

        private void DisableButton_Click(object sender, EventArgs e)
        {
            UpdateMutex.WaitOne();
            RequestedState = MOOG_DISABLE;
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "Moving to Disable";
        }
        #endregion

        #region Recorder
        private void recorder()
        {
            //open text file
            Stopwatch elapsedTimer = new Stopwatch();
            DateTime currentTime = DateTime.Now;
            string filename = "XPlaneFlightData_" + currentTime.Year.ToString("0000") + currentTime.Month.ToString("00") + currentTime.Day.ToString("00") + "_" + currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") + currentTime.Second.ToString("00") + ".txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, true);

            //make label for top of file
            //string time;
            //time = currentTime.Hour.ToString() + ":" + currentTime.Minute.ToString("00") + ":" + currentTime.Second.ToString("00")
            //    + "." + currentTime.Millisecond.ToString("000");

            //file.WriteLine("Date: " + currentTime.ToLongDateString());
            //file.WriteLine("Time: " + time);

            string line;
            line = "Elapsed Time (ms), X Acceleration (m/s/s), Y Acceleration (m/s/s) , Z Acceleration (m/s/s) , Roll (rad) , Pitch (rad) , Yaw (rad) , Roll Velocity (rad/s) , Pitch Velocity (rad/s) , Yaw Velocity (rad/s) , Roll Acceleration (rad/s/s) , Pitch Acceleration (rad/s/s) , Yaw Acceleration (rad/s/s)";
            line += ", Moog Roll (rad), Moog Pitch (rad), Moog Yaw (rad)";
            if (_crossbowConnection != null && _crossbowConnection.IsOpen())
            {
                line += ", " + _crossbowConnection.GetLineFormatRads();
            } 
            file.WriteLine(line);
            file.Flush();

            int FrameCounter = 0;
            elapsedTimer.Start();
            while (RecordingUpdate)
            {
                System.Threading.Thread.Sleep(20);

                InputDataMutex.WaitOne();
                byte[] dataBytes = XplaneBytes; 
                InputDataMutex.ReleaseMutex();
                MDACommand data = fromBytes(dataBytes);
                

                MOOGMutex.WaitOne();
                MOOGResponse local_resp = CurrentMOOGResponse;
                MOOGMutex.ReleaseMutex();
                
                line = elapsedTimer.ElapsedMilliseconds.ToString() + ", " + data.a_x.ToString() + ", " + data.a_y.ToString() + ", " + data.a_z.ToString() + ", " + data.roll.ToString() + ", " + data.pitch.ToString() + ", " + data.yaw.ToString() + ", " + data.v_roll.ToString() + ", " + data.v_pitch.ToString() + ", " + data.v_yaw.ToString() + ", " + data.a_roll.ToString() + ", " + data.a_pitch.ToString() + ", " + data.a_yaw.ToString();
                line += ", " + local_resp.roll.ToString() + ", " + local_resp.pitch.ToString() + ", " + local_resp.yaw.ToString();
                if (_crossbowConnection != null && _crossbowConnection.IsOpen())
                {
                    line += ", " + _crossbowConnection.GetCurrentDataRads();
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

        #region MDAFileChange
        private void MDAFileBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = MDAFileBox.SelectedIndex;
            UpdateMutex.WaitOne();
            RequestedState = MOOG_NEWMDAFILE;
            RequestedMDAFile = MOOG_MDAOPTIONS[index];
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "MDA" + RequestedMDAFile.ToString("000") + ".in";
        }
        #endregion

        #region CrossbowConnect
        private void cmb_Connection_SelectedIndexChanged(object sender, EventArgs e)
        {
            CrossbowConnectButton.Enabled = true;
        }

        private void crossbow_show_panel(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = crossbowCheck.Checked ? 0 : 180;
        }

        private void CrossbowConnect_Click(object sender, EventArgs e)
        {
            if (_crossbowConnection != null && _crossbowConnection.IsOpen())
            {
                //Disconnect
                _crossbowConnection.DisconnectSerial();
                CrossbowConnectButton.Text = "Connect";
            }
            else
            {
                //Connect
                string portName = cmb_Connection.Text;
                SerialStatus stat = _crossbowConnection.ConnectSerial(portName, 38400);
                                
                switch (stat)
                {
                    case SerialStatus.SUCCESS:
                        //Console.WriteLine("Successfully connected to AHRS.");
                        break;
                    case SerialStatus.PING_RESPONSE_FAILURE:
                    case SerialStatus.CONNECTION_FAILURE:
                    case SerialStatus.PACKET_SIZE_MISSMATCH:
                        //Console.WriteLine(_crossbowConnection.GetErrorMessage());
                        MessageBox.Show("Failed to connect:" + _crossbowConnection.GetErrorMessage());
                        return;
                }

                //Initiate thread for receiving data
                CrossbowUpdateThread = new Thread(new ThreadStart(_crossbowConnection.MonitorSerial));
                CrossbowUpdateThread.IsBackground = true;
                CrossbowUpdateThread.Start();

                CrossbowConnectButton.Text = "Disconnect";
            }
        }
        #endregion

        #region UnitConversion

        #endregion

        
    }
}
