namespace ArduinoCalibration
{
    partial class _arduinoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._comPortSelection = new System.Windows.Forms.ComboBox();
            this._throttleLevel = new System.Windows.Forms.ProgressBar();
            this._propSpeedLevel = new System.Windows.Forms.ProgressBar();
            this._pitchLevel = new System.Windows.Forms.ProgressBar();
            this._rollLevel = new System.Windows.Forms.ProgressBar();
            this._yawLevel = new System.Windows.Forms.ProgressBar();
            this._bThrottle = new System.Windows.Forms.Button();
            this._bPropSpeed = new System.Windows.Forms.Button();
            this._bPitch = new System.Windows.Forms.Button();
            this._bRoll = new System.Windows.Forms.Button();
            this._bYaw = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._bComConnect = new System.Windows.Forms.Button();
            this._statusText = new System.Windows.Forms.TextBox();
            this._bComClose = new System.Windows.Forms.Button();
            this._arduinoPort = new System.IO.Ports.SerialPort(this.components);
            this._arduinoTimer = new System.Windows.Forms.Timer(this.components);
            this._newConfig = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label7 = new System.Windows.Forms.Label();
            this._bCarbHeat = new System.Windows.Forms.Button();
            this._carbHeatLevel = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this._bLeftBrake = new System.Windows.Forms.Button();
            this._leftBrakeLevel = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this._bRightBrake = new System.Windows.Forms.Button();
            this._rightBrakeLevel = new System.Windows.Forms.ProgressBar();
            this.label10 = new System.Windows.Forms.Label();
            this._bChoke = new System.Windows.Forms.Button();
            this._chokeLevel = new System.Windows.Forms.ProgressBar();
            this.label11 = new System.Windows.Forms.Label();
            this._bParkingBrake = new System.Windows.Forms.Button();
            this._parkBrakeLevel = new System.Windows.Forms.ProgressBar();
            this._throttleInvert = new System.Windows.Forms.CheckBox();
            this._propSpeedInvert = new System.Windows.Forms.CheckBox();
            this._carbHeatInvert = new System.Windows.Forms.CheckBox();
            this._chokeInvert = new System.Windows.Forms.CheckBox();
            this._parkBrakeInvert = new System.Windows.Forms.CheckBox();
            this._pitchInvert = new System.Windows.Forms.CheckBox();
            this._rollInvert = new System.Windows.Forms.CheckBox();
            this._yawInvert = new System.Windows.Forms.CheckBox();
            this._leftBrakeInvert = new System.Windows.Forms.CheckBox();
            this._rightBrakeInvert = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _comPortSelection
            // 
            this._comPortSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comPortSelection.FormattingEnabled = true;
            this._comPortSelection.Location = new System.Drawing.Point(235, 27);
            this._comPortSelection.Name = "_comPortSelection";
            this._comPortSelection.Size = new System.Drawing.Size(75, 21);
            this._comPortSelection.TabIndex = 0;
            this._comPortSelection.SelectedIndexChanged += new System.EventHandler(this._comPortSelection_SelectedIndexChanged);
            // 
            // _throttleLevel
            // 
            this._throttleLevel.Location = new System.Drawing.Point(90, 120);
            this._throttleLevel.Name = "_throttleLevel";
            this._throttleLevel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._throttleLevel.Size = new System.Drawing.Size(250, 15);
            this._throttleLevel.TabIndex = 1;
            // 
            // _propSpeedLevel
            // 
            this._propSpeedLevel.Location = new System.Drawing.Point(90, 188);
            this._propSpeedLevel.Name = "_propSpeedLevel";
            this._propSpeedLevel.Size = new System.Drawing.Size(250, 15);
            this._propSpeedLevel.TabIndex = 2;
            // 
            // _pitchLevel
            // 
            this._pitchLevel.Location = new System.Drawing.Point(434, 120);
            this._pitchLevel.Name = "_pitchLevel";
            this._pitchLevel.Size = new System.Drawing.Size(250, 15);
            this._pitchLevel.TabIndex = 3;
            // 
            // _rollLevel
            // 
            this._rollLevel.Location = new System.Drawing.Point(434, 192);
            this._rollLevel.Name = "_rollLevel";
            this._rollLevel.Size = new System.Drawing.Size(250, 15);
            this._rollLevel.TabIndex = 4;
            // 
            // _yawLevel
            // 
            this._yawLevel.Location = new System.Drawing.Point(434, 261);
            this._yawLevel.Name = "_yawLevel";
            this._yawLevel.Size = new System.Drawing.Size(250, 15);
            this._yawLevel.TabIndex = 5;
            // 
            // _bThrottle
            // 
            this._bThrottle.Location = new System.Drawing.Point(162, 91);
            this._bThrottle.Name = "_bThrottle";
            this._bThrottle.Size = new System.Drawing.Size(108, 23);
            this._bThrottle.TabIndex = 3;
            this._bThrottle.Text = "Reset Throttle";
            this._bThrottle.UseVisualStyleBackColor = true;
            this._bThrottle.Click += new System.EventHandler(this._bThrottle_Click);
            // 
            // _bPropSpeed
            // 
            this._bPropSpeed.Location = new System.Drawing.Point(162, 159);
            this._bPropSpeed.Name = "_bPropSpeed";
            this._bPropSpeed.Size = new System.Drawing.Size(108, 23);
            this._bPropSpeed.TabIndex = 4;
            this._bPropSpeed.Text = "Reset RPM";
            this._bPropSpeed.UseVisualStyleBackColor = true;
            this._bPropSpeed.Click += new System.EventHandler(this._bPropSpeed_Click);
            // 
            // _bPitch
            // 
            this._bPitch.Location = new System.Drawing.Point(506, 91);
            this._bPitch.Name = "_bPitch";
            this._bPitch.Size = new System.Drawing.Size(108, 23);
            this._bPitch.TabIndex = 5;
            this._bPitch.Text = "Reset Pitch";
            this._bPitch.UseVisualStyleBackColor = true;
            this._bPitch.Click += new System.EventHandler(this._bPitch_Click);
            // 
            // _bRoll
            // 
            this._bRoll.Location = new System.Drawing.Point(506, 163);
            this._bRoll.Name = "_bRoll";
            this._bRoll.Size = new System.Drawing.Size(108, 23);
            this._bRoll.TabIndex = 6;
            this._bRoll.Text = "Reset Roll";
            this._bRoll.UseVisualStyleBackColor = true;
            this._bRoll.Click += new System.EventHandler(this._bRoll_Click);
            // 
            // _bYaw
            // 
            this._bYaw.Location = new System.Drawing.Point(506, 232);
            this._bYaw.Name = "_bYaw";
            this._bYaw.Size = new System.Drawing.Size(108, 23);
            this._bYaw.TabIndex = 7;
            this._bYaw.Text = "Reset Yaw";
            this._bYaw.UseVisualStyleBackColor = true;
            this._bYaw.Click += new System.EventHandler(this._bYaw_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Throttle:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Prop Speed:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(394, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Pitch:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(400, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Roll:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(397, 263);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Yaw:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(161, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Arduino Port:";
            // 
            // _bComConnect
            // 
            this._bComConnect.Location = new System.Drawing.Point(336, 25);
            this._bComConnect.Name = "_bComConnect";
            this._bComConnect.Size = new System.Drawing.Size(75, 23);
            this._bComConnect.TabIndex = 1;
            this._bComConnect.Text = "Connect";
            this._bComConnect.UseVisualStyleBackColor = true;
            this._bComConnect.Click += new System.EventHandler(this._bComConnect_Click);
            // 
            // _statusText
            // 
            this._statusText.Location = new System.Drawing.Point(164, 62);
            this._statusText.Name = "_statusText";
            this._statusText.ReadOnly = true;
            this._statusText.Size = new System.Drawing.Size(409, 20);
            this._statusText.TabIndex = 18;
            this._statusText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _bComClose
            // 
            this._bComClose.Enabled = false;
            this._bComClose.Location = new System.Drawing.Point(417, 25);
            this._bComClose.Name = "_bComClose";
            this._bComClose.Size = new System.Drawing.Size(75, 23);
            this._bComClose.TabIndex = 2;
            this._bComClose.Text = "Disconnect";
            this._bComClose.UseVisualStyleBackColor = true;
            this._bComClose.Click += new System.EventHandler(this._bComClose_Click);
            // 
            // _arduinoTimer
            // 
            this._arduinoTimer.Interval = 2000;
            this._arduinoTimer.Tick += new System.EventHandler(this._arduinoTimer_Tick);
            // 
            // _newConfig
            // 
            this._newConfig.Location = new System.Drawing.Point(498, 25);
            this._newConfig.Name = "_newConfig";
            this._newConfig.Size = new System.Drawing.Size(75, 23);
            this._newConfig.TabIndex = 20;
            this._newConfig.Text = "Save Config";
            this._newConfig.UseVisualStyleBackColor = true;
            this._newConfig.Click += new System.EventHandler(this._newConfig_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "xml files|*.xml|All Files|*.*";
            this.openFileDialog1.InitialDirectory = "c:\\";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select X-Plane Plugin Folder";
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 263);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Carb Heat:";
            // 
            // _bCarbHeat
            // 
            this._bCarbHeat.Location = new System.Drawing.Point(162, 232);
            this._bCarbHeat.Name = "_bCarbHeat";
            this._bCarbHeat.Size = new System.Drawing.Size(108, 23);
            this._bCarbHeat.TabIndex = 8;
            this._bCarbHeat.Text = "Reset Carb Heat";
            this._bCarbHeat.UseVisualStyleBackColor = true;
            this._bCarbHeat.Click += new System.EventHandler(this._bCarbHeat_Click);
            // 
            // _carbHeatLevel
            // 
            this._carbHeatLevel.Location = new System.Drawing.Point(90, 261);
            this._carbHeatLevel.Name = "_carbHeatLevel";
            this._carbHeatLevel.Size = new System.Drawing.Size(250, 15);
            this._carbHeatLevel.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(370, 334);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Left Brake:";
            // 
            // _bLeftBrake
            // 
            this._bLeftBrake.Location = new System.Drawing.Point(506, 303);
            this._bLeftBrake.Name = "_bLeftBrake";
            this._bLeftBrake.Size = new System.Drawing.Size(108, 23);
            this._bLeftBrake.TabIndex = 9;
            this._bLeftBrake.Text = "Reset Left Brake";
            this._bLeftBrake.UseVisualStyleBackColor = true;
            this._bLeftBrake.Click += new System.EventHandler(this._bLeftBrake_Click);
            // 
            // _leftBrakeLevel
            // 
            this._leftBrakeLevel.Location = new System.Drawing.Point(434, 332);
            this._leftBrakeLevel.Name = "_leftBrakeLevel";
            this._leftBrakeLevel.Size = new System.Drawing.Size(250, 15);
            this._leftBrakeLevel.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(369, 396);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Right Brake";
            // 
            // _bRightBrake
            // 
            this._bRightBrake.Location = new System.Drawing.Point(505, 365);
            this._bRightBrake.Name = "_bRightBrake";
            this._bRightBrake.Size = new System.Drawing.Size(108, 23);
            this._bRightBrake.TabIndex = 10;
            this._bRightBrake.Text = "Reset Right Brake";
            this._bRightBrake.UseVisualStyleBackColor = true;
            this._bRightBrake.Click += new System.EventHandler(this._bRightBrake_Click);
            // 
            // _rightBrakeLevel
            // 
            this._rightBrakeLevel.Location = new System.Drawing.Point(433, 394);
            this._rightBrakeLevel.Name = "_rightBrakeLevel";
            this._rightBrakeLevel.Size = new System.Drawing.Size(250, 15);
            this._rightBrakeLevel.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(39, 334);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "Choke:";
            // 
            // _bChoke
            // 
            this._bChoke.Location = new System.Drawing.Point(164, 303);
            this._bChoke.Name = "_bChoke";
            this._bChoke.Size = new System.Drawing.Size(108, 23);
            this._bChoke.TabIndex = 33;
            this._bChoke.Text = "Reset Choke";
            this._bChoke.UseVisualStyleBackColor = true;
            this._bChoke.Click += new System.EventHandler(this._bChoke_Click);
            // 
            // _chokeLevel
            // 
            this._chokeLevel.Location = new System.Drawing.Point(92, 332);
            this._chokeLevel.Name = "_chokeLevel";
            this._chokeLevel.Size = new System.Drawing.Size(250, 15);
            this._chokeLevel.TabIndex = 34;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 396);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Parking Brake:";
            // 
            // _bParkingBrake
            // 
            this._bParkingBrake.Location = new System.Drawing.Point(164, 365);
            this._bParkingBrake.Name = "_bParkingBrake";
            this._bParkingBrake.Size = new System.Drawing.Size(108, 23);
            this._bParkingBrake.TabIndex = 36;
            this._bParkingBrake.Text = "Reset Park Brake";
            this._bParkingBrake.UseVisualStyleBackColor = true;
            this._bParkingBrake.Click += new System.EventHandler(this._bParkingBrake_Click);
            // 
            // _parkBrakeLevel
            // 
            this._parkBrakeLevel.Location = new System.Drawing.Point(92, 394);
            this._parkBrakeLevel.Name = "_parkBrakeLevel";
            this._parkBrakeLevel.Size = new System.Drawing.Size(250, 15);
            this._parkBrakeLevel.TabIndex = 37;
            // 
            // _throttleInvert
            // 
            this._throttleInvert.AutoSize = true;
            this._throttleInvert.Location = new System.Drawing.Point(277, 95);
            this._throttleInvert.Name = "_throttleInvert";
            this._throttleInvert.Size = new System.Drawing.Size(53, 17);
            this._throttleInvert.TabIndex = 39;
            this._throttleInvert.Text = "Invert";
            this._throttleInvert.UseVisualStyleBackColor = true;
            // 
            // _propSpeedInvert
            // 
            this._propSpeedInvert.AutoSize = true;
            this._propSpeedInvert.Location = new System.Drawing.Point(277, 163);
            this._propSpeedInvert.Name = "_propSpeedInvert";
            this._propSpeedInvert.Size = new System.Drawing.Size(53, 17);
            this._propSpeedInvert.TabIndex = 40;
            this._propSpeedInvert.Text = "Invert";
            this._propSpeedInvert.UseVisualStyleBackColor = true;
            // 
            // _carbHeatInvert
            // 
            this._carbHeatInvert.AutoSize = true;
            this._carbHeatInvert.Location = new System.Drawing.Point(277, 236);
            this._carbHeatInvert.Name = "_carbHeatInvert";
            this._carbHeatInvert.Size = new System.Drawing.Size(53, 17);
            this._carbHeatInvert.TabIndex = 41;
            this._carbHeatInvert.Text = "Invert";
            this._carbHeatInvert.UseVisualStyleBackColor = true;
            // 
            // _chokeInvert
            // 
            this._chokeInvert.AutoSize = true;
            this._chokeInvert.Location = new System.Drawing.Point(277, 307);
            this._chokeInvert.Name = "_chokeInvert";
            this._chokeInvert.Size = new System.Drawing.Size(53, 17);
            this._chokeInvert.TabIndex = 42;
            this._chokeInvert.Text = "Invert";
            this._chokeInvert.UseVisualStyleBackColor = true;
            // 
            // _parkBrakeInvert
            // 
            this._parkBrakeInvert.AutoSize = true;
            this._parkBrakeInvert.Location = new System.Drawing.Point(277, 369);
            this._parkBrakeInvert.Name = "_parkBrakeInvert";
            this._parkBrakeInvert.Size = new System.Drawing.Size(53, 17);
            this._parkBrakeInvert.TabIndex = 43;
            this._parkBrakeInvert.Text = "Invert";
            this._parkBrakeInvert.UseVisualStyleBackColor = true;
            // 
            // _pitchInvert
            // 
            this._pitchInvert.AutoSize = true;
            this._pitchInvert.Location = new System.Drawing.Point(620, 95);
            this._pitchInvert.Name = "_pitchInvert";
            this._pitchInvert.Size = new System.Drawing.Size(53, 17);
            this._pitchInvert.TabIndex = 44;
            this._pitchInvert.Text = "Invert";
            this._pitchInvert.UseVisualStyleBackColor = true;
            // 
            // _rollInvert
            // 
            this._rollInvert.AutoSize = true;
            this._rollInvert.Location = new System.Drawing.Point(620, 167);
            this._rollInvert.Name = "_rollInvert";
            this._rollInvert.Size = new System.Drawing.Size(53, 17);
            this._rollInvert.TabIndex = 45;
            this._rollInvert.Text = "Invert";
            this._rollInvert.UseVisualStyleBackColor = true;
            // 
            // _yawInvert
            // 
            this._yawInvert.AutoSize = true;
            this._yawInvert.Location = new System.Drawing.Point(620, 236);
            this._yawInvert.Name = "_yawInvert";
            this._yawInvert.Size = new System.Drawing.Size(53, 17);
            this._yawInvert.TabIndex = 46;
            this._yawInvert.Text = "Invert";
            this._yawInvert.UseVisualStyleBackColor = true;
            // 
            // _leftBrakeInvert
            // 
            this._leftBrakeInvert.AutoSize = true;
            this._leftBrakeInvert.Location = new System.Drawing.Point(620, 307);
            this._leftBrakeInvert.Name = "_leftBrakeInvert";
            this._leftBrakeInvert.Size = new System.Drawing.Size(53, 17);
            this._leftBrakeInvert.TabIndex = 47;
            this._leftBrakeInvert.Text = "Invert";
            this._leftBrakeInvert.UseVisualStyleBackColor = true;
            // 
            // _rightBrakeInvert
            // 
            this._rightBrakeInvert.AutoSize = true;
            this._rightBrakeInvert.Location = new System.Drawing.Point(620, 369);
            this._rightBrakeInvert.Name = "_rightBrakeInvert";
            this._rightBrakeInvert.Size = new System.Drawing.Size(53, 17);
            this._rightBrakeInvert.TabIndex = 48;
            this._rightBrakeInvert.Text = "Invert";
            this._rightBrakeInvert.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(421, 140);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 49;
            this.label12.Text = "Down";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(673, 140);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Up";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(421, 215);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 13);
            this.label14.TabIndex = 51;
            this.label14.Text = "Left";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(666, 215);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 52;
            this.label15.Text = "Right";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(666, 284);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 54;
            this.label16.Text = "Right";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(421, 284);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(25, 13);
            this.label17.TabIndex = 53;
            this.label17.Text = "Left";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(666, 354);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(27, 13);
            this.label18.TabIndex = 56;
            this.label18.Text = "Max";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(421, 354);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(24, 13);
            this.label19.TabIndex = 55;
            this.label19.Text = "Min";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(666, 412);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(27, 13);
            this.label20.TabIndex = 58;
            this.label20.Text = "Max";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(421, 412);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(24, 13);
            this.label21.TabIndex = 57;
            this.label21.Text = "Min";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(326, 140);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(27, 13);
            this.label22.TabIndex = 60;
            this.label22.Text = "Max";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(81, 140);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(24, 13);
            this.label23.TabIndex = 59;
            this.label23.Text = "Min";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(326, 215);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(27, 13);
            this.label24.TabIndex = 62;
            this.label24.Text = "Max";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(81, 215);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(24, 13);
            this.label25.TabIndex = 61;
            this.label25.Text = "Min";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(326, 284);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(27, 13);
            this.label26.TabIndex = 64;
            this.label26.Text = "Max";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(81, 284);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(24, 13);
            this.label27.TabIndex = 63;
            this.label27.Text = "Min";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(326, 354);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(27, 13);
            this.label28.TabIndex = 66;
            this.label28.Text = "Max";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(81, 354);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(24, 13);
            this.label29.TabIndex = 65;
            this.label29.Text = "Min";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(326, 412);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(27, 13);
            this.label30.TabIndex = 68;
            this.label30.Text = "Max";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(81, 412);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(24, 13);
            this.label31.TabIndex = 67;
            this.label31.Text = "Min";
            // 
            // _arduinoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 441);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this._rightBrakeInvert);
            this.Controls.Add(this._leftBrakeInvert);
            this.Controls.Add(this._yawInvert);
            this.Controls.Add(this._rollInvert);
            this.Controls.Add(this._pitchInvert);
            this.Controls.Add(this._parkBrakeInvert);
            this.Controls.Add(this._chokeInvert);
            this.Controls.Add(this._carbHeatInvert);
            this.Controls.Add(this._propSpeedInvert);
            this.Controls.Add(this._throttleInvert);
            this.Controls.Add(this.label11);
            this.Controls.Add(this._bParkingBrake);
            this.Controls.Add(this._parkBrakeLevel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this._bChoke);
            this.Controls.Add(this._chokeLevel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this._bRightBrake);
            this.Controls.Add(this._rightBrakeLevel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this._bLeftBrake);
            this.Controls.Add(this._leftBrakeLevel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this._bCarbHeat);
            this.Controls.Add(this._carbHeatLevel);
            this.Controls.Add(this._newConfig);
            this.Controls.Add(this._bComClose);
            this.Controls.Add(this._statusText);
            this.Controls.Add(this._bComConnect);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._bYaw);
            this.Controls.Add(this._bRoll);
            this.Controls.Add(this._bPitch);
            this.Controls.Add(this._bPropSpeed);
            this.Controls.Add(this._bThrottle);
            this.Controls.Add(this._yawLevel);
            this.Controls.Add(this._rollLevel);
            this.Controls.Add(this._pitchLevel);
            this.Controls.Add(this._propSpeedLevel);
            this.Controls.Add(this._throttleLevel);
            this.Controls.Add(this._comPortSelection);
            this.Name = "_arduinoForm";
            this.Text = "Arduino Calibration Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _comPortSelection;
        private System.Windows.Forms.ProgressBar _throttleLevel;
        private System.Windows.Forms.ProgressBar _propSpeedLevel;
        private System.Windows.Forms.ProgressBar _pitchLevel;
        private System.Windows.Forms.ProgressBar _rollLevel;
        private System.Windows.Forms.ProgressBar _yawLevel;
        private System.Windows.Forms.Button _bThrottle;
        private System.Windows.Forms.Button _bPropSpeed;
        private System.Windows.Forms.Button _bPitch;
        private System.Windows.Forms.Button _bRoll;
        private System.Windows.Forms.Button _bYaw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button _bComConnect;
        private System.Windows.Forms.TextBox _statusText;
        private System.Windows.Forms.Button _bComClose;
        private System.IO.Ports.SerialPort _arduinoPort;
        private System.Windows.Forms.Timer _arduinoTimer;
        private System.Windows.Forms.Button _newConfig;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button _bCarbHeat;
        private System.Windows.Forms.ProgressBar _carbHeatLevel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button _bLeftBrake;
        private System.Windows.Forms.ProgressBar _leftBrakeLevel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button _bRightBrake;
        private System.Windows.Forms.ProgressBar _rightBrakeLevel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button _bChoke;
        private System.Windows.Forms.ProgressBar _chokeLevel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button _bParkingBrake;
        private System.Windows.Forms.ProgressBar _parkBrakeLevel;
        private System.Windows.Forms.CheckBox _throttleInvert;
        private System.Windows.Forms.CheckBox _propSpeedInvert;
        private System.Windows.Forms.CheckBox _carbHeatInvert;
        private System.Windows.Forms.CheckBox _chokeInvert;
        private System.Windows.Forms.CheckBox _parkBrakeInvert;
        private System.Windows.Forms.CheckBox _pitchInvert;
        private System.Windows.Forms.CheckBox _rollInvert;
        private System.Windows.Forms.CheckBox _yawInvert;
        private System.Windows.Forms.CheckBox _leftBrakeInvert;
        private System.Windows.Forms.CheckBox _rightBrakeInvert;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
    }
}

