using System;
//using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace MotionPlatformControl
{
    enum SerialStatus
    {
        CONNECTION_FAILURE = -1,
        PING_RESPONSE_FAILURE = -2,
        PACKET_SIZE_MISSMATCH = -3,
        BAUD_RATE_CHANGE_FAIL = -4,
        SUCCESS = 1
    }

    #region DataPacket
    struct AHRSDataPacket 
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
            const double GRAVITY = 9.80;
            const double Deg2Rad = Math.PI / 180.0;
            string outputLine = (roll*Deg2Rad).ToString() + ", " + (pitch*Deg2Rad).ToString() + ", " + (yaw*Deg2Rad).ToString()
                + ", " + (v_roll*Deg2Rad).ToString() + ", " + (v_pitch*Deg2Rad).ToString() + ", " + (v_yaw*Deg2Rad).ToString() + ", " +
                (a_x * GRAVITY).ToString() + ", " + (a_y * GRAVITY).ToString() + ", " + (a_z * GRAVITY).ToString() + ", " + mag_x.ToString()
                + ", " + mag_y.ToString() + ", " + mag_z.ToString() + ", " + temp_sensor.ToString()
                + ", " + ahrs_time.ToString();
            return outputLine;
        }

        public string getDebug()
        {
            string outputLine = time + ", " + String.Format("{0:0.000}", roll) + ", " + String.Format("{0:0.000}", pitch) + ", " + String.Format("{0:0.000}", yaw);
            return outputLine;
        }

        public string getFormat()
        {
            string AHRSFormat = "Roll (deg) ,  Pitch (deg) ,  Yaw (deg) ,  RollRate (deg/s),  PitchRate (deg/s) ,  YawRate (deg/s) ,  XAccel (Gs) ,  YAccel (Gs) ,  ZAccel (Gs) ,  XMag (gauss),  YMag (gauss) ,  ZMag (gauss) ,  InTemp (C) ,  AHRSTicks";
            return AHRSFormat;
        }

        public string getFormatRad()
        {
            string AHRSFormat = "Roll (rad) ,  Pitch (rad) ,  Yaw (rad) ,  RollRate (rad/s) ,  PitchRate (rad/s) ,  YawRate (rad/s) ,  XAccel (m/s/s) ,  YAccel (m/s/s) ,  ZAccel (m/s/s) ,  XMag (gauss),  YMag (gauss) ,  ZMag (gauss) ,  InTemp (C) ,  AHRSTicks";
            return AHRSFormat;
        }
    } 
    #endregion

    class AhrsSerialData : IDisposable
    {
        #region Variables
        private SerialPort h_AhrsSerialPort = null;
        private Mutex AHRSMutex;
        private Mutex LocalMonitorMutex;
        private Mutex LocalDataMutex;
        private bool isMonitoring;

        private List<byte> m_buffer;
        private int m_dataCount = 0;
        private int m_packetLength = 0;
        private Stopwatch m_freqTimer;
        private bool m_lowFrequency = false;
        private AHRSDataPacket m_data;
        private string m_msg;
        private bool m_receiving = false;

        private const int SENSOR_TIMEOUT_IN_MSECS = 5000;
        private const int MIN_FREQUENCY = 40; //Hz

        enum AHRSconstants
        {
            VG_MODE_PACKET_SIZE = 30,
            SCALED_MODE_PACKET_SIZE = 24,
            VOLTAGE_MODE_PACKET_SIZE = 24,
            PACKET_HEADER = 0xFF
        }
        #endregion

        #region Constructor
        //constructor
        public AhrsSerialData(Mutex _globalMutex)
        {
            //Mutex needed so many class doesn't access data as it is being written to
            AHRSMutex = _globalMutex;
            LocalMonitorMutex = new Mutex();
            LocalDataMutex = new Mutex();

            m_buffer = new List<byte>();
            m_freqTimer = new Stopwatch();
            m_data = new AHRSDataPacket();
            m_msg = "No errors";
        }

        //Destructor
        ~AhrsSerialData()
        {
            if (isMonitoring)
            {
                KillThread();
            }
        }

        //Dispose of IDisposable types
        public void Dispose()
        {
            if (isMonitoring)
            {
                KillThread();
            }

            if (h_AhrsSerialPort != null)
            {
                h_AhrsSerialPort.Dispose();
                h_AhrsSerialPort = null;
            }

            if (AHRSMutex != null)
            {
                AHRSMutex.Close();
                AHRSMutex = null;
            }

            if (LocalDataMutex != null)
            {
                LocalDataMutex.Close();
                LocalDataMutex = null;
            }

            if (LocalMonitorMutex != null)
            {
                LocalMonitorMutex.Close();
                LocalMonitorMutex = null;
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
            if (h_AhrsSerialPort.IsOpen)
            {
                //Clear buffer
                h_AhrsSerialPort.DiscardInBuffer();
                h_AhrsSerialPort.Close();
            }
        }
        #endregion //End

        #region Connection
        //Connect
        public SerialStatus ConnectSerial(string portName, int baudrate)
        {
            SerialStatus stat = SerialStatus.SUCCESS;
            h_AhrsSerialPort = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
            h_AhrsSerialPort.Handshake = Handshake.None;
            h_AhrsSerialPort.ReadTimeout = SENSOR_TIMEOUT_IN_MSECS;
            h_AhrsSerialPort.WriteTimeout = SENSOR_TIMEOUT_IN_MSECS;

            try
            {
                h_AhrsSerialPort.Open();
            }
            catch (System.Exception ex)
            {
                m_msg = ex.Message;
                return SerialStatus.CONNECTION_FAILURE;
            }


            //Check connection is open
            if (!h_AhrsSerialPort.IsOpen)
            {
                m_msg = "AHRS connection failed to open.";
                return SerialStatus.CONNECTION_FAILURE;
            }

            Thread.Sleep(100);
            h_AhrsSerialPort.DiscardOutBuffer();

            // Changes to polled mode
            h_AhrsSerialPort.Write(new char[1] { 'P' }, 0, 1);

            Thread.Sleep(100);

            if (!pingSerial(5000))
            {
                h_AhrsSerialPort.Close();
                return SerialStatus.PING_RESPONSE_FAILURE;
            }
            else
            {
                m_packetLength = determinePacketLength();
                if (m_packetLength < 30)
                {
                    m_msg = "AHRS packet size too small.";
                    m_receiving = false;
                    return SerialStatus.PACKET_SIZE_MISSMATCH;
                }
                m_msg = "AHRS packet size found.";
            }

            //Check Baud Rate
            if (baudrate != 38400)
            {
                //Request baud rate change
                if (getChar(1000,'b','B'))
                {
                    if (!changeBaud(baudrate))
                    {
                        stat = SerialStatus.BAUD_RATE_CHANGE_FAIL;
                    }
                }
                else
                {
                    stat = SerialStatus.BAUD_RATE_CHANGE_FAIL;
                }

            }


            //Change to Angle (VG) Mode
            h_AhrsSerialPort.Write(new char[1] { 'a' }, 0, 1);
            m_freqTimer.Start();

            //Change to Normal Mode
            h_AhrsSerialPort.Write(new char[2] { 'T', 'C' }, 0, 2);

            return stat;
        }

        // Change baudrate
        private bool changeBaud(int newBaudrate)
        {
            if (!h_AhrsSerialPort.IsOpen)
            {
                return false;
            }

            h_AhrsSerialPort.BaudRate = newBaudrate;

            return getChar(1000,'a','A');
        }

        // Wait for single char response
        private bool getChar(long timeoutInMilliSeconds, char outChar, char reqChar)
        {
            if (!h_AhrsSerialPort.IsOpen) return false;

            char[] data = new char[100];
            int bytesRead = 0;
            Stopwatch stopWatch = new Stopwatch();

            h_AhrsSerialPort.DiscardInBuffer();
            h_AhrsSerialPort.Write(new char[1] {outChar}, 0, 1);

            stopWatch.Start();

            while (bytesRead < 1)
            {
                try
                {
                    if (h_AhrsSerialPort.BytesToRead > 0)
                    {
                        bytesRead = h_AhrsSerialPort.Read(data, 0, 1);
                    }
                }
                catch (System.TimeoutException ex)
                {
                    m_msg = ex.Message;
                    return false;
                }

                if (bytesRead > 0)  //Received response, then good
                {
                    if (data[0] == reqChar)
                    {
                        return true;
                    }
                    m_msg = "Baudrate change failed.";
                    return false;
                }

                //Has timeout expired?
                if (stopWatch.ElapsedMilliseconds > timeoutInMilliSeconds)
                {
                    m_msg = "Baudrate change has timed out.";
                    return false;
                }
            }

            return false;
        }

        // Ping AHRS 
        private bool pingSerial(long timeoutInMilliSeconds)
        {
            if (!h_AhrsSerialPort.IsOpen) return false;

            char[] data = new char[100];
            int bytesRead = 0;
            char[] pingChar = new char[1] { 'R' };
            Stopwatch stopWatch = new Stopwatch();

            h_AhrsSerialPort.DiscardInBuffer();
            h_AhrsSerialPort.Write(pingChar, 0, 1);

            stopWatch.Start();

            while (bytesRead < 1)
            {
                try
                {
                    if (h_AhrsSerialPort.BytesToRead > 0)
                    {
                        bytesRead = h_AhrsSerialPort.Read(data, 0, 1);
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
        public AHRSDataPacket GetData()
        {
            return m_data;
        }

        public string GetCurrentData()
        {
            AHRSMutex.WaitOne();
            string line = m_data.getline();
            //string line = m_data.getDebug();
            AHRSMutex.ReleaseMutex();
            return line;
        }
        public string GetCurrentDataRads()
        {
            AHRSMutex.WaitOne();
            string line = m_data.getlineRads();
            //string line = m_data.getDebug();
            AHRSMutex.ReleaseMutex();
            return line;
        }

        public string GetLineFormat()
        {
            return m_data.getFormat();
        }

        public string GetLineFormatRads()
        {
            return m_data.getFormatRad();
        }

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
            bool status = (h_AhrsSerialPort != null && h_AhrsSerialPort.IsOpen);

            return status;
        }

        #endregion

        #region Thread
        //Thread to continuously read data
        public void MonitorSerial()
        {
            if (!h_AhrsSerialPort.IsOpen)
            {
                return;
            }

            Stopwatch timeoutTimer = new Stopwatch();
            long maxTimeout = 5000; //ms
            byte[] data = new byte[100];
            int bytesRead = 0;
            bool monitor = true;

            m_dataCount = 0;

            LocalMonitorMutex.WaitOne();
            isMonitoring = monitor;
            LocalMonitorMutex.ReleaseMutex();

            //Switch to continuous mode
            h_AhrsSerialPort.Write(new char[1] { 'C' }, 0, 1);
            timeoutTimer.Start();
            m_receiving = true;

            while (monitor)
            {
                //Check serial port open
                if (!h_AhrsSerialPort.IsOpen)
                {
                    m_receiving = false;
                    m_msg = "Port has closed.";
                    break;
                }

                //Check for timeout
                if (timeoutTimer.ElapsedMilliseconds > maxTimeout)
                {
                    //ATTEMPT RECONNECT????
                    m_receiving = false;
                    m_msg = "Port has timed out.";
                    break;
                }

                //Something about frequency...
                if (m_lowFrequency)
                {
                    //Create notification
                    bytesRead = -1;
                }

                //Read data
                bytesRead = h_AhrsSerialPort.Read(data, 0, m_packetLength);

                if (bytesRead > 0)
                {
                    timeoutTimer.Reset();
                    timeoutTimer.Start();
                    //PROCESS DATA
                    processPacket(data, bytesRead, m_packetLength);
                }


                LocalMonitorMutex.WaitOne();
                monitor = isMonitoring;
                LocalMonitorMutex.ReleaseMutex();
            }

            if (h_AhrsSerialPort.IsOpen)
            {
                //Tell AHRS to stop sending data
                h_AhrsSerialPort.Write("P");
            }
        }
        #endregion

        #region ProcessPacket
        private void processPacket(byte[] data, int bytesRead, int packetLength)
        {
            int n = 0;

            while (n < bytesRead)
            {
                if (m_buffer.Count == 0) //buffer is empty
                {
                    //find index of potential first byte which could be header
                    for (; n < bytesRead; n++)
                    {
                        if (data[n] == 0xFF)
                        {
                            break;
                        }//if
                    } //for
                } //if

                //Add data to buffer
                for (; n < bytesRead; n++)
                {
                    //Add data
                    m_buffer.Add(data[n]);

                    //If buffer now holds sufficient points
                    if (m_buffer.Count == packetLength)
                    {
                        int sum = 0;
                        for (int k = 1; k < m_buffer.Count - 1; k++)
                        {
                            sum += m_buffer[k];
                        } //for

                        int checksum = sum % 256;
                        //Check Data is in Sync
                        if (m_buffer[0] == 0xFF && m_buffer[packetLength - 1] == checksum)
                        {
                            m_dataCount++;

                            #region Frequency
                            if (m_freqTimer.ElapsedMilliseconds > 5000)
                            {
                                double currentFreq = 1000.0 * m_dataCount / (m_freqTimer.ElapsedMilliseconds);

                                //Require Minimum Frequency
                                if (currentFreq < MIN_FREQUENCY)
                                {
                                    m_lowFrequency = true;
                                }
                                else
                                {
                                    m_lowFrequency = false;
                                } //if

                                m_freqTimer.Reset();
                                m_freqTimer.Start();
                                m_dataCount = 0;
                            } //if
                            #endregion

                            //Data ready for writing
                            processData();
                            m_buffer.RemoveRange(0, packetLength);
                            //m_buffer.Clear();
                            //h_AhrsSerialPort.DiscardInBuffer();
                            m_receiving = true;
                            m_msg = "Processed data.";

                        } //if data in sync
                        else  //Otherwise data out of sync
                        {
                            m_buffer.RemoveAt(0);
                            while (m_buffer[0] != 0xFF)
                            {
                                m_buffer.RemoveAt(0);
                                if (m_buffer.Count == 0)
                                { //Buffer cleared
                                    break;
                                }
                            }
                        } //else not in sync
                    } //if
                } //for

                n++;

            } //while
        } //processPacketLength

        // function to take m_buffer and convert to data
        private void processData()
        {
            //convert data from array of bytes to proper data format
            DateTime currentTime = DateTime.Now;

            short[] data = new short[12];
            int n = 0;

            // Combine MSB and LSB
            for (n = 0; n < 12; n++)
            {
                data[n] = BitConverter.ToInt16(new byte[2] { m_buffer[2 * n + 2], m_buffer[2 * n + 1] }, 0);
            }
            UInt16 tempSensor = BitConverter.ToUInt16(new byte[2] { m_buffer[2 * n + 2], m_buffer[2 * n + 1] }, 0);
            n++;
            UInt16 timeTicks = BitConverter.ToUInt16(new byte[2] { m_buffer[2 * n + 2], m_buffer[2 * n + 1] }, 0);

            AHRSMutex.WaitOne();
            m_data.time = currentTime.Hour.ToString() + ":" + currentTime.Minute.ToString("00") + ":" + currentTime.Second.ToString("00")
                + "." + currentTime.Millisecond.ToString("000");

            //Angle = data * (scale) / 2^15
            m_data.roll = 1.0 * data[0] * (180) / (1 << 15);
            m_data.pitch = 1.0 * data[1] * (180) / (1 << 15);
            m_data.yaw = 1.0 * data[2] * (180) / (1 << 15);

            //Angular rate = data * (AR * 1.5) / 2^15 ; AR = +/- 200 deg/s
            m_data.v_roll = 1.0 * data[3] * (200 * 1.5) / (1 << 15);
            m_data.v_pitch = 1.0 * data[4] * (200 * 1.5) / (1 << 15);
            m_data.v_yaw = 1.0 * data[5] * (200 * 1.5) / (1 << 15);

            //Accel = data * (GR * 1.5) / 2^15 ; GR = +/- 4G
            m_data.a_x = 1.0 * data[6] * (4 * 1.5) / (1 << 15);
            m_data.a_y = 1.0 * data[7] * (4 * 1.5) / (1 << 15);
            m_data.a_z = 1.0 * data[8] * (4 * 1.5) / (1 << 15);

            //mag = data * (MR * 1.5) / 2^15 .... Magnetic field
            m_data.mag_x = 1.0 * data[9] * (4 * 1.5) / (1 << 15);
            m_data.mag_y = 1.0 * data[10] * (4 * 1.5) / (1 << 15);
            m_data.mag_z = 1.0 * data[11] * (4 * 1.5) / (1 << 15);

            //temp = 44.44 * ( data * 5 / 4096 - 1.375 ) 
            m_data.temp_sensor = 44.44 * (tempSensor * 5 / 4096 - 1.375);

            m_data.ahrs_time = timeTicks;

            AHRSMutex.ReleaseMutex();
        }
        #endregion

        #region PacketLength
        private int determinePacketLength()
        {
            if (!h_AhrsSerialPort.IsOpen) return -1;

            byte[] data = new byte[100];
            List<byte> datalist = new List<byte>();

            int bytesRead = 1;
            Stopwatch stopWatch = new Stopwatch();

            h_AhrsSerialPort.DiscardInBuffer(); //Clear buffer
            h_AhrsSerialPort.Write(new char[1] { 'G' }, 0, 1); //Request three packets
            h_AhrsSerialPort.Write(new char[1] { 'G' }, 0, 1);
            h_AhrsSerialPort.Write(new char[1] { 'G' }, 0, 1);
            stopWatch.Start();

            while (datalist.Count < 90)
            {
                bytesRead = h_AhrsSerialPort.Read(data, 0, data.Length);

                for (int i = 0; i < bytesRead; i++)
                {
                    datalist.Add(data[i]);
                }

                if (stopWatch.ElapsedMilliseconds > 3000)
                {
                    //Serial timeout
                    h_AhrsSerialPort.Close();
                    return 0;
                }
            }

            int verifiedCount = 0;
            int measuredLength = 0;

            for (int n = 0; n < datalist.Count; n++)
            {
                if ((byte)datalist[n] == 0xFF)
                {
                    int lengthCount = 1;
                    int sum = 0;
                    for (n++; n < datalist.Count; n++)
                    {
                        lengthCount++;
                        int checksum = sum % 256;
                        if (checksum == datalist[n] && (n + 1 == datalist.Count || datalist[n + 1] == 0xFF))
                        {
                            if (measuredLength == lengthCount)
                            {
                                verifiedCount++;
                            }
                            else
                            {
                                measuredLength = lengthCount;
                            } //if
                            break;
                        } //if
                        sum += datalist[n];
                    } //for
                } //if
            } //for

            return measuredLength;
        } //determinePacketLength()
        #endregion

        #region Convert
        private double convertAhrs(short _raw, int index)
        {
            double raw = (double)_raw;
            if (raw > 0xffff / 2.0) raw = raw - 0xffff;
            if (index <= 3)
            {
                return raw * (180.0 * 1.0) / 32767.0 + 0;
            }
            else if (index <= 6)
            {
                return raw * (200.0 * 1.5) / 32767.0 + 0;
            }
            else if (index <= 9)
            {
                return raw * (4.0 * 1.5) / 32767.0 + 0;
            }
            return 0;
        }
        #endregion
    }
}
