namespace MotionPlatformControl
{
    partial class ControlGUI
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
            InputUpdate = false;
            PlatformUpdate = false;
            RecordingUpdate = false;

            System.Threading.Thread.Sleep(100);

            if (_crossbowConnection != null)
            {
                _crossbowConnection.Dispose();
                _crossbowConnection = null;
            }

            if (CrossbowMutex != null)
            {
                CrossbowMutex.Close();
                CrossbowMutex = null;
            }

            if (InputDataMutex != null)
            {
                InputDataMutex.Close();
                InputDataMutex = null;
            }

            if (MOOGMutex != null)
            {
                MOOGMutex.Close();
                MOOGMutex = null;
            }

            if (UpdateMutex != null)
            {
                UpdateMutex.Close();
                UpdateMutex = null;
            }

            if (MoogElapsedSendState != null)
            {
                MoogElapsedSendState.Reset();
            }

            if (MOOGPlatformRcvUDP != null)
            {
                MOOGPlatformRcvUDP.Close();
                MOOGPlatformRcvUDP = null;
            }

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
            this.AxText = new System.Windows.Forms.TextBox();
            this.AyText = new System.Windows.Forms.TextBox();
            this.AzText = new System.Windows.Forms.TextBox();
            this.ArollText = new System.Windows.Forms.TextBox();
            this.ApitchText = new System.Windows.Forms.TextBox();
            this.AyawText = new System.Windows.Forms.TextBox();
            this.VpitchText = new System.Windows.Forms.TextBox();
            this.VrollText = new System.Windows.Forms.TextBox();
            this.VyawText = new System.Windows.Forms.TextBox();
            this.RollText = new System.Windows.Forms.TextBox();
            this.PitchText = new System.Windows.Forms.TextBox();
            this.YawText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.EngageButton = new System.Windows.Forms.Button();
            this.ParkButton = new System.Windows.Forms.Button();
            this.StatusText = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.DisableButton = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.PYawText = new System.Windows.Forms.TextBox();
            this.PPitchText = new System.Windows.Forms.TextBox();
            this.PRollText = new System.Windows.Forms.TextBox();
            this.PHeaveText = new System.Windows.Forms.TextBox();
            this.PSurgeText = new System.Windows.Forms.TextBox();
            this.PSwayText = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.errorBar = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Discrete_7 = new System.Windows.Forms.Label();
            this.Discrete_6 = new System.Windows.Forms.Label();
            this.Discrete_5 = new System.Windows.Forms.Label();
            this.Discrete_4 = new System.Windows.Forms.Label();
            this.Discrete_3 = new System.Windows.Forms.Label();
            this.Discrete_2 = new System.Windows.Forms.Label();
            this.Discrete_1 = new System.Windows.Forms.Label();
            this.Discrete_0 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Fault_15 = new System.Windows.Forms.Label();
            this.Fault_14 = new System.Windows.Forms.Label();
            this.Fault_13 = new System.Windows.Forms.Label();
            this.Fault_12 = new System.Windows.Forms.Label();
            this.Fault_11 = new System.Windows.Forms.Label();
            this.Fault_10 = new System.Windows.Forms.Label();
            this.Fault_9 = new System.Windows.Forms.Label();
            this.Fault_8 = new System.Windows.Forms.Label();
            this.Fault_7 = new System.Windows.Forms.Label();
            this.Fault_6 = new System.Windows.Forms.Label();
            this.Fault_5 = new System.Windows.Forms.Label();
            this.Fault_4 = new System.Windows.Forms.Label();
            this.Fault_3 = new System.Windows.Forms.Label();
            this.Fault_2 = new System.Windows.Forms.Label();
            this.Fault_1 = new System.Windows.Forms.Label();
            this.Fault_0 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.RStatusState = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.InhibitButton = new System.Windows.Forms.Button();
            this.InputRecordingCheckbox = new System.Windows.Forms.CheckBox();
            this.MDAFileBox = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmb_Connection = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.CrossbowConnectButton = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.xbow_yaw = new System.Windows.Forms.TextBox();
            this.xbow_pitch = new System.Windows.Forms.TextBox();
            this.xbow_roll = new System.Windows.Forms.TextBox();
            this.xbow_rVelo = new System.Windows.Forms.TextBox();
            this.xbow_pVelo = new System.Windows.Forms.TextBox();
            this.xbow_qVelo = new System.Windows.Forms.TextBox();
            this.xbow_zAccel = new System.Windows.Forms.TextBox();
            this.xbow_yAccel = new System.Windows.Forms.TextBox();
            this.xbow_xAccel = new System.Windows.Forms.TextBox();
            this.crossbowCheck = new System.Windows.Forms.CheckBox();
            this.label28 = new System.Windows.Forms.Label();
            this.TimeOffsetText = new System.Windows.Forms.TextBox();
            this.SendDeltaTimeTextbox = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AxText
            // 
            this.AxText.BackColor = System.Drawing.Color.White;
            this.AxText.Location = new System.Drawing.Point(91, 54);
            this.AxText.Name = "AxText";
            this.AxText.ReadOnly = true;
            this.AxText.Size = new System.Drawing.Size(70, 20);
            this.AxText.TabIndex = 1;
            // 
            // AyText
            // 
            this.AyText.BackColor = System.Drawing.Color.White;
            this.AyText.Location = new System.Drawing.Point(91, 80);
            this.AyText.Name = "AyText";
            this.AyText.ReadOnly = true;
            this.AyText.Size = new System.Drawing.Size(70, 20);
            this.AyText.TabIndex = 1;
            // 
            // AzText
            // 
            this.AzText.BackColor = System.Drawing.Color.White;
            this.AzText.Location = new System.Drawing.Point(91, 106);
            this.AzText.Name = "AzText";
            this.AzText.ReadOnly = true;
            this.AzText.Size = new System.Drawing.Size(70, 20);
            this.AzText.TabIndex = 1;
            // 
            // ArollText
            // 
            this.ArollText.BackColor = System.Drawing.Color.White;
            this.ArollText.Location = new System.Drawing.Point(91, 132);
            this.ArollText.Name = "ArollText";
            this.ArollText.ReadOnly = true;
            this.ArollText.Size = new System.Drawing.Size(70, 20);
            this.ArollText.TabIndex = 1;
            // 
            // ApitchText
            // 
            this.ApitchText.BackColor = System.Drawing.Color.White;
            this.ApitchText.Location = new System.Drawing.Point(91, 158);
            this.ApitchText.Name = "ApitchText";
            this.ApitchText.ReadOnly = true;
            this.ApitchText.Size = new System.Drawing.Size(70, 20);
            this.ApitchText.TabIndex = 1;
            // 
            // AyawText
            // 
            this.AyawText.BackColor = System.Drawing.Color.White;
            this.AyawText.Location = new System.Drawing.Point(91, 184);
            this.AyawText.Name = "AyawText";
            this.AyawText.ReadOnly = true;
            this.AyawText.Size = new System.Drawing.Size(70, 20);
            this.AyawText.TabIndex = 1;
            // 
            // VpitchText
            // 
            this.VpitchText.BackColor = System.Drawing.Color.White;
            this.VpitchText.Location = new System.Drawing.Point(91, 236);
            this.VpitchText.Name = "VpitchText";
            this.VpitchText.ReadOnly = true;
            this.VpitchText.Size = new System.Drawing.Size(70, 20);
            this.VpitchText.TabIndex = 1;
            // 
            // VrollText
            // 
            this.VrollText.BackColor = System.Drawing.Color.White;
            this.VrollText.Location = new System.Drawing.Point(91, 210);
            this.VrollText.Name = "VrollText";
            this.VrollText.ReadOnly = true;
            this.VrollText.Size = new System.Drawing.Size(70, 20);
            this.VrollText.TabIndex = 1;
            // 
            // VyawText
            // 
            this.VyawText.BackColor = System.Drawing.Color.White;
            this.VyawText.Location = new System.Drawing.Point(91, 262);
            this.VyawText.Name = "VyawText";
            this.VyawText.ReadOnly = true;
            this.VyawText.Size = new System.Drawing.Size(70, 20);
            this.VyawText.TabIndex = 1;
            // 
            // RollText
            // 
            this.RollText.BackColor = System.Drawing.Color.White;
            this.RollText.Location = new System.Drawing.Point(91, 288);
            this.RollText.Name = "RollText";
            this.RollText.ReadOnly = true;
            this.RollText.Size = new System.Drawing.Size(70, 20);
            this.RollText.TabIndex = 1;
            // 
            // PitchText
            // 
            this.PitchText.BackColor = System.Drawing.Color.White;
            this.PitchText.Location = new System.Drawing.Point(91, 314);
            this.PitchText.Name = "PitchText";
            this.PitchText.ReadOnly = true;
            this.PitchText.Size = new System.Drawing.Size(70, 20);
            this.PitchText.TabIndex = 1;
            // 
            // YawText
            // 
            this.YawText.BackColor = System.Drawing.Color.White;
            this.YawText.Location = new System.Drawing.Point(91, 340);
            this.YawText.Name = "YawText";
            this.YawText.ReadOnly = true;
            this.YawText.Size = new System.Drawing.Size(70, 20);
            this.YawText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X Accel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Accel";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Z Accel";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "p Accel";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "q Accel";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "r Accel";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 213);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "p Velo";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(46, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "q Velo";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(49, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "r Velo";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(58, 291);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Roll";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(52, 317);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Pitch";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(55, 343);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Yaw";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EngageButton
            // 
            this.EngageButton.Enabled = false;
            this.EngageButton.Location = new System.Drawing.Point(179, 52);
            this.EngageButton.Name = "EngageButton";
            this.EngageButton.Size = new System.Drawing.Size(75, 23);
            this.EngageButton.TabIndex = 3;
            this.EngageButton.Text = "Engage";
            this.EngageButton.UseVisualStyleBackColor = true;
            this.EngageButton.Click += new System.EventHandler(this.EngageButton_Click);
            // 
            // ParkButton
            // 
            this.ParkButton.Enabled = false;
            this.ParkButton.Location = new System.Drawing.Point(260, 51);
            this.ParkButton.Name = "ParkButton";
            this.ParkButton.Size = new System.Drawing.Size(75, 23);
            this.ParkButton.TabIndex = 3;
            this.ParkButton.Text = "Park";
            this.ParkButton.UseVisualStyleBackColor = true;
            this.ParkButton.Click += new System.EventHandler(this.ParkButton_Click);
            // 
            // StatusText
            // 
            this.StatusText.BackColor = System.Drawing.Color.White;
            this.StatusText.Location = new System.Drawing.Point(289, 158);
            this.StatusText.Name = "StatusText";
            this.StatusText.ReadOnly = true;
            this.StatusText.Size = new System.Drawing.Size(112, 20);
            this.StatusText.TabIndex = 4;
            this.StatusText.Text = "Not Connected";
            this.StatusText.TextChanged += new System.EventHandler(this.PlatformStatusChange);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(207, 161);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Platform State:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ResetButton
            // 
            this.ResetButton.Enabled = false;
            this.ResetButton.Location = new System.Drawing.Point(260, 80);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 3;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // DisableButton
            // 
            this.DisableButton.Location = new System.Drawing.Point(341, 80);
            this.DisableButton.Name = "DisableButton";
            this.DisableButton.Size = new System.Drawing.Size(75, 23);
            this.DisableButton.TabIndex = 3;
            this.DisableButton.Text = "Disable";
            this.DisableButton.UseVisualStyleBackColor = true;
            this.DisableButton.Click += new System.EventHandler(this.DisableButton_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(242, 265);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "Heave";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(248, 239);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(33, 13);
            this.label18.TabIndex = 16;
            this.label18.Text = "Sway";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(246, 213);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 13);
            this.label19.TabIndex = 15;
            this.label19.Text = "Surge";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PYawText
            // 
            this.PYawText.BackColor = System.Drawing.Color.White;
            this.PYawText.Location = new System.Drawing.Point(289, 340);
            this.PYawText.Name = "PYawText";
            this.PYawText.ReadOnly = true;
            this.PYawText.Size = new System.Drawing.Size(70, 20);
            this.PYawText.TabIndex = 7;
            // 
            // PPitchText
            // 
            this.PPitchText.BackColor = System.Drawing.Color.White;
            this.PPitchText.Location = new System.Drawing.Point(289, 314);
            this.PPitchText.Name = "PPitchText";
            this.PPitchText.ReadOnly = true;
            this.PPitchText.Size = new System.Drawing.Size(70, 20);
            this.PPitchText.TabIndex = 6;
            // 
            // PRollText
            // 
            this.PRollText.BackColor = System.Drawing.Color.White;
            this.PRollText.Location = new System.Drawing.Point(289, 288);
            this.PRollText.Name = "PRollText";
            this.PRollText.ReadOnly = true;
            this.PRollText.Size = new System.Drawing.Size(70, 20);
            this.PRollText.TabIndex = 5;
            // 
            // PHeaveText
            // 
            this.PHeaveText.BackColor = System.Drawing.Color.White;
            this.PHeaveText.Location = new System.Drawing.Point(289, 262);
            this.PHeaveText.Name = "PHeaveText";
            this.PHeaveText.ReadOnly = true;
            this.PHeaveText.Size = new System.Drawing.Size(70, 20);
            this.PHeaveText.TabIndex = 8;
            // 
            // PSurgeText
            // 
            this.PSurgeText.BackColor = System.Drawing.Color.White;
            this.PSurgeText.Location = new System.Drawing.Point(289, 210);
            this.PSurgeText.Name = "PSurgeText";
            this.PSurgeText.ReadOnly = true;
            this.PSurgeText.Size = new System.Drawing.Size(70, 20);
            this.PSurgeText.TabIndex = 10;
            // 
            // PSwayText
            // 
            this.PSwayText.BackColor = System.Drawing.Color.White;
            this.PSwayText.Location = new System.Drawing.Point(289, 236);
            this.PSwayText.Name = "PSwayText";
            this.PSwayText.ReadOnly = true;
            this.PSwayText.Size = new System.Drawing.Size(70, 20);
            this.PSwayText.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(253, 347);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "Yaw";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(250, 321);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "Pitch";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(256, 295);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(25, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "Roll";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorBar
            // 
            this.errorBar.BackColor = System.Drawing.Color.White;
            this.errorBar.Location = new System.Drawing.Point(42, 387);
            this.errorBar.Name = "errorBar";
            this.errorBar.ReadOnly = true;
            this.errorBar.Size = new System.Drawing.Size(359, 20);
            this.errorBar.TabIndex = 4;
            this.errorBar.TextChanged += new System.EventHandler(this.PlatformStatusChange);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Discrete_7);
            this.groupBox1.Controls.Add(this.Discrete_6);
            this.groupBox1.Controls.Add(this.Discrete_5);
            this.groupBox1.Controls.Add(this.Discrete_4);
            this.groupBox1.Controls.Add(this.Discrete_3);
            this.groupBox1.Controls.Add(this.Discrete_2);
            this.groupBox1.Controls.Add(this.Discrete_1);
            this.groupBox1.Controls.Add(this.Discrete_0);
            this.groupBox1.Location = new System.Drawing.Point(28, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(132, 126);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Discrete I/O";
            // 
            // Discrete_7
            // 
            this.Discrete_7.AutoSize = true;
            this.Discrete_7.Enabled = false;
            this.Discrete_7.Location = new System.Drawing.Point(6, 107);
            this.Discrete_7.Name = "Discrete_7";
            this.Discrete_7.Size = new System.Drawing.Size(69, 13);
            this.Discrete_7.TabIndex = 7;
            this.Discrete_7.Text = "EStop Sense";
            // 
            // Discrete_6
            // 
            this.Discrete_6.AutoSize = true;
            this.Discrete_6.Enabled = false;
            this.Discrete_6.Location = new System.Drawing.Point(6, 94);
            this.Discrete_6.Name = "Discrete_6";
            this.Discrete_6.Size = new System.Drawing.Size(100, 13);
            this.Discrete_6.TabIndex = 6;
            this.Discrete_6.Text = "Amplifier Enbl Comd";
            // 
            // Discrete_5
            // 
            this.Discrete_5.AutoSize = true;
            this.Discrete_5.Enabled = false;
            this.Discrete_5.Location = new System.Drawing.Point(6, 81);
            this.Discrete_5.Name = "Discrete_5";
            this.Discrete_5.Size = new System.Drawing.Size(86, 13);
            this.Discrete_5.TabIndex = 5;
            this.Discrete_5.Text = "Drive Bus Sense";
            // 
            // Discrete_4
            // 
            this.Discrete_4.AutoSize = true;
            this.Discrete_4.Enabled = false;
            this.Discrete_4.Location = new System.Drawing.Point(6, 68);
            this.Discrete_4.Name = "Discrete_4";
            this.Discrete_4.Size = new System.Drawing.Size(109, 13);
            this.Discrete_4.TabIndex = 4;
            this.Discrete_4.Text = "Limit Shunt Command";
            // 
            // Discrete_3
            // 
            this.Discrete_3.AutoSize = true;
            this.Discrete_3.Enabled = false;
            this.Discrete_3.Location = new System.Drawing.Point(6, 55);
            this.Discrete_3.Name = "Discrete_3";
            this.Discrete_3.Size = new System.Drawing.Size(96, 13);
            this.Discrete_3.TabIndex = 3;
            this.Discrete_3.Text = "Limit Switch Sense";
            // 
            // Discrete_2
            // 
            this.Discrete_2.AutoSize = true;
            this.Discrete_2.Enabled = false;
            this.Discrete_2.Location = new System.Drawing.Point(6, 42);
            this.Discrete_2.Name = "Discrete_2";
            this.Discrete_2.Size = new System.Drawing.Size(105, 13);
            this.Discrete_2.TabIndex = 2;
            this.Discrete_2.Text = "Amplifier Fault Sense";
            // 
            // Discrete_1
            // 
            this.Discrete_1.AutoSize = true;
            this.Discrete_1.Enabled = false;
            this.Discrete_1.Location = new System.Drawing.Point(6, 29);
            this.Discrete_1.Name = "Discrete_1";
            this.Discrete_1.Size = new System.Drawing.Size(104, 13);
            this.Discrete_1.TabIndex = 1;
            this.Discrete_1.Text = "Thermal Fault Sense";
            // 
            // Discrete_0
            // 
            this.Discrete_0.AutoSize = true;
            this.Discrete_0.Enabled = false;
            this.Discrete_0.Location = new System.Drawing.Point(6, 16);
            this.Discrete_0.Name = "Discrete_0";
            this.Discrete_0.Size = new System.Drawing.Size(74, 13);
            this.Discrete_0.TabIndex = 0;
            this.Discrete_0.Text = "Base at Home";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Fault_15);
            this.groupBox2.Controls.Add(this.Fault_14);
            this.groupBox2.Controls.Add(this.Fault_13);
            this.groupBox2.Controls.Add(this.Fault_12);
            this.groupBox2.Controls.Add(this.Fault_11);
            this.groupBox2.Controls.Add(this.Fault_10);
            this.groupBox2.Controls.Add(this.Fault_9);
            this.groupBox2.Controls.Add(this.Fault_8);
            this.groupBox2.Controls.Add(this.Fault_7);
            this.groupBox2.Controls.Add(this.Fault_6);
            this.groupBox2.Controls.Add(this.Fault_5);
            this.groupBox2.Controls.Add(this.Fault_4);
            this.groupBox2.Controls.Add(this.Fault_3);
            this.groupBox2.Controls.Add(this.Fault_2);
            this.groupBox2.Controls.Add(this.Fault_1);
            this.groupBox2.Controls.Add(this.Fault_0);
            this.groupBox2.Location = new System.Drawing.Point(28, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(132, 230);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Faults";
            // 
            // Fault_15
            // 
            this.Fault_15.AutoSize = true;
            this.Fault_15.Enabled = false;
            this.Fault_15.Location = new System.Drawing.Point(6, 210);
            this.Fault_15.Name = "Fault_15";
            this.Fault_15.Size = new System.Drawing.Size(36, 13);
            this.Fault_15.TabIndex = 7;
            this.Fault_15.Text = "EStop";
            // 
            // Fault_14
            // 
            this.Fault_14.AutoSize = true;
            this.Fault_14.Enabled = false;
            this.Fault_14.Location = new System.Drawing.Point(5, 197);
            this.Fault_14.Name = "Fault_14";
            this.Fault_14.Size = new System.Drawing.Size(47, 13);
            this.Fault_14.TabIndex = 7;
            this.Fault_14.Text = "Snubber";
            // 
            // Fault_13
            // 
            this.Fault_13.AutoSize = true;
            this.Fault_13.Enabled = false;
            this.Fault_13.Location = new System.Drawing.Point(5, 184);
            this.Fault_13.Name = "Fault_13";
            this.Fault_13.Size = new System.Drawing.Size(95, 13);
            this.Fault_13.TabIndex = 7;
            this.Fault_13.Text = "Actuator Runaway";
            // 
            // Fault_12
            // 
            this.Fault_12.AutoSize = true;
            this.Fault_12.Enabled = false;
            this.Fault_12.Location = new System.Drawing.Point(5, 171);
            this.Fault_12.Name = "Fault_12";
            this.Fault_12.Size = new System.Drawing.Size(40, 13);
            this.Fault_12.TabIndex = 7;
            this.Fault_12.Text = "Battery";
            // 
            // Fault_11
            // 
            this.Fault_11.AutoSize = true;
            this.Fault_11.Enabled = false;
            this.Fault_11.Location = new System.Drawing.Point(5, 158);
            this.Fault_11.Name = "Fault_11";
            this.Fault_11.Size = new System.Drawing.Size(73, 13);
            this.Fault_11.TabIndex = 7;
            this.Fault_11.Text = "Low Idle Rate";
            // 
            // Fault_10
            // 
            this.Fault_10.AutoSize = true;
            this.Fault_10.Enabled = false;
            this.Fault_10.Location = new System.Drawing.Point(5, 145);
            this.Fault_10.Name = "Fault_10";
            this.Fault_10.Size = new System.Drawing.Size(75, 13);
            this.Fault_10.TabIndex = 7;
            this.Fault_10.Text = "Motor Thermal";
            // 
            // Fault_9
            // 
            this.Fault_9.AutoSize = true;
            this.Fault_9.Enabled = false;
            this.Fault_9.Location = new System.Drawing.Point(5, 132);
            this.Fault_9.Name = "Fault_9";
            this.Fault_9.Size = new System.Drawing.Size(89, 13);
            this.Fault_9.TabIndex = 7;
            this.Fault_9.Text = "Command Range";
            // 
            // Fault_8
            // 
            this.Fault_8.AutoSize = true;
            this.Fault_8.Enabled = false;
            this.Fault_8.Location = new System.Drawing.Point(6, 119);
            this.Fault_8.Name = "Fault_8";
            this.Fault_8.Size = new System.Drawing.Size(70, 13);
            this.Fault_8.TabIndex = 7;
            this.Fault_8.Text = "Invalid Frame";
            // 
            // Fault_7
            // 
            this.Fault_7.AutoSize = true;
            this.Fault_7.Enabled = false;
            this.Fault_7.Location = new System.Drawing.Point(6, 106);
            this.Fault_7.Name = "Fault_7";
            this.Fault_7.Size = new System.Drawing.Size(57, 13);
            this.Fault_7.TabIndex = 7;
            this.Fault_7.Text = "Watchdog";
            // 
            // Fault_6
            // 
            this.Fault_6.AutoSize = true;
            this.Fault_6.Enabled = false;
            this.Fault_6.Location = new System.Drawing.Point(6, 93);
            this.Fault_6.Name = "Fault_6";
            this.Fault_6.Size = new System.Drawing.Size(63, 13);
            this.Fault_6.TabIndex = 6;
            this.Fault_6.Text = "Limit Switch";
            // 
            // Fault_5
            // 
            this.Fault_5.AutoSize = true;
            this.Fault_5.Enabled = false;
            this.Fault_5.Location = new System.Drawing.Point(6, 80);
            this.Fault_5.Name = "Fault_5";
            this.Fault_5.Size = new System.Drawing.Size(56, 13);
            this.Fault_5.TabIndex = 5;
            this.Fault_5.Text = "Drive Bus ";
            // 
            // Fault_4
            // 
            this.Fault_4.AutoSize = true;
            this.Fault_4.Enabled = false;
            this.Fault_4.Location = new System.Drawing.Point(6, 67);
            this.Fault_4.Name = "Fault_4";
            this.Fault_4.Size = new System.Drawing.Size(46, 13);
            this.Fault_4.TabIndex = 4;
            this.Fault_4.Text = "Amplifier";
            // 
            // Fault_3
            // 
            this.Fault_3.AutoSize = true;
            this.Fault_3.Enabled = false;
            this.Fault_3.Location = new System.Drawing.Point(6, 54);
            this.Fault_3.Name = "Fault_3";
            this.Fault_3.Size = new System.Drawing.Size(70, 13);
            this.Fault_3.TabIndex = 3;
            this.Fault_3.Text = "Comm Failure";
            // 
            // Fault_2
            // 
            this.Fault_2.AutoSize = true;
            this.Fault_2.Enabled = false;
            this.Fault_2.Location = new System.Drawing.Point(6, 41);
            this.Fault_2.Name = "Fault_2";
            this.Fault_2.Size = new System.Drawing.Size(43, 13);
            this.Fault_2.TabIndex = 2;
            this.Fault_2.Text = "Homing";
            // 
            // Fault_1
            // 
            this.Fault_1.AutoSize = true;
            this.Fault_1.Enabled = false;
            this.Fault_1.Location = new System.Drawing.Point(6, 28);
            this.Fault_1.Name = "Fault_1";
            this.Fault_1.Size = new System.Drawing.Size(52, 13);
            this.Fault_1.TabIndex = 1;
            this.Fault_1.Text = "Envelope";
            // 
            // Fault_0
            // 
            this.Fault_0.AutoSize = true;
            this.Fault_0.Enabled = false;
            this.Fault_0.Location = new System.Drawing.Point(6, 15);
            this.Fault_0.Name = "Fault_0";
            this.Fault_0.Size = new System.Drawing.Size(79, 13);
            this.Fault_0.TabIndex = 0;
            this.Fault_0.Text = "Torque Monitor";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(193, 187);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(90, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "Requested State:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RStatusState
            // 
            this.RStatusState.BackColor = System.Drawing.Color.White;
            this.RStatusState.Location = new System.Drawing.Point(289, 184);
            this.RStatusState.Name = "RStatusState";
            this.RStatusState.ReadOnly = true;
            this.RStatusState.Size = new System.Drawing.Size(112, 20);
            this.RStatusState.TabIndex = 4;
            this.RStatusState.Text = "Not Connected";
            this.RStatusState.TextChanged += new System.EventHandler(this.PlatformStatusChange);
            // 
            // RunButton
            // 
            this.RunButton.Enabled = false;
            this.RunButton.Location = new System.Drawing.Point(179, 80);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 3;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // InhibitButton
            // 
            this.InhibitButton.Location = new System.Drawing.Point(341, 51);
            this.InhibitButton.Name = "InhibitButton";
            this.InhibitButton.Size = new System.Drawing.Size(75, 23);
            this.InhibitButton.TabIndex = 3;
            this.InhibitButton.Text = "Inhibit";
            this.InhibitButton.UseVisualStyleBackColor = true;
            this.InhibitButton.Click += new System.EventHandler(this.InhibitButton_Click);
            // 
            // InputRecordingCheckbox
            // 
            this.InputRecordingCheckbox.AutoSize = true;
            this.InputRecordingCheckbox.Location = new System.Drawing.Point(314, 4);
            this.InputRecordingCheckbox.Name = "InputRecordingCheckbox";
            this.InputRecordingCheckbox.Size = new System.Drawing.Size(88, 17);
            this.InputRecordingCheckbox.TabIndex = 24;
            this.InputRecordingCheckbox.Text = "Record Input";
            this.InputRecordingCheckbox.UseVisualStyleBackColor = true;
            this.InputRecordingCheckbox.CheckedChanged += new System.EventHandler(this.InputRecording_CheckedChanged);
            // 
            // MDAFileBox
            // 
            this.MDAFileBox.FormattingEnabled = true;
            this.MDAFileBox.Items.AddRange(new object[] {
            "101 - Original",
            "102 - Overtilt",
            "103",
            "104",
            "105",
            "106"});
            this.MDAFileBox.Location = new System.Drawing.Point(289, 130);
            this.MDAFileBox.Name = "MDAFileBox";
            this.MDAFileBox.Size = new System.Drawing.Size(95, 21);
            this.MDAFileBox.TabIndex = 25;
            this.MDAFileBox.Text = "102 - Overtilt";
            this.MDAFileBox.SelectedIndexChanged += new System.EventHandler(this.MDAFileBox_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(228, 135);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 13);
            this.label21.TabIndex = 2;
            this.label21.Text = "MDA File:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Location = new System.Drawing.Point(422, -2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cmb_Connection);
            this.splitContainer1.Panel2.Controls.Add(this.label34);
            this.splitContainer1.Panel2.Controls.Add(this.CrossbowConnectButton);
            this.splitContainer1.Panel2.Controls.Add(this.label22);
            this.splitContainer1.Panel2.Controls.Add(this.label23);
            this.splitContainer1.Panel2.Controls.Add(this.label24);
            this.splitContainer1.Panel2.Controls.Add(this.label25);
            this.splitContainer1.Panel2.Controls.Add(this.label26);
            this.splitContainer1.Panel2.Controls.Add(this.label27);
            this.splitContainer1.Panel2.Controls.Add(this.label31);
            this.splitContainer1.Panel2.Controls.Add(this.label32);
            this.splitContainer1.Panel2.Controls.Add(this.label33);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_yaw);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_pitch);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_roll);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_rVelo);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_pVelo);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_qVelo);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_zAccel);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_yAccel);
            this.splitContainer1.Panel2.Controls.Add(this.xbow_xAccel);
            this.splitContainer1.Size = new System.Drawing.Size(197, 441);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 26;
            // 
            // cmb_Connection
            // 
            this.cmb_Connection.FormattingEnabled = true;
            this.cmb_Connection.Location = new System.Drawing.Point(49, 346);
            this.cmb_Connection.Name = "cmb_Connection";
            this.cmb_Connection.Size = new System.Drawing.Size(86, 21);
            this.cmb_Connection.TabIndex = 29;
            this.cmb_Connection.SelectedIndexChanged += new System.EventHandler(this.cmb_Connection_SelectedIndexChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(53, 14);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(69, 17);
            this.label34.TabIndex = 28;
            this.label34.Text = "Crossbow";
            // 
            // CrossbowConnectButton
            // 
            this.CrossbowConnectButton.Enabled = false;
            this.CrossbowConnectButton.Location = new System.Drawing.Point(56, 382);
            this.CrossbowConnectButton.Name = "CrossbowConnectButton";
            this.CrossbowConnectButton.Size = new System.Drawing.Size(75, 23);
            this.CrossbowConnectButton.TabIndex = 27;
            this.CrossbowConnectButton.Text = "Connect";
            this.CrossbowConnectButton.UseVisualStyleBackColor = true;
            this.CrossbowConnectButton.Click += new System.EventHandler(this.CrossbowConnect_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(43, 268);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(28, 13);
            this.label22.TabIndex = 15;
            this.label22.Text = "Yaw";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(40, 242);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(31, 13);
            this.label23.TabIndex = 19;
            this.label23.Text = "Pitch";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(46, 216);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(25, 13);
            this.label24.TabIndex = 20;
            this.label24.Text = "Roll";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(37, 190);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(34, 13);
            this.label25.TabIndex = 18;
            this.label25.Text = "r Velo";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(34, 164);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(37, 13);
            this.label26.TabIndex = 16;
            this.label26.Text = "q Velo";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(34, 138);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(37, 13);
            this.label27.TabIndex = 17;
            this.label27.Text = "p Velo";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(27, 110);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(44, 13);
            this.label31.TabIndex = 24;
            this.label31.Text = "Z Accel";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(27, 84);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(44, 13);
            this.label32.TabIndex = 22;
            this.label32.Text = "Y Accel";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(27, 59);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(44, 13);
            this.label33.TabIndex = 23;
            this.label33.Text = "X Accel";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // xbow_yaw
            // 
            this.xbow_yaw.BackColor = System.Drawing.Color.White;
            this.xbow_yaw.Location = new System.Drawing.Point(79, 265);
            this.xbow_yaw.Name = "xbow_yaw";
            this.xbow_yaw.ReadOnly = true;
            this.xbow_yaw.Size = new System.Drawing.Size(70, 20);
            this.xbow_yaw.TabIndex = 6;
            // 
            // xbow_pitch
            // 
            this.xbow_pitch.BackColor = System.Drawing.Color.White;
            this.xbow_pitch.Location = new System.Drawing.Point(79, 239);
            this.xbow_pitch.Name = "xbow_pitch";
            this.xbow_pitch.ReadOnly = true;
            this.xbow_pitch.Size = new System.Drawing.Size(70, 20);
            this.xbow_pitch.TabIndex = 7;
            // 
            // xbow_roll
            // 
            this.xbow_roll.BackColor = System.Drawing.Color.White;
            this.xbow_roll.Location = new System.Drawing.Point(79, 213);
            this.xbow_roll.Name = "xbow_roll";
            this.xbow_roll.ReadOnly = true;
            this.xbow_roll.Size = new System.Drawing.Size(70, 20);
            this.xbow_roll.TabIndex = 8;
            // 
            // xbow_rVelo
            // 
            this.xbow_rVelo.BackColor = System.Drawing.Color.White;
            this.xbow_rVelo.Location = new System.Drawing.Point(79, 187);
            this.xbow_rVelo.Name = "xbow_rVelo";
            this.xbow_rVelo.ReadOnly = true;
            this.xbow_rVelo.Size = new System.Drawing.Size(70, 20);
            this.xbow_rVelo.TabIndex = 3;
            // 
            // xbow_pVelo
            // 
            this.xbow_pVelo.BackColor = System.Drawing.Color.White;
            this.xbow_pVelo.Location = new System.Drawing.Point(79, 135);
            this.xbow_pVelo.Name = "xbow_pVelo";
            this.xbow_pVelo.ReadOnly = true;
            this.xbow_pVelo.Size = new System.Drawing.Size(70, 20);
            this.xbow_pVelo.TabIndex = 4;
            // 
            // xbow_qVelo
            // 
            this.xbow_qVelo.BackColor = System.Drawing.Color.White;
            this.xbow_qVelo.Location = new System.Drawing.Point(79, 161);
            this.xbow_qVelo.Name = "xbow_qVelo";
            this.xbow_qVelo.ReadOnly = true;
            this.xbow_qVelo.Size = new System.Drawing.Size(70, 20);
            this.xbow_qVelo.TabIndex = 5;
            // 
            // xbow_zAccel
            // 
            this.xbow_zAccel.BackColor = System.Drawing.Color.White;
            this.xbow_zAccel.Location = new System.Drawing.Point(79, 107);
            this.xbow_zAccel.Name = "xbow_zAccel";
            this.xbow_zAccel.ReadOnly = true;
            this.xbow_zAccel.Size = new System.Drawing.Size(70, 20);
            this.xbow_zAccel.TabIndex = 12;
            // 
            // xbow_yAccel
            // 
            this.xbow_yAccel.BackColor = System.Drawing.Color.White;
            this.xbow_yAccel.Location = new System.Drawing.Point(79, 81);
            this.xbow_yAccel.Name = "xbow_yAccel";
            this.xbow_yAccel.ReadOnly = true;
            this.xbow_yAccel.Size = new System.Drawing.Size(70, 20);
            this.xbow_yAccel.TabIndex = 10;
            // 
            // xbow_xAccel
            // 
            this.xbow_xAccel.BackColor = System.Drawing.Color.White;
            this.xbow_xAccel.Location = new System.Drawing.Point(79, 55);
            this.xbow_xAccel.Name = "xbow_xAccel";
            this.xbow_xAccel.ReadOnly = true;
            this.xbow_xAccel.Size = new System.Drawing.Size(70, 20);
            this.xbow_xAccel.TabIndex = 11;
            // 
            // crossbowCheck
            // 
            this.crossbowCheck.AutoSize = true;
            this.crossbowCheck.Location = new System.Drawing.Point(314, 27);
            this.crossbowCheck.Name = "crossbowCheck";
            this.crossbowCheck.Size = new System.Drawing.Size(102, 17);
            this.crossbowCheck.TabIndex = 27;
            this.crossbowCheck.Text = "Show Crossbow";
            this.crossbowCheck.UseVisualStyleBackColor = true;
            this.crossbowCheck.CheckedChanged += new System.EventHandler(this.crossbow_show_panel);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(24, 28);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(59, 13);
            this.label28.TabIndex = 29;
            this.label28.Text = "Time offset";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TimeOffsetText
            // 
            this.TimeOffsetText.BackColor = System.Drawing.Color.White;
            this.TimeOffsetText.Location = new System.Drawing.Point(91, 25);
            this.TimeOffsetText.Name = "TimeOffsetText";
            this.TimeOffsetText.ReadOnly = true;
            this.TimeOffsetText.Size = new System.Drawing.Size(70, 20);
            this.TimeOffsetText.TabIndex = 28;
            // 
            // SendDeltaTimeTextbox
            // 
            this.SendDeltaTimeTextbox.BackColor = System.Drawing.Color.White;
            this.SendDeltaTimeTextbox.Location = new System.Drawing.Point(238, 25);
            this.SendDeltaTimeTextbox.Name = "SendDeltaTimeTextbox";
            this.SendDeltaTimeTextbox.ReadOnly = true;
            this.SendDeltaTimeTextbox.Size = new System.Drawing.Size(70, 20);
            this.SendDeltaTimeTextbox.TabIndex = 30;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(173, 28);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(59, 13);
            this.label29.TabIndex = 31;
            this.label29.Text = "Time offset";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ControlGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 436);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.SendDeltaTimeTextbox);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.TimeOffsetText);
            this.Controls.Add(this.crossbowCheck);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.InputRecordingCheckbox);
            this.Controls.Add(this.MDAFileBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.PYawText);
            this.Controls.Add(this.PPitchText);
            this.Controls.Add(this.PRollText);
            this.Controls.Add(this.PHeaveText);
            this.Controls.Add(this.PSurgeText);
            this.Controls.Add(this.PSwayText);
            this.Controls.Add(this.errorBar);
            this.Controls.Add(this.RStatusState);
            this.Controls.Add(this.StatusText);
            this.Controls.Add(this.InhibitButton);
            this.Controls.Add(this.DisableButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.ParkButton);
            this.Controls.Add(this.EngageButton);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.YawText);
            this.Controls.Add(this.PitchText);
            this.Controls.Add(this.RollText);
            this.Controls.Add(this.VyawText);
            this.Controls.Add(this.VrollText);
            this.Controls.Add(this.VpitchText);
            this.Controls.Add(this.AyawText);
            this.Controls.Add(this.ApitchText);
            this.Controls.Add(this.ArollText);
            this.Controls.Add(this.AzText);
            this.Controls.Add(this.AyText);
            this.Controls.Add(this.AxText);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControlGUI";
            this.Text = "MOOG Platform MDA Control";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AxText;
        private System.Windows.Forms.TextBox AyText;
        private System.Windows.Forms.TextBox AzText;
        private System.Windows.Forms.TextBox ArollText;
        private System.Windows.Forms.TextBox ApitchText;
        private System.Windows.Forms.TextBox AyawText;
        private System.Windows.Forms.TextBox VpitchText;
        private System.Windows.Forms.TextBox VrollText;
        private System.Windows.Forms.TextBox VyawText;
        private System.Windows.Forms.TextBox RollText;
        private System.Windows.Forms.TextBox PitchText;
        private System.Windows.Forms.TextBox YawText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button EngageButton;
        private System.Windows.Forms.Button ParkButton;
        private System.Windows.Forms.TextBox StatusText;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button DisableButton;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox PYawText;
        private System.Windows.Forms.TextBox PPitchText;
        private System.Windows.Forms.TextBox PRollText;
        private System.Windows.Forms.TextBox PHeaveText;
        private System.Windows.Forms.TextBox PSurgeText;
        private System.Windows.Forms.TextBox PSwayText;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox errorBar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Discrete_0;
        private System.Windows.Forms.Label Discrete_3;
        private System.Windows.Forms.Label Discrete_2;
        private System.Windows.Forms.Label Discrete_1;
        private System.Windows.Forms.Label Discrete_7;
        private System.Windows.Forms.Label Discrete_6;
        private System.Windows.Forms.Label Discrete_5;
        private System.Windows.Forms.Label Discrete_4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label Fault_7;
        private System.Windows.Forms.Label Fault_6;
        private System.Windows.Forms.Label Fault_5;
        private System.Windows.Forms.Label Fault_4;
        private System.Windows.Forms.Label Fault_3;
        private System.Windows.Forms.Label Fault_2;
        private System.Windows.Forms.Label Fault_1;
        private System.Windows.Forms.Label Fault_0;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox RStatusState;
        private System.Windows.Forms.Label Fault_14;
        private System.Windows.Forms.Label Fault_13;
        private System.Windows.Forms.Label Fault_12;
        private System.Windows.Forms.Label Fault_11;
        private System.Windows.Forms.Label Fault_10;
        private System.Windows.Forms.Label Fault_9;
        private System.Windows.Forms.Label Fault_8;
        private System.Windows.Forms.Label Fault_15;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button InhibitButton;
        private System.Windows.Forms.CheckBox InputRecordingCheckbox;
        private System.Windows.Forms.ComboBox MDAFileBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button CrossbowConnectButton;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox xbow_yaw;
        private System.Windows.Forms.TextBox xbow_pitch;
        private System.Windows.Forms.TextBox xbow_roll;
        private System.Windows.Forms.TextBox xbow_rVelo;
        private System.Windows.Forms.TextBox xbow_pVelo;
        private System.Windows.Forms.TextBox xbow_qVelo;
        private System.Windows.Forms.TextBox xbow_zAccel;
        private System.Windows.Forms.TextBox xbow_yAccel;
        private System.Windows.Forms.TextBox xbow_xAccel;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.CheckBox crossbowCheck;
        private System.Windows.Forms.ComboBox cmb_Connection;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox TimeOffsetText;
        private System.Windows.Forms.TextBox SendDeltaTimeTextbox;
        private System.Windows.Forms.Label label29;
    }
}

