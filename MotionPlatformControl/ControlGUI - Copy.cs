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

        public float a_roll;			// rad/s2
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

        public uint spare1;
        public uint spare2;
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
        //Motion Command Words
        const byte MOOG_DISABLE = 0x00DC;
        const byte MOOG_PARK = 0xD2;
        const byte MOOG_ENGAGE = 0xB4;
        const byte MOOG_RESET = 0xA0;
        const byte MOOG_MDAMODE = 0x8C;
        const byte MOOG_NEWMDA = 0x80; //New MDA accelerations
        #endregion

        #region Variables
        private Label[] DiscreteIO = new Label[8];
        private Label[] Faults = new Label[16];
        private Thread InputUDPThread = null;
        private Thread PlatformUDPThread = null;
        private Thread DisplayUpdateThread = null;
        private static Mutex InputDataMutex = new Mutex();
        private static Mutex MOOGMutex = new Mutex();
        private static Mutex UpdateMutex = new Mutex();
        private bool InputUpdate = true;
        private bool PlatformUpdate = true;
        private uint PlatformIO = 0;
        private uint PlatformFault = 0;
        private byte PlatformState = 0;
        private byte PlatformLastState = 16;
        private byte RequestedState = 0;
        private MDACommand CurrentMDACommand;
        private MOOGResponse CurrentMOOGResponse;
        #endregion //Variables

        #region Constructor
        public ControlGUI()
        {
            InitializeComponent(); //C# automatic form Stuff
            CurrentMDACommand = new MDACommand();
            InitLabelArrays();
            InitializeThread();
        }
        #endregion

        #region Init
        private void InitializeThread()
        {
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
                PlatformUDPThread = new Thread(new ThreadStart(PlatformCommuncationTest));
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
        }

        private void InitLabelArrays()
        {
            DiscreteIO[0] = Discrete_0;
            DiscreteIO[1] = Discrete_1;
            DiscreteIO[2] = Discrete_2;
            DiscreteIO[3] = Discrete_3;
            DiscreteIO[4] = Discrete_4;
            DiscreteIO[5] = Discrete_5;
            DiscreteIO[6] = Discrete_6;
            DiscreteIO[7] = Discrete_7;
        }
        #endregion

        #region InputUDPThread
        /*Function running on InputUDPThread that retrieves 
         *UDP datagrams being sent by UDP meant for the platform */ 
        private void UpdateUDPInputData()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            System.Net.Sockets.UdpClient InputUDPClient = new System.Net.Sockets.UdpClient(5345);

            while (InputUpdate)
            {
                if (InputUDPClient.Available > 0)
                {
                    try
                    {
					    byte[] recvBytes = InputUDPClient.Receive(ref RemoteIpEndPoint);

					    InputDataMutex.WaitOne();
					    CurrentMDACommand = fromBytes(recvBytes);
					    InputDataMutex.ReleaseMutex();

                        //UpdateCommandDataDisplay(CurrentMDACommand);
                    }
                    catch (System.Exception ex)
                    {
                        errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                        break;
                    }
                    
                } //if
            } //while

            InputUDPClient.Close();
        }

        
        #endregion

        #region PlatformThread
        /*Function running on PlatformUDPThread that communicates
         *with MOOG platform by UDP datagrams*/
        private void PlatformCommuncation()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 992);
            System.Net.Sockets.UdpClient PlatformSendUDP = new System.Net.Sockets.UdpClient();
            System.Net.Sockets.UdpClient PlatformRcvUDP = new System.Net.Sockets.UdpClient();

            PlatformRcvUDP.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            PlatformRcvUDP.Client.Bind(RemoteIpEndPoint);
            
            PlatformSendUDP.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            PlatformSendUDP.Client.Bind(RemoteIpEndPoint);

            PlatformRcvUDP.Connect("192.168.0.101", 992);
            //PlatformSendUDP.Connect("192.168.0.101", 991);
            
            bool bSetMode = false;
            SendCommand(MOOG_ENGAGE, PlatformSendUDP, "192.168.0.101", 991);

            byte[] test = PlatformSendUDP.Receive(ref RemoteIpEndPoint);

            while (PlatformUpdate)
            {
                //Wait for platform request
                if (PlatformRcvUDP.Available > 0)
                {
                    try
                    {
                        byte[] recvBytes = PlatformRcvUDP.Receive(ref RemoteIpEndPoint);
                        MOOGResponse platformResp = fromMOOGBytes(recvBytes);

                        string hostname = RemoteIpEndPoint.Address.ToString();
                        int dest_port = RemoteIpEndPoint.Port - 1; //Platform sends and receives on different ports

                        string statusMessage = String.Format("Platform: {0}:{1}", hostname, dest_port);
                        errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), statusMessage);

                        //check for faults
                        if (platformResp.fault > 0)
                        {
                            //What's the fault?
                        } //if

                        //Update Status
                        UpdateFeedbackState(platformResp);
                        MOOGMutex.WaitOne();
                        PlatformState = (byte)(platformResp.status % 16);
                        MOOGMutex.ReleaseMutex();

                        UpdateMutex.WaitOne();
                        byte LocalRequestedState = RequestedState;
                        UpdateMutex.ReleaseMutex();

                        if (LocalRequestedState == MOOG_DISABLE)
                        {
                            //Send Disable Command
                            SendCommand(MOOG_DISABLE, PlatformSendUDP, hostname, dest_port);
                        }

                        switch (PlatformState)
                        {
                            case (byte)MachineStates.ENGAGED:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send updated state
                                    SendState(PlatformSendUDP, hostname, dest_port);
                                }
                                else if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformSendUDP, hostname, dest_port);
                                }

                                //Display feedback data
                                break;
                            case (byte)MachineStates.POWERUP:
                                if (bSetMode == false)
                                {
                                    //Send MDA Mode command
                                    SendCommand(MOOG_MDAMODE, PlatformSendUDP, hostname, dest_port);
                                }

                                break;
                            case (byte)MachineStates.PARKING:
                                //Do nothing and wait
                                break;
                            case (byte)MachineStates.IDLE:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    SendCommand(MOOG_ENGAGE, PlatformSendUDP, hostname, dest_port);
                                }//if
                                break;
                            case (byte)MachineStates.STANDBY:
                                if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformSendUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.FAULT2:
                            case (byte)MachineStates.INHIBITED:
                                //Need to tell user to issue Reset Command
                                if (LocalRequestedState == MOOG_RESET)
                                {
                                    //Send Reset command
                                    SendCommand(MOOG_RESET, PlatformSendUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.DISABLED:
                                //Warn user a hard reset is required
                                break;
                        }

                        //Update displayed state
                        if (PlatformState != PlatformLastState)
                        {
                            StatusText.BeginInvoke(new InvokeDelegateState(UpdatePlatformState), PlatformState);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                        break;
                    }
                } //if
            }//while
        }

        private void PlatformCommuncationOriginal()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            System.Net.Sockets.UdpClient PlatformSendUDP = new System.Net.Sockets.UdpClient();
            System.Net.Sockets.UdpClient PlatformRcvUDP = new System.Net.Sockets.UdpClient(992);
            
            PlatformRcvUDP.Connect("192.168.0.101", 992);
            
            bool bSetMode = false;

            while (PlatformUpdate)
            {
                //Wait for platform request
                if (PlatformRcvUDP.Available > 0)
                {
                    try
                    {
                        byte[] recvBytes = PlatformRcvUDP.Receive(ref RemoteIpEndPoint);
                        MOOGResponse platformResp = fromMOOGBytes(recvBytes);

                        string hostname = RemoteIpEndPoint.Address.ToString();
                        int dest_port = RemoteIpEndPoint.Port - 1; //Platform sends and receives on different ports

                        string statusMessage = String.Format("Platform: {0}:{1}", hostname, dest_port);
                        errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), statusMessage);

                        //check for faults
                        if (platformResp.fault > 0)
                        {
                            //What's the fault?
                        } //if

                        //Update Status
                        UpdateFeedbackState(platformResp);
                        MOOGMutex.WaitOne();
                        PlatformState = (byte)(platformResp.status % 16);
                        MOOGMutex.ReleaseMutex();

                        UpdateMutex.WaitOne();
                        byte LocalRequestedState = RequestedState;
                        UpdateMutex.ReleaseMutex();

                        if (LocalRequestedState == MOOG_DISABLE)
                        {
                            //Send Disable Command
                            SendCommand(MOOG_DISABLE, PlatformSendUDP, hostname, dest_port);
                        }

                        switch (PlatformState)
                        {
                            case (byte)MachineStates.ENGAGED:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send updated state
                                    SendState(PlatformSendUDP, hostname, dest_port);
                                }
                                else if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformSendUDP, hostname, dest_port);
                                }

                                //Display feedback data
                                break;
                            case (byte)MachineStates.POWERUP:
                                if (bSetMode == false)
                                {
                                    //Send MDA Mode command
                                    SendCommand(MOOG_MDAMODE, PlatformSendUDP, hostname, dest_port);
                                }

                                break;
                            case (byte)MachineStates.PARKING:
                                //Do nothing and wait
                                break;
                            case (byte)MachineStates.IDLE:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    SendCommand(MOOG_ENGAGE, PlatformSendUDP, hostname, dest_port);
                                }//if
                                break;
                            case (byte)MachineStates.STANDBY:
                                if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformSendUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.FAULT2:
                            case (byte)MachineStates.INHIBITED:
                                //Need to tell user to issue Reset Command
                                if (LocalRequestedState == MOOG_RESET)
                                {
                                    //Send Reset command
                                    SendCommand(MOOG_RESET, PlatformSendUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.DISABLED:
                                //Warn user a hard reset is required
                                break;
                        }

                        //Update displayed state
                        if (PlatformState != PlatformLastState)
                        {
                            StatusText.BeginInvoke(new InvokeDelegateState(UpdatePlatformState), PlatformState);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                        break;
                    }
                } //if
            }//while
        }

        private void PlatformCommuncationTest()
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 991);
            //System.Net.Sockets.UdpClient PlatformSendUDP = new System.Net.Sockets.UdpClient(991);
            System.Net.Sockets.UdpClient PlatformRcvUDP = new System.Net.Sockets.UdpClient(992);

            MDACommand platformCommand = new MDACommand();
            platformCommand.MCW = (uint) MOOG_ENGAGE;
            byte[] sendBytes = flipStrBytes(platformCommand);

            string hostname = RemoteIpEndPoint.Address.ToString();
            int dest_port = RemoteIpEndPoint.Port; //Platform sends and receives on different ports

            PlatformRcvUDP.Send(sendBytes, sendBytes.Length, hostname,dest_port);

            //bool bSetMode = false;

            while (PlatformUpdate)
            {
                //Wait for platform request
                if (PlatformRcvUDP.Available > 0)
                {
                    try
                    {
                        byte[] recvBytes = PlatformRcvUDP.Receive(ref RemoteIpEndPoint);
                        MOOGResponse platformResp = fromMOOGBytes(recvBytes);

//                         string statusMessage = String.Format("Platform: {0}:{1}", hostname, dest_port);
//                         errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), statusMessage);

                        //check for faults
                        if (platformResp.fault > 0)
                        {
                            //What's the fault?
                        } //if

                        //Update Status
                        //UpdateFeedbackState(platformResp);
                        MOOGMutex.WaitOne();
                        CurrentMOOGResponse = platformResp;
                        MOOGMutex.ReleaseMutex();

                        PlatformFault = platformResp.fault;
                        PlatformIO = platformResp.io_info;
                        PlatformState = (byte)(platformResp.status % 16);

                        UpdateMutex.WaitOne();
                        byte LocalRequestedState = RequestedState;
                        UpdateMutex.ReleaseMutex();

                        if (LocalRequestedState == MOOG_DISABLE)
                        {
                            //Send Disable Command
                            SendCommand(MOOG_DISABLE, PlatformRcvUDP, hostname, dest_port);
                        }

                        switch (PlatformState)
                        {
                            case (byte)MachineStates.ENGAGED:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send updated state
                                    SendState(PlatformRcvUDP, hostname, dest_port);
                                }
                                else if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformRcvUDP, hostname, dest_port);
                                }

                                //Display feedback data
                                break;
                            case (byte)MachineStates.POWERUP:
                                //Send MDA Mode command
                                    SendCommand(MOOG_MDAMODE, PlatformRcvUDP, hostname, dest_port);
                                break;
                            case (byte)MachineStates.PARKING:
                                if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformRcvUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.IDLE:
                                //Send updated state
                                if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    SendCommand(MOOG_ENGAGE, PlatformRcvUDP, hostname, dest_port);
                                }//if
                                else //Keep connection alive
                                {
                                    SendCommand(MOOG_MDAMODE, PlatformRcvUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.STANDBY:
                                if (LocalRequestedState == MOOG_PARK)
                                {
                                    //Send Park command
                                    SendCommand(MOOG_PARK, PlatformRcvUDP, hostname, dest_port);
                                }
                                else if (LocalRequestedState == MOOG_ENGAGE)
                                {
                                    //Send Engage Command
                                    SendState(PlatformRcvUDP, hostname, dest_port);
                                }//if
                                break;
                            case (byte)MachineStates.FAULT2:
                            case (byte)MachineStates.INHIBITED:
                                //Need to tell user to issue Reset Command
                                if (LocalRequestedState == MOOG_RESET)
                                {
                                    //Send Reset command
                                    SendCommand(MOOG_RESET, PlatformRcvUDP, hostname, dest_port);
                                }
                                break;
                            case (byte)MachineStates.DISABLED:
                                //Warn user a hard reset is required
                                break;
                        }

                        //Update displayed state
                        if (PlatformState != PlatformLastState)
                        {
                            StatusText.BeginInvoke(new InvokeDelegateState(UpdatePlatformState), PlatformState);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
                        break;
                    }
                } //if
                else
                {   //Attempt to re-engage communication
                    SendCommand(MOOG_MDAMODE, PlatformRcvUDP, hostname, dest_port);
                }
            }//while
        }
        #endregion

        #region UpdateDisplayThread
        private void UpdateDisplays()
        {
            while (PlatformUpdate)
            {
                MOOGMutex.WaitOne();
                MOOGResponse local_resp = CurrentMOOGResponse;
                MOOGMutex.ReleaseMutex();

                UpdateFaults(local_resp.fault);
                UpdateDiscreteIO(local_resp.io_info);
                UpdateFeedbackState(local_resp);

                InputDataMutex.WaitOne();
                MDACommand LocalMDACommand = CurrentMDACommand;
                InputDataMutex.ReleaseMutex();

                UpdateCommandDataDisplay(LocalMDACommand);

                Thread.Sleep(500);
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

        }
        #endregion

        #region SendToMoog
        //Sends most recent MDA command
        private void SendState(System.Net.Sockets.UdpClient PlatformSendUDP, string hostname, int port)
        {
            InputDataMutex.WaitOne();
            MDACommand LocalCurrentMDA = CurrentMDACommand;
            InputDataMutex.ReleaseMutex();

            byte[] sendBytes = flipStrBytes(LocalCurrentMDA);

            try
            {
                PlatformSendUDP.Send(sendBytes, sendBytes.Length, hostname, port);
            }
            catch (System.Exception ex)
            {
            	//Cause failure of some sorts.
                errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
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
                errorBar.BeginInvoke(new InvokeDelegateString(ErrorBarMessage), ex.Message);
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
                    //Display feedback data
                    break;
                case (byte)MachineStates.POWERUP:
                    //Do nothing and wait
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    break;
                case (byte)MachineStates.PARKING:
                    //Do nothing and wait
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    break;
                case (byte)MachineStates.IDLE:
                    //Send updated state
                    EngageButton.Enabled = true;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
                    break;
                case (byte)MachineStates.STANDBY:
                    //Send updated state
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = true;
                    break;
                case (byte)MachineStates.FAULT2:
                case (byte)MachineStates.INHIBITED:
                    //Need to tell user to issue Reset Command
                    ResetButton.Enabled = true;
                    EngageButton.Enabled = false;
                    ParkButton.Enabled = false;
                    break;
                case (byte)MachineStates.DISABLED:
                    //Warn user a hard reset is required
                    EngageButton.Enabled = false;
                    ResetButton.Enabled = false;
                    ParkButton.Enabled = false;
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

        private void DisableButton_Click(object sender, EventArgs e)
        {
            UpdateMutex.WaitOne();
            RequestedState = MOOG_DISABLE;
            UpdateMutex.ReleaseMutex();

            RStatusState.Text = "Moving to Disable";
        }
        #endregion

    }
}
