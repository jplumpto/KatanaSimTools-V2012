using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ArduinoCalibration
{
    public partial class _arduinoForm : Form
    {
        
        #region Structures
        
        private class ArduinoTest
        {
            public ArduinoTest()
            {

            }

            #region StructVariables
            private Int16 throttle = -1;
            public Int16 Throttle
            {
                get { return throttle;}
                set {throttle = value;}
            }

            private Int16 pitch = -1;
            public Int16 Pitch
            {
                get { return pitch; }
                set { pitch = value; }
            }

            private Int16 roll = -1;
            public Int16 Roll
            {
                get { return roll; }
                set { roll = value; }
            }

            private Int16 yaw = -1;
            public Int16 Yaw
            {
                get { return yaw; }
                set { yaw = value; }
            }

            private Int16 lBrake = -1;
            public Int16 LeftBrake
            {
                get { return lBrake; }
                set { lBrake = value; }
            }

            private Int16 rBrake = -1;
            public Int16 RightBrake
            {
                get { return rBrake; }
                set { rBrake = value; }
            }

            private Int16 carbHeat = -1;
            public Int16 CarbHeat
            {
                get { return carbHeat; }
                set { carbHeat = value; }
            }

            private Int16 choke = 100;
            public Int16 Choke
            {
                get { return choke; }
                set { choke = value; }
            }

            private Int16 parkingBrake = 100;
            public Int16 ParkingBrake
            {
                get { return parkingBrake; }
                set { parkingBrake = value; }
            }

            private Int16 engSpeed = -1;
            public Int16 PropSpeedSpeed
            {
                get { return engSpeed; }
                set { engSpeed = value; }
            }
            #endregion //Struct Variables

            #region Parse
            public void Parse (string results)
            {
                string[] variables = results.Split(';');
                foreach (string var in variables)
                {
                    string[] values = var.Split(':');
                    switch (values[0])
                    {
                        //Throttle
                        case "TH":
                            this.throttle = Int16.Parse(values[1]);
                    	    break;
                        //Pitch
                        case "PI":
                            this.pitch = Int16.Parse(values[1]);
                            break;
                        case "RL":
                            this.roll = Int16.Parse(values[1]);
                            break;
                        case "YW":
                            this.yaw = Int16.Parse(values[1]);
                            break;
                        case "LB":
                            this.lBrake = Int16.Parse(values[1]);
                            break;
                        case "RB":
                            this.rBrake = Int16.Parse(values[1]);
                            break;
                        case "ES":
                            this.engSpeed = Int16.Parse(values[1]);
                            break;
                        case "CH":
                            this.carbHeat = Int16.Parse(values[1]);
                            break;
                        case "CK":
                            this.choke = Int16.Parse(values[1]);
                            break;
                        case "PB":
                            this.parkingBrake = Int16.Parse(values[1]);
                            break;
                    }
                }
            }
            #endregion //Parse
        };
        
        #endregion

        #region Variables
        private const float ARDUINO_WAIT_TIME = 2000f;
        private const float ARDUINO_UPDATE_FREQ = 100f; //Hz

        private bool _updating = false;
        private ArduinoTest _currentState;
        private System.Timers.Timer _mTimer = new System.Timers.Timer();

        public delegate void UpdateControlCallback(string state);
        #endregion

        #region Constructor
        public _arduinoForm()
        {
            InitializeComponent();
            initializeComponents();
        }

        private void initializeComponents()
        {
            string[] ports = SerialPort.GetPortNames();
            _comPortSelection.Items.AddRange(ports);
            _arduinoPort = new SerialPort();

            _currentState = new ArduinoTest();
            _mTimer.Elapsed +=new System.Timers.ElapsedEventHandler( timer_elapsed );
            _mTimer.Interval = ARDUINO_WAIT_TIME*2;

            //open_config_file();
            string path = System.Environment.GetEnvironmentVariable("XPlanePlugin");

            //Check that path exists
            if (path == null)
            {
                folderBrowserDialog1.ShowDialog();
                path = folderBrowserDialog1.SelectedPath;

                if (path == null)
                {
                    return;
                }

                System.Environment.SetEnvironmentVariable("XPlanePlugin", path, EnvironmentVariableTarget.User);

            }
            else
            {
                string filename = path + "\\ArduinoConfig.xml";
                open_config(filename);
            }

            find_arduino();
            
        }
        #endregion

        #region Buttons
        private void _comPortSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            _arduinoPort.PortName = (string)_comPortSelection.SelectedItem;
        }

        private void _bComConnect_Click(object sender, EventArgs e)
        {
            if (!_arduinoPort.IsOpen)
            {
                try
                {
                    _arduinoPort.Open();
                    _arduinoPort.DataReceived += new SerialDataReceivedEventHandler(receive_data);
                    _statusText.Text = "COM port opened.";
                    _arduinoPort.DiscardInBuffer();
                    _arduinoPort.DiscardOutBuffer();
                }
                catch (System.Exception err)
                {
                    _statusText.Text = "Unable to open COM port:" + err.Message;
                }
            }

            if (_arduinoPort.IsOpen)
            {
                _comPortSelection.Enabled = false;
                _bComConnect.Enabled = false;
                _bComClose.Enabled = true;
                _updating = true;
                _mTimer.Enabled = true;
            }
        }

        private void _bComClose_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                try
                {
                    _updating = false;
                    _comPortSelection.Enabled = true;
                    _bComConnect.Enabled = true;
                    _bComClose.Enabled = false;
                    //_arduinoPort.Close();
                    _statusText.Text = "COM port closed.";
                }
                catch (System.Exception err)
                {
                    _statusText.Text = "Closed COM port:" + err.Message;
                }
            }
        }
        
        private void _bThrottle_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _throttleLevel.Minimum = _currentState.Throttle;
                _throttleLevel.Maximum = _currentState.Throttle + 1;
            }
        }

        private void _bPropSpeed_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _propSpeedLevel.Minimum = _currentState.PropSpeedSpeed;
                _propSpeedLevel.Maximum = _currentState.PropSpeedSpeed + 1;
            }
        }
        
        private void _bPitch_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _pitchLevel.Minimum = _currentState.Pitch;
                _pitchLevel.Maximum = _currentState.Pitch + 1;
            }
        }

        private void _bRoll_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _rollLevel.Minimum = _currentState.Roll;
                _rollLevel.Maximum = _currentState.Roll + 1;
            }

        }

        private void _bYaw_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _yawLevel.Minimum = _currentState.Yaw;
                _yawLevel.Maximum = _currentState.Yaw + 1;
            }
        }

        private void _bCarbHeat_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _carbHeatLevel.Minimum = _currentState.CarbHeat;
                _carbHeatLevel.Maximum = _currentState.CarbHeat + 1;
            }

        }

        private void _bLeftBrake_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _leftBrakeLevel.Minimum = _currentState.LeftBrake;
                _leftBrakeLevel.Maximum = _currentState.LeftBrake + 1;
            }

        }

        private void _bRightBrake_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _rightBrakeLevel.Minimum = _currentState.RightBrake;
                _rightBrakeLevel.Maximum = _currentState.RightBrake + 1;
            }

        }

        private void _bChoke_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _chokeLevel.Minimum = _currentState.Choke;
                _chokeLevel.Maximum = _currentState.Choke + 1;
            }
        }

        private void _bParkingBrake_Click(object sender, EventArgs e)
        {
            if (_arduinoPort.IsOpen)
            {
                _parkBrakeLevel.Minimum = _currentState.ParkingBrake;
                _parkBrakeLevel.Maximum = _currentState.ParkingBrake + 1;
            }
        }

        #endregion

        #region HelperFunctions
        private void call_for_update()
        {
            _arduinoPort.WriteLine("C;");
        }

        #region UpdateCallbacks
        private void update_throttle_state(string state)
        {
            switch (state)
            {
                case "min":
                    _throttleLevel.Minimum = _currentState.Throttle;
                    break;
                case "max":
                    _throttleLevel.Maximum = _currentState.Throttle;
                    break;
                case "value":
                    int throttleState = _currentState.Throttle;

                    if (_throttleInvert.Checked)
                    {
                        throttleState = (100 - 100 * (throttleState - _throttleLevel.Minimum) / (_throttleLevel.Maximum - _throttleLevel.Minimum)) * (_throttleLevel.Maximum - _throttleLevel.Minimum) / 100 + _throttleLevel.Minimum;
                    }

                    _throttleLevel.Value = throttleState;
                    break;
            }
        }

        private void update_pitch_state(string state)
        {
            switch (state)
            {
                case "min":
                    _pitchLevel.Minimum = _currentState.Pitch;
                    break;
                case "max":
                    _pitchLevel.Maximum = _currentState.Pitch;
                    break;
                case "value":
                    int pitchState = _currentState.Pitch;

                    if (_pitchInvert.Checked)
                    {
                        pitchState = (100 - 100 * (pitchState - _pitchLevel.Minimum) / (_pitchLevel.Maximum - _pitchLevel.Minimum)) * (_pitchLevel.Maximum - _pitchLevel.Minimum) / 100 + _pitchLevel.Minimum;
                    }

                    _pitchLevel.Value = pitchState;
                    break;
            }
        }

        private void update_roll_state(string state)
        {
            switch (state)
            {
                case "min":
                    _rollLevel.Minimum = _currentState.Roll;
                    break;
                case "max":
                    _rollLevel.Maximum = _currentState.Roll;
                    break;
                case "value":
                    int rollState = _currentState.Roll;

                    if (_rollInvert.Checked)
                    {
                        rollState = (100 - 100 * (rollState - _rollLevel.Minimum) / (_rollLevel.Maximum - _rollLevel.Minimum)) * (_rollLevel.Maximum - _rollLevel.Minimum) / 100 + _rollLevel.Minimum;
                    }

                    _rollLevel.Value = rollState;
                    break;
            }
        }

        private void update_yaw_state(string state)
        {
            switch (state)
            {
                case "min":
                    _yawLevel.Minimum = _currentState.Yaw;
                    break;
                case "max":
                    _yawLevel.Maximum = _currentState.Yaw;
                    break;
                case "value":
                    int yawState = _currentState.Yaw;

                    if (_yawInvert.Checked)
                    {
                        yawState = (100 - 100 * (yawState - _yawLevel.Minimum) / (_yawLevel.Maximum - _yawLevel.Minimum)) * (_yawLevel.Maximum - _yawLevel.Minimum) / 100 + _yawLevel.Minimum;
                    }

                    _yawLevel.Value = yawState;
                    break;
            }
        }

        private void update_propeller_speed_state(string state)
        {
            switch (state)
            {
                case "min":
                    _propSpeedLevel.Minimum = _currentState.PropSpeedSpeed;
                    break;
                case "max":
                    _propSpeedLevel.Maximum = _currentState.PropSpeedSpeed;
                    break;
                case "value":
                    int propSpeedState = _currentState.PropSpeedSpeed;

                    if (_propSpeedInvert.Checked)
                    {
                        propSpeedState = (100 - 100 * (propSpeedState - _propSpeedLevel.Minimum) / (_propSpeedLevel.Maximum - _propSpeedLevel.Minimum)) * (_propSpeedLevel.Maximum - _propSpeedLevel.Minimum) / 100 + _propSpeedLevel.Minimum;
                    }

                    _propSpeedLevel.Value = propSpeedState;
                    break;
            }
        }

        private void update_carb_heat_state(string state)
        {
            switch (state)
            {
                case "min":
                    _carbHeatLevel.Minimum = _currentState.CarbHeat;
                    break;
                case "max":
                    _carbHeatLevel.Maximum = _currentState.CarbHeat;
                    break;
                case "value":
                    int carbHeatState = _currentState.CarbHeat;

                    if (_carbHeatInvert.Checked)
                    {
                        carbHeatState = (100 - 100 * (carbHeatState - _carbHeatLevel.Minimum) / (_carbHeatLevel.Maximum - _carbHeatLevel.Minimum)) * (_carbHeatLevel.Maximum - _carbHeatLevel.Minimum) / 100 + _carbHeatLevel.Minimum;
                    }

                    _carbHeatLevel.Value = carbHeatState;
                    break;
            }
        }

        private void update_choke_state(string state)
        {
            switch (state)
            {
                case "min":
                    _chokeLevel.Minimum = _currentState.Choke;
                    break;
                case "max":
                    _chokeLevel.Maximum = _currentState.Choke;
                    break;
                case "value":
                    int chokeState = _currentState.Choke;

                    if (_chokeInvert.Checked)
                    {
                        chokeState = (100 - 100 * (chokeState - _rollLevel.Minimum) / (_chokeLevel.Maximum - _chokeLevel.Minimum)) * (_chokeLevel.Maximum - _chokeLevel.Minimum) / 100 + _chokeLevel.Minimum;
                    }

                    _chokeLevel.Value = chokeState;
                    break;
            }
        }

        private void update_parking_brake_state(string state)
        {
            switch (state)
            {
                case "min":
                    _parkBrakeLevel.Minimum = _currentState.ParkingBrake;
                    break;
                case "max":
                    _parkBrakeLevel.Maximum = _currentState.ParkingBrake;
                    break;
                case "value":
                    int parkBrakeState = _currentState.ParkingBrake;

                    if (_parkBrakeInvert.Checked)
                    {
                        parkBrakeState = (100 - 100 * (parkBrakeState - _parkBrakeLevel.Minimum) / (_parkBrakeLevel.Maximum - _parkBrakeLevel.Minimum)) * (_parkBrakeLevel.Maximum - _parkBrakeLevel.Minimum) / 100 + _parkBrakeLevel.Minimum;
                    }

                    _parkBrakeLevel.Value = parkBrakeState;
                    break;
            }
        }

        private void update_left_brake_state(string state)
        {
            switch (state)
            {
                case "min":
                    _leftBrakeLevel.Minimum = _currentState.LeftBrake;
                    break;
                case "max":
                    _leftBrakeLevel.Maximum = _currentState.LeftBrake;
                    break;
                case "value":
                    int leftBrakeState = _currentState.LeftBrake;

                    if (_leftBrakeInvert.Checked)
                    {
                        leftBrakeState = (100 - 100 * (leftBrakeState - _leftBrakeLevel.Minimum) / (_leftBrakeLevel.Maximum - _leftBrakeLevel.Minimum)) * (_leftBrakeLevel.Maximum - _leftBrakeLevel.Minimum) / 100 + _leftBrakeLevel.Minimum;
                    }

                    _leftBrakeLevel.Value = leftBrakeState;
                    break;
            }
        }

        private void update_right_brake_state(string state)
        {
            switch (state)
            {
                case "min":
                    _rightBrakeLevel.Minimum = _currentState.RightBrake;
                    break;
                case "max":
                    _rightBrakeLevel.Maximum = _currentState.RightBrake;
                    break;
                case "value":
                    int rightBrakeState = _currentState.RightBrake;

                    if (_rightBrakeInvert.Checked)
                    {
                        rightBrakeState = (100 - 100 * (rightBrakeState - _rightBrakeLevel.Minimum) / (_rightBrakeLevel.Maximum - _rightBrakeLevel.Minimum)) * (_rightBrakeLevel.Maximum - _rightBrakeLevel.Minimum) / 100 + _rightBrakeLevel.Minimum;
                    }

                    _rightBrakeLevel.Value = rightBrakeState;
                    break;
            }
        }
        #endregion

        private void update_state()
        {
            #region Throttle
            if (_currentState.Throttle > _throttleLevel.Maximum)
            {
                _throttleLevel.Invoke(new UpdateControlCallback(this.update_throttle_state),new object[]{"max"});
            } else if (_currentState.Throttle < _throttleLevel.Minimum)
            {
                _throttleLevel.Invoke(new UpdateControlCallback(this.update_throttle_state), new object[]{"min"});
            }
            _throttleLevel.Invoke(new UpdateControlCallback(this.update_throttle_state), new object[]{"value"});
            #endregion

            #region Pitch
            if (_currentState.Pitch > _pitchLevel.Maximum)
            {
                _pitchLevel.Invoke(new UpdateControlCallback(this.update_pitch_state), new object[] { "max" });
            }
            else if (_currentState.Pitch < _pitchLevel.Minimum)
            {
                _pitchLevel.Invoke(new UpdateControlCallback(this.update_pitch_state), new object[] { "min" });
            }
            _pitchLevel.Invoke(new UpdateControlCallback(this.update_pitch_state), new object[] { "value" });
            #endregion

            #region Roll
            if (_currentState.Roll > _rollLevel.Maximum)
            {
                _rollLevel.Invoke(new UpdateControlCallback(this.update_roll_state), new object[] { "max" });
            }
            else if (_currentState.Roll < _rollLevel.Minimum)
            {
                _rollLevel.Invoke(new UpdateControlCallback(this.update_roll_state), new object[] { "min" });
            }
            _rollLevel.Invoke(new UpdateControlCallback(this.update_roll_state), new object[] { "value" });
            #endregion

            #region Yaw
            if (_currentState.Yaw > _yawLevel.Maximum)
            {
                _yawLevel.Invoke(new UpdateControlCallback(this.update_yaw_state), new object[] { "max" });
            }
            else if (_currentState.Yaw < _yawLevel.Minimum)
            {
                _yawLevel.Invoke(new UpdateControlCallback(this.update_yaw_state), new object[] { "min" });
            }
            _yawLevel.Invoke(new UpdateControlCallback(this.update_yaw_state), new object[] { "value" });
            #endregion

            #region PropSpeedSpeed
            if (_currentState.PropSpeedSpeed > _propSpeedLevel.Maximum)
            {
                _propSpeedLevel.Invoke(new UpdateControlCallback(this.update_propeller_speed_state), new object[] { "max" });
            }
            else if (_currentState.PropSpeedSpeed < _propSpeedLevel.Minimum)
            {
                _propSpeedLevel.Invoke(new UpdateControlCallback(this.update_propeller_speed_state), new object[] { "min" });
            }
            _propSpeedLevel.Invoke(new UpdateControlCallback(this.update_propeller_speed_state), new object[] { "value" });
            #endregion

            #region CarbHeat
            if (_currentState.CarbHeat > _carbHeatLevel.Maximum)
            {
                _carbHeatLevel.Invoke(new UpdateControlCallback(this.update_carb_heat_state), new object[] { "max" });
            }
            else if (_currentState.CarbHeat < _carbHeatLevel.Minimum)
            {
                _carbHeatLevel.Invoke(new UpdateControlCallback(this.update_carb_heat_state), new object[] { "min" });
            }
            _carbHeatLevel.Invoke(new UpdateControlCallback(this.update_carb_heat_state), new object[] { "value" });
            #endregion

            #region Choke
            if (_currentState.Choke > _chokeLevel.Maximum)
            {
                _chokeLevel.Invoke(new UpdateControlCallback(this.update_choke_state), new object[] { "max" });
            }
            else if (_currentState.Choke < _chokeLevel.Minimum)
            {
                _chokeLevel.Invoke(new UpdateControlCallback(this.update_choke_state), new object[] { "min" });
            }
            _chokeLevel.Invoke(new UpdateControlCallback(this.update_choke_state), new object[] { "value" });
            #endregion

            #region ParkingBrake
            if (_currentState.ParkingBrake > _parkBrakeLevel.Maximum)
            {
                _parkBrakeLevel.Invoke(new UpdateControlCallback(this.update_parking_brake_state), new object[] { "max" });
            }
            else if (_currentState.ParkingBrake < _parkBrakeLevel.Minimum)
            {
                _parkBrakeLevel.Invoke(new UpdateControlCallback(this.update_parking_brake_state), new object[] { "min" });
            }
            _parkBrakeLevel.Invoke(new UpdateControlCallback(this.update_parking_brake_state), new object[] { "value" });
            #endregion

            #region LeftBrake
            if (_currentState.LeftBrake > _leftBrakeLevel.Maximum)
            {
                _leftBrakeLevel.Invoke(new UpdateControlCallback(this.update_left_brake_state), new object[] { "max" });
            }
            else if (_currentState.LeftBrake < _leftBrakeLevel.Minimum)
            {
                _leftBrakeLevel.Invoke(new UpdateControlCallback(this.update_left_brake_state), new object[] { "min" });
            }
            _leftBrakeLevel.Invoke(new UpdateControlCallback(this.update_left_brake_state), new object[] { "value" });
            #endregion

            #region RightBrake
            if (_currentState.RightBrake > _rightBrakeLevel.Maximum)
            {
                _rightBrakeLevel.Invoke(new UpdateControlCallback(this.update_right_brake_state), new object[] { "max" });
            }
            else if (_currentState.RightBrake < _rightBrakeLevel.Minimum)
            {
                _rightBrakeLevel.Invoke(new UpdateControlCallback(this.update_right_brake_state), new object[] { "min" });
            }
            _rightBrakeLevel.Invoke(new UpdateControlCallback(this.update_right_brake_state), new object[] { "value" });
            #endregion
        }
        
        private void receive_data( object sender, System.IO.Ports.SerialDataReceivedEventArgs e )
        {
            if (!_updating)
            {
                return;
            }

            //.Net 3.5 COM ports are kind of buggy when using asynchronous model.
            //Check if the port is still open on data receive otherwise we throw out this packet
            //and re-open the port
            if (!_arduinoPort.IsOpen)
            {
                _arduinoPort.Open();
            } 
            else
            {
                string line = _arduinoPort.ReadLine();
                //_statusText.Text = line;
                _currentState.Parse(line);
                update_state();
            }
            //this.Refresh();
            Application.DoEvents();

        }
        
        private void timer_elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_mTimer.Interval == ARDUINO_WAIT_TIME*2)
            {
                _mTimer.Interval = 1000f / ARDUINO_UPDATE_FREQ; //update interval (ms)
            }

            if (_updating)
            {
                //Request a calibration packet
                _arduinoPort.Write("C;");
            } else {
                _mTimer.Enabled = false;
            }
        }

        private void find_arduino()
        {
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
                        _comPortSelection.Text = queryObj["DeviceID"].ToString();
                        return;
                    }

                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }

        }


        #endregion

        #region Config
        private void _newConfig_Click(object sender, EventArgs e)
        {
            try
            {

                string path = System.Environment.GetEnvironmentVariable("XPlanePlugin");
                string filename = path + "\\ArduinoConfig.xml";
                ConfigSettings config = new ConfigSettings();
                config.ArduinoPort = _arduinoPort.PortName;
                config.ThrottleMin = _throttleLevel.Minimum;
                config.ThrottleMax = _throttleLevel.Maximum;
                config.ThrottleInvert = _throttleInvert.Checked ? 1 : 0;
                config.PitchMin = _pitchLevel.Minimum;
                config.PitchMax = _pitchLevel.Maximum;
                config.PitchInvert = _pitchInvert.Checked ? 1 : 0;
                config.RollMin = _rollLevel.Minimum;
                config.RollMax = _rollLevel.Maximum;
                config.RollInvert = _rollInvert.Checked ? 1 : 0;
                config.YawMin = _yawLevel.Minimum;
                config.YawMax = _yawLevel.Maximum;
                config.YawInvert = _yawInvert.Checked ? 1 : 0;
                config.PropSpeedMin = _propSpeedLevel.Minimum;
                config.PropSpeedMax = _propSpeedLevel.Maximum;
                config.PropSpeedInvert = _propSpeedInvert.Checked ? 1 : 0;
                config.CarbHeatMin = _carbHeatLevel.Minimum;
                config.CarbHeatMax = _carbHeatLevel.Maximum;
                config.CarbHeatInvert = _carbHeatInvert.Checked ? 1 : 0;
                config.ChokeMin = _chokeLevel.Minimum;
                config.ChokeMax = _chokeLevel.Maximum;
                config.ChokeInvert = _chokeInvert.Checked ? 1 : 0;
                config.ParkBrakeMin = _parkBrakeLevel.Minimum;
                config.ParkBrakeMax = _parkBrakeLevel.Maximum;
                config.ParkBrakeInvert = _parkBrakeInvert.Checked ? 1 : 0;
                config.LeftBrakeMin = _leftBrakeLevel.Minimum;
                config.LeftBrakeMax = _leftBrakeLevel.Maximum;
                config.LeftBrakeInvert = _leftBrakeInvert.Checked ? 1 : 0;
                config.RightBrakeMin = _rightBrakeLevel.Minimum;
                config.RightBrakeMax = _rightBrakeLevel.Maximum;
                config.RightBrakeInvert = _rightBrakeInvert.Checked ? 1 : 0;

                config.Serialize(filename);
                _statusText.Text = "Saved to " + filename;
            }
            catch (System.Exception ex)
            {
                _statusText.Text = ex.Message;
            }
        }

//         private void _bOpenConfig_Click(object sender, EventArgs e)
//         {
//             string fName;
//             if (openFileDialog1.ShowDialog() == DialogResult.OK)
//             {
//                 fName = openFileDialog1.FileName;
//                 open_config(fName);
//             }
//         }

        private void open_config(string filename)
        {
            try
            {
                ConfigSettings config = ConfigSettings.DeSerialize(filename);

                _arduinoPort.PortName = config.ArduinoPort;
                _comPortSelection.Text = config.ArduinoPort;
                _throttleLevel.Minimum = config.ThrottleMin;
                _throttleLevel.Maximum = config.ThrottleMax;
                _throttleInvert.Checked = config.ThrottleInvert == 1;
                _pitchLevel.Minimum = config.PitchMin;
                _pitchLevel.Maximum = config.PitchMax;
                _pitchInvert.Checked = config.PitchInvert == 1;
                _rollLevel.Minimum = config.RollMin;
                _rollLevel.Maximum = config.RollMax;
                _rollInvert.Checked = config.RollInvert == 1;
                _yawLevel.Minimum = config.YawMin;
                _yawLevel.Maximum = config.YawMax;
                _yawInvert.Checked = config.YawInvert == 1;
                
                _propSpeedLevel.Minimum = config.PropSpeedMin;
                _propSpeedLevel.Maximum = config.PropSpeedMax;
                _propSpeedInvert.Checked = config.PropSpeedInvert == 1;
                _carbHeatLevel.Minimum = config.CarbHeatMin;
                _carbHeatLevel.Maximum = config.CarbHeatMax;
                _carbHeatInvert.Checked = config.CarbHeatInvert == 1;
                _chokeLevel.Minimum = config.ChokeMin;
                _chokeLevel.Maximum = config.ChokeMax;
                _chokeInvert.Checked = config.ChokeInvert == 1;
                _parkBrakeLevel.Minimum = config.ParkBrakeMin;
                _parkBrakeLevel.Maximum = config.ParkBrakeMax;
                _parkBrakeInvert.Checked = config.ParkBrakeInvert == 1;
                _leftBrakeLevel.Minimum = config.LeftBrakeMin;
                _leftBrakeLevel.Maximum = config.LeftBrakeMax;
                _leftBrakeInvert.Checked = config.LeftBrakeInvert == 1;
                _rightBrakeLevel.Minimum = config.RightBrakeMin;
                _rightBrakeLevel.Maximum = config.RightBrakeMax;
                _rightBrakeInvert.Checked = config.RightBrakeInvert == 1;

                _statusText.Text = "Opened Config File: " + filename;
            }
            catch (System.Exception ex)
            {
                _statusText.Text = ex.Message;
            }
        }
        #endregion

        private void _arduinoTimer_Tick(object sender, EventArgs e)
        {

        }

                
        
    }
}
