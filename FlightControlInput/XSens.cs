using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using CMT;

namespace FlightControlInput
{
    
    #region MTi
    public class XSens : CMT.MotionTrackerClass
    {
        public enum SerialStatus
        {
            CONNECTION_FAILURE = -1,
            PING_RESPONSE_FAILURE = -2,
            PACKET_SIZE_MISSMATCH = -3,
            BAUD_RATE_CHANGE_FAIL = -4,
            NO_DEVICES_FOUND = -5,
            SUCCESS = 1
        }

        private enum CmtQueueMode
        {
            CMT_QM_FIFO = 0,
            CMT_QM_LAST = 1,
            CMT_QM_RAW = 2
        };

        #region DataPacket
        public struct ImuDataPacket
        {
            public string time;
            public double roll;
            public double pitch;
            public double yaw;
            public double v_roll;
            public double v_pitch;
            public double v_yaw;
            public double a_x;
            public double a_y;
            public double a_z;
            public double mag_x;
            public double mag_y;
            public double mag_z;
            public double temp_sensor;
            public UInt16 ahrs_time;

            public string getline()
            {
                string outputLine = roll.ToString() + ", " + pitch.ToString() + ", " + yaw.ToString()
                    + ", " + v_roll.ToString() + ", " + v_pitch.ToString() + ", " + v_yaw.ToString() + ", " +
                    a_x.ToString() + ", " + a_y.ToString() + ", " + a_z.ToString() + ", " + mag_x.ToString()
                    + ", " + mag_y.ToString() + ", " + mag_z.ToString() + ", " + temp_sensor.ToString()
                    + ", " + ahrs_time.ToString();
                return outputLine;
            }

            public string getlineRads()
            {
                
                const double Deg2Rad = Math.PI / 180.0;
                string outputLine = (roll * Deg2Rad).ToString() + ", " + (pitch * Deg2Rad).ToString() + ", " + (yaw * Deg2Rad).ToString()
                    + ", " + (v_roll * Deg2Rad).ToString() + ", " + (v_pitch * Deg2Rad).ToString() + ", " + (v_yaw * Deg2Rad).ToString() + ", " +
                    (a_x).ToString() + ", " + (a_y).ToString() + ", " + (a_z).ToString() + ", " + mag_x.ToString()
                    + ", " + mag_y.ToString() + ", " + mag_z.ToString() + ", " + temp_sensor.ToString()
                    + ", " + ahrs_time.ToString();
                return outputLine;
            }

            public string getDebug()
            {
                string outputLine = String.Format("{0:0.000}", roll) + ", " + String.Format("{0:0.000}", pitch) + ", " + String.Format("{0:0.000}", yaw);
                return outputLine;
            }

            public string getFormat()
            {
                string AHRSFormat = "Roll (deg) ,  Pitch (deg) ,  Yaw (deg) ,  RollRate (deg/s),  PitchRate (deg/s) ,  YawRate (deg/s) ,  XAccel (m/s/s) ,  YAccel (m/s/s) ,  ZAccel (m/s/s) ,  XMag (gauss),  YMag (gauss) ,  ZMag (gauss) ,  InTemp (C) ,  AHRSTicks";
                return AHRSFormat;
            }

            public string getFormatRad()
            {
                string AHRSFormat = "RollXsens_rad ,  PitchXsens_rad ,  YawXsens_rad ,  RollRateXsens_rads ,  PitchRateXsens_rads ,  YawRateXsens_rads ,  XAccelXsens_mss ,  YAccelXsens_mss ,  ZAccelXsens_mss ,  XMagXsens_gauss,  YMagXsens_gauss ,  ZMagXsens_gauss ,  InTempXsens_C ,  AHRSTicks";
                return AHRSFormat;
            }
        }
        #endregion

        #region CmtStructures
        //struct CmtEuler
        //{
        //    public double m_roll;		//!< The roll (rotation around x-axis / back-front-line)
        //    public double m_pitch;		//!< The pitch (rotation around y-axis / right-left-line)
        //    public double m_yaw;		//!< The yaw (rotation around z-axis / down-up-line)
        //};

        //struct CmtVector
        //{
        //    public double X;
        //    public double Y;
        //    public double Z;
        //};

        //struct CmtCalData
        //{
        //    public CmtVector m_acc;
        //    public CmtVector m_gyr;
        //    public CmtVector m_mag;
        //};
        #endregion

        private string _deviceIDString = "485DA-A9EE4-35BC6-85B6B";
        private int _deviceID = 0;
        private string _errorMessage = "";
        private short _comPrt = 1;
        private short _frequency = 0;
        private bool _isConnected = false;
        private bool _isMonitoring = false;
        private bool _isReceiving = false;
        private Mutex _xSensMutex;
        private Mutex _localMonitorMutex = new Mutex();
        private ImuDataPacket _data = new ImuDataPacket();

        #region Constructor
        public XSens()
        {
            cmtCreateInstance(_deviceIDString);
            _xSensMutex = new Mutex();
        }

        public XSens(Mutex xSensMutex)
        {
            cmtCreateInstance(_deviceIDString);
            _xSensMutex = xSensMutex;
        }

        ~XSens()
        {
            if (_isMonitoring)
            {
                KillThread();
            }
        }

        //Dispose of IDisposable types
        public void Dispose()
        {
            if (_isMonitoring)
            {
                KillThread();
            }

            if (_xSensMutex != null)
            {
                _xSensMutex.Close();
                _xSensMutex = null;
            }
        }
        #endregion

        #region End
        public void KillThread()
        {
            _isMonitoring = false;
            
            Thread.Sleep(10);
        }

        public void DisconnectSerial()
        {
            if (_isMonitoring)
            {
                KillThread();
            }

            cmtClosePort((int)_comPrt);
            _isConnected = false;
        }
        
        #endregion

        #region Connection
        public SerialStatus ConnectSerial(string comPrt, int baudrate)
        {
            string tmp = string.Empty;
            short res;

            #region OpenPort
            for (int i = 0; i < comPrt.Length && i < 5; i++)
            {
                if (Char.IsDigit(comPrt[i]))
                    tmp += comPrt[i];
            }

            if (tmp.Length > 0)
                try
                {
                    baudrate = 0;
                    _comPrt = Int16.Parse(tmp);
                    cmtOpenPort(_comPrt, baudrate);
                    cmtGetDeviceId(out _deviceID);
                }
                catch (SystemException ex)
                {
                    _errorMessage = "Could not open COM Port:" + ex.Message;
                    _isConnected = false;
                    return SerialStatus.CONNECTION_FAILURE;
                }
            #endregion

            #region FindDevice(s)
            //Count Sensors to ensure connected
            cmtGetMtCount(out res);

            if (res == 0)
            {
                _errorMessage = "No Devices connected.";
                _isConnected = false;
                return SerialStatus.NO_DEVICES_FOUND;
            }
            #endregion

            #region Configure
            //Set Queue Mode so that we always get latest data
            cmtSetQueueMode((int)CmtQueueMode.CMT_QM_LAST);

            //Set sensor to config state
            cmtGotoConfig();

            //Get frequency
            cmtGetSampleFrequency(out _frequency);

            //Set Device to send calibrated data and euler angles
            int mode = 0;
            int settings = 0;
            cmtGetDeviceMode(out mode, out settings, out _frequency);

            Thread.Sleep(5);

            //mode = 0x4000;// CMT_OUTPUTMODE_RAW;
            mode = 0x0002 | 0x0004; // CMT_OUTPUTMODE_CALIB | CMT_OUTPUTMODE_ORIENT;
            int OrientMode_Euler = 4;
            int Coordinates_NED = 1 << 31;//Convert.ToInt32("80000000",16);
            settings = OrientMode_Euler | Coordinates_NED;// CMT_OUTPUTSETTINGS_ORIENTMODE_EULER | OUTPUTSETTINGS_COORDINATES_NED;
            
            cmtSetDeviceMode(mode, settings, _frequency);

            #endregion

            #region Filtering
            //Software filtering
            short scenarioType = 6; //Machine

            try
            {
                cmtSetScenario(scenarioType);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                _errorMessage = "Failed to setup filtering, " + ex.ToString();
                return SerialStatus.CONNECTION_FAILURE;
            }
            #endregion

            _isConnected = true;
            return SerialStatus.SUCCESS;
        }
        #endregion

        #region Thread
        //Thread to continously read data
        public void MonitorSerial()
        {
            const short NoRotationDuration = 120; // seconds
            Stopwatch timeoutTimer = new Stopwatch();
            Stopwatch noRotationTimer = new Stopwatch();
            bool holdRotation = false;
            bool monitor = true;
            long maxTimeout = 5000; //ms or 5 seconds

            //Start receiving data
            cmtGotoMeasurement();
            Thread.Sleep(20);

            //Measure Drift for stationary postion
            try
            {
                cmtSetNoRotation(NoRotationDuration);
                holdRotation = true;
                noRotationTimer.Start();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                _errorMessage = "Failed to set no rotation: " + ex.Message;
                _isConnected = true;
            }

            _localMonitorMutex.WaitOne();
            _isMonitoring = monitor;
            _localMonitorMutex.ReleaseMutex();
            timeoutTimer.Start();

            //Run while loop to collect data
            while (monitor)
            {
                if (holdRotation)
                {
                    if (noRotationTimer.ElapsedMilliseconds >= NoRotationDuration * 1000)
                    {
                        holdRotation = false;
                        noRotationTimer.Stop();
                    }

                    _data.a_x = 1.0f * noRotationTimer.ElapsedMilliseconds / 1000.0f - NoRotationDuration;

                    //Reset Orientation
                    try
                    {
                        int ResetOrientationValue = 3;
                        cmtResetOrientation(ResetOrientationValue, _deviceID);
                    }
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        _errorMessage = "Failed to reorient: " + ex.Message;
                    }
                    
                    timeoutTimer.Reset();
                    timeoutTimer.Start();
                }
                else
                {
                    try
                    {
                        getData();
                        timeoutTimer.Reset();
                        timeoutTimer.Start();
                    }
                    catch (SystemException ex)
                    {
                        _isReceiving = false;
                        _errorMessage = "Failed to receive buffer: " + ex.Message;
                        break;
                    }

                    //Check for timeout
                    if (timeoutTimer.ElapsedMilliseconds > maxTimeout)
                    {
                        //ATTEMPT RECONNECT????
                        _isReceiving = false;
                        _errorMessage = "Port has timed out.";
                        break;
                    }
                }

                _localMonitorMutex.WaitOne();
                monitor = _isMonitoring;
                _localMonitorMutex.ReleaseMutex();
            }

            try
            {
                //Stop receiving data
                cmtGotoConfig();
            }
            catch (SystemException ex)
            {
                _errorMessage = ex.Message;
            }
        }

        #endregion

        #region ExternalDataAccess
        public ImuDataPacket GetData()
        {
            return _data;
        }
        
        public string GetCurrentData()
        {
            _xSensMutex.WaitOne();
            string line = _data.getline();
            _xSensMutex.ReleaseMutex();
            return line;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }

        public string GetCurrentDataRads()
        {
            _xSensMutex.WaitOne();
            string line = _data.getlineRads();
            //string line = m_data.getDebug();
            _xSensMutex.ReleaseMutex();
            return line;
        }

        public string GetDebugData()
        {
            return _data.getDebug();
        }

        public string GetLineFormat()
        {
            return _data.getFormat();
        }

        public string GetLineFormatRads()
        {
            return _data.getFormatRad();
        }

        public bool IsOpen()
        {
            return _isConnected;
        }

        public bool ReceivingData()
        {
            return _isReceiving;
        }
        #endregion

        #region GetData
        private void getData()
        {
            double[] calData;
            double[] euler_angles;
            Object EA;

            try
            {
                cmtGetNextDataBundle();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                _errorMessage = ex.Message;
                return;
            }
            Thread.Sleep(10);

            cmtDataGetOriEuler(out EA);

            euler_angles = (double[])EA;

            _data.roll = euler_angles[0];
            _data.pitch = euler_angles[1];
            _data.yaw = euler_angles[2];

            cmtDataGetCalData(out EA);
            calData = (double[])EA;

            _data.a_x = -calData[0];
            _data.a_y = -calData[1];
            _data.a_z = -calData[2];

            _data.v_roll = calData[3];
            _data.v_pitch = calData[4];
            _data.v_yaw = calData[5];

            _data.mag_x = calData[6];
            _data.mag_y = calData[7];
            _data.mag_z = calData[8];
        }
        #endregion
    }
    #endregion
}
