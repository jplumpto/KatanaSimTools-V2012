using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace IOSCircuitBreakers
{
    public partial class CircuitBreakerApp : Form
    {
        #region PrivateVariables
        private const int ARDUINO_WAIT_TIME = 2000; //2000 ms, Arduino boot time
        private const float STATUS_CHECK_TIME = 30000f; //30,000 ms

        private bool _foundArduino = false;
        private System.Timers.Timer _mTimer = new System.Timers.Timer();
        #endregion PrivateVariables

        public CircuitBreakerApp()
        {
            InitializeComponent();
            find_arduino();

            if (!_foundArduino)
            {
                MessageBox.Show("No Arduino is connected!");
                return;
            }
            initiate_arduino();
        }

        #region Arduino
        private void find_arduino()
        {
            _arduinoCompileDate.Text = "December 1, 2013";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_SerialPort");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string deviceName = queryObj["Name"].ToString();
                    if (deviceName.Contains("Arduino"))
                    {
                        _arduinoPort.PortName = queryObj["DeviceID"].ToString();
                        _arduinoCompileDate.Text = queryObj["DeviceID"].ToString();
                        _foundArduino = true;
                    }
                    
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }

        }

        private void initiate_arduino()
        {
            if (!_arduinoPort.IsOpen)
            {
                try
                {
                    _arduinoPort.Open();
                    _arduinoPort.DiscardInBuffer();
                    _arduinoPort.DiscardOutBuffer();

                    //Wait for Arduino to boot
                    System.Threading.Thread.Sleep(ARDUINO_WAIT_TIME);

                    //Check Code Date
                    _arduinoPort.Write("V;");
                    System.Threading.Thread.Sleep(20);
                    _arduinoCompileDate.Text = _arduinoPort.ReadLine();

                    //With Successful open, initiate timer to check connection
                    _mTimer.Interval = STATUS_CHECK_TIME;
                    _mTimer.Elapsed +=new System.Timers.ElapsedEventHandler( check_arduino);

                }
                catch (System.Exception err)
                {
                    MessageBox.Show("Unable to open COM port:" + err.Message);
                }
            }
        }


        //Function checks periodically for open connection to Arduino
        private void check_arduino(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_arduinoPort.IsOpen)
            {
                try
                {
                    _arduinoPort.Open();
                    _arduinoPort.DiscardInBuffer();
                    _arduinoPort.DiscardOutBuffer();
                }
                catch (System.Exception err)
                {
                    MessageBox.Show("Connection to Arduino lost:" + err.Message);
                    _mTimer.Enabled = false;
                }
            }
        }

        #endregion Arduino
  
        #region Breakers
        private void pop_breaker(int breakNum)
        {
            //string message = "Popping breaker number " + breakNum.ToString();
            //MessageBox.Show(message);

            if (_arduinoPort.IsOpen)
            {
                string cmd = "B" + breakNum.ToString() + ";";
                _arduinoPort.Write(cmd);
            }
            else
            {
                string cmd = "No trip: CB" + breakNum.ToString() + ";";
                _arduinoCompileDate.Text = cmd;
            }

        }

        #region FirstColumn
        private void cb1_Click(object sender, EventArgs e)
        {
            this.pop_breaker(1);
        }

        private void cb2_Click(object sender, EventArgs e)
        {
            this.pop_breaker(2);
        }

        private void cb3_Click(object sender, EventArgs e)
        {
            this.pop_breaker(3);
        }

        private void cb4_Click(object sender, EventArgs e)
        {
            this.pop_breaker(4);
        }

        private void cb5_Click(object sender, EventArgs e)
        {
            this.pop_breaker(5);
        }

        private void cb6_Click(object sender, EventArgs e)
        {
            this.pop_breaker(6);
        }

        private void cb7_Click(object sender, EventArgs e)
        {
            this.pop_breaker(7);
        }

        private void cb8_Click(object sender, EventArgs e)
        {
            this.pop_breaker(8);
        }

        private void cb9_Click(object sender, EventArgs e)
        {
            this.pop_breaker(9);
        }

        private void cb10_Click(object sender, EventArgs e)
        {
            this.pop_breaker(10);
        }

        private void cb11_Click(object sender, EventArgs e)
        {
            this.pop_breaker(11);
        }

        private void cb12_Click(object sender, EventArgs e)
        {
            this.pop_breaker(12);
        }
        #endregion FirstColumn

        #region MiddleColumn
        private void cb13_Click(object sender, EventArgs e)
        {
            this.pop_breaker(13);
        }

        private void cb14_Click(object sender, EventArgs e)
        {
            this.pop_breaker(14);
        }

        private void cb15_Click(object sender, EventArgs e)
        {
            this.pop_breaker(15);
        }

        private void cb16_Click(object sender, EventArgs e)
        {
            this.pop_breaker(16);
        }

        private void cb17_Click(object sender, EventArgs e)
        {
            this.pop_breaker(17);
        }

        private void cb18_Click(object sender, EventArgs e)
        {
            this.pop_breaker(18);
        }

        private void cb19_Click(object sender, EventArgs e)
        {
            this.pop_breaker(19);
        }

        private void cb20_Click(object sender, EventArgs e)
        {
            this.pop_breaker(20);
        }

        private void cb21_Click(object sender, EventArgs e)
        {
            this.pop_breaker(21);
        }

        private void cb22_Click(object sender, EventArgs e)
        {
            this.pop_breaker(22);
        }

        private void cb23_Click(object sender, EventArgs e)
        {
            this.pop_breaker(23);
        }
        #endregion MiddleColumn

        #region ThirdColumn
        private void cb24_Click(object sender, EventArgs e)
        {
            this.pop_breaker(24);
        }

        private void cb25_Click(object sender, EventArgs e)
        {
            this.pop_breaker(25);
        }

        private void cb26_Click(object sender, EventArgs e)
        {
            this.pop_breaker(26);
        }

        private void cb27_Click(object sender, EventArgs e)
        {
            this.pop_breaker(27);
        }
        #endregion ThirdColumn
        #endregion Breakers

    }
}
