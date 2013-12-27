using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.IO.Ports;

namespace FlightControlInput
{
    enum SerialStatus
    {
        CONNECTION_FAILURE,
        PING_RESPONSE_FAILURE,
        PACKET_SIZE_MISSMATCH,
        SUCCESS
    }

    class ArduinoCommunication
    {
        #region Variables
        private const byte HEADER = 0xAA;
        private SerialPort h_ArduinoSerialPort = null;
        private Mutex LocalMonitorMutex;
        private Mutex LocalDataMutex;
        private bool isMonitoring;

        private List<byte> m_buffer;
        private Stopwatch m_freqTimer;
        private string m_msg;
        private bool m_receiving = false;

        private const int SENSOR_TIMEOUT_IN_MSECS = 5000;
        private const int MIN_FREQUENCY = 40; //Hz

        #endregion

        #region Constructor
        //constructor
        public ArduinoCommunication()
        {
            //Mutex needed so many class doesn't access data as it is being written to
            LocalMonitorMutex = new Mutex();
            LocalDataMutex = new Mutex();

            m_buffer = new List<byte>();
            m_freqTimer = new Stopwatch();
            m_msg = "No errors";
        }

        //Destructor
        ~ArduinoCommunication()
        {
            if (isMonitoring)
            {
                KillThread();
            }
        }
        #endregion

        #region End
        // Tells AHRS thread to end
        public void KillThread()
        {
            LocalMonitorMutex.WaitOne();
            isMonitoring = false;
            LocalMonitorMutex.ReleaseMutex();

            Thread.Sleep(10);
        }

        public void DisconnectSerial()
        {
            if (isMonitoring)
            {
                KillThread();
            }

            //Close connection correctly
            if (h_ArduinoSerialPort.IsOpen)
            {
                //Clear buffer
                h_ArduinoSerialPort.DiscardInBuffer();
                h_ArduinoSerialPort.Close();
            }
        }
        #endregion //End

        #region Connection
        //Connect
        public SerialStatus ConnectSerial(string portName, int baudrate)
        {
            h_ArduinoSerialPort = new SerialPort(portName, baudrate, Parity.None, 8, StopBits.One);
            h_ArduinoSerialPort.Handshake = Handshake.None;
            h_ArduinoSerialPort.ReadTimeout = SENSOR_TIMEOUT_IN_MSECS;
            h_ArduinoSerialPort.WriteTimeout = SENSOR_TIMEOUT_IN_MSECS;

            try
            {
                h_ArduinoSerialPort.Open();
            }
            catch (System.Exception ex)
            {
                m_msg = ex.Message;
                return SerialStatus.CONNECTION_FAILURE;
            }


            //Check connection is open
            if (!h_ArduinoSerialPort.IsOpen)
            {
                m_msg = "ArduIMU connection failed to open.";
                return SerialStatus.CONNECTION_FAILURE;
            }

            Thread.Sleep(100);
            h_ArduinoSerialPort.DiscardOutBuffer();

            // Changes to polled mode
            h_ArduinoSerialPort.Write(new char[1] { 'P' }, 0, 1);

            Thread.Sleep(100);

            if (!pingSerial(5000))
            {
                h_ArduinoSerialPort.Close();
                return SerialStatus.PING_RESPONSE_FAILURE;
            }
            
            m_freqTimer.Start();

            return SerialStatus.SUCCESS;
        }

        // Ping AHRS 
        private bool pingSerial(long timeoutInMilliSeconds)
        {
            if (!h_ArduinoSerialPort.IsOpen) return false;

            char[] data = new char[100];
            int bytesRead = 0;
            char[] pingChar = new char[1] { 'R' };
            Stopwatch stopWatch = new Stopwatch();

            h_ArduinoSerialPort.DiscardInBuffer();
            h_ArduinoSerialPort.Write(pingChar, 0, 1);

            stopWatch.Start();

            while (bytesRead < 1)
            {
                try
                {
                    if (h_ArduinoSerialPort.BytesToRead > 0)
                    {
                        bytesRead = h_ArduinoSerialPort.Read(data, 0, 1);
                    }
                }
                catch (System.TimeoutException ex)
                {
                    m_msg = ex.Message;
                    return false;
                }

                if (bytesRead > 0)  //Received response, then good
                {
                    m_msg = "AHRS ping response received.";
                    return true;
                }

                //Has timeout expired?
                if (stopWatch.ElapsedMilliseconds > timeoutInMilliSeconds)
                {
                    m_msg = "AHRS ping has timed out.";
                    return false;
                }
            }

            return false;
        }


        #endregion //Connection

        #region Strings
        public string GetErrorMessage()
        {
            return m_msg;
        }

        public bool ReceivingData()
        {
            return m_receiving;
        }

        public bool IsOpen()
        {
            bool status = (h_ArduinoSerialPort != null && h_ArduinoSerialPort.IsOpen);

            return status;
        }

        #endregion

        #region Thread
        public void SendBytes(byte[] data)
        {
            float rollRatio = BitConverter.ToSingle(data, 0);
            float pitchRatio = BitConverter.ToSingle(data, 4);
            float yawRatio = BitConverter.ToSingle(data, 8);

            byte rollAnalog = (byte)(255 * ((rollRatio + 1) / 2 ));
            byte pitchAnalog = (byte)(255 * ((pitchRatio + 1) / 2));
            byte yawAnalog = (byte)(255 * ((yawRatio + 1) / 2));

            byte checksum = (byte)((rollAnalog + pitchAnalog + yawAnalog) % 256);

            byte[] transmitArray = new byte[] { HEADER, rollAnalog, pitchAnalog, yawAnalog, checksum };

            h_ArduinoSerialPort.Write(transmitArray, 0, transmitArray.Length);
        }
        #endregion

        #region ProcessPacket
        
        #endregion

        
    }
}
