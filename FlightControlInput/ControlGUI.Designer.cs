namespace FlightControlInput
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
            ControlInputUpdate = false;
            MotionInputUpdate = false;
            RecordingUpdate = false;
            DisplayUpdate = false;

            System.Threading.Thread.Sleep(100);

            if (InputDataMutex != null)
            {
                InputDataMutex.Close();
                InputDataMutex = null;
            }

            
            if (UpdateMutex != null)
            {
                UpdateMutex.Close();
                UpdateMutex = null;
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
            this.components = new System.ComponentModel.Container();
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
            this.errorBar = new System.Windows.Forms.TextBox();
            this.InputRecordingCheckbox = new System.Windows.Forms.CheckBox();
            this.label28 = new System.Windows.Forms.Label();
            this.TimeOffsetText = new System.Windows.Forms.TextBox();
            this.SendDeltaTimeTextbox = new System.Windows.Forms.TextBox();
            this.ConnectInputUDPButton = new System.Windows.Forms.Button();
            this.CreateInputDataButton = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.InputUDPPortBox = new System.Windows.Forms.TextBox();
            this.RollFreqBox = new System.Windows.Forms.TextBox();
            this.PitchFreqBox = new System.Windows.Forms.TextBox();
            this.YawFreqBox = new System.Windows.Forms.TextBox();
            this.YawAmpBox = new System.Windows.Forms.TextBox();
            this.PitchAmpBox = new System.Windows.Forms.TextBox();
            this.RollAmpBox = new System.Windows.Forms.TextBox();
            this.RollCheckBox = new System.Windows.Forms.CheckBox();
            this.PitchCheckBox = new System.Windows.Forms.CheckBox();
            this.YawCheckBox = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ControllerInitButton = new System.Windows.Forms.Button();
            this.PitchDownButton = new System.Windows.Forms.Button();
            this.PitchUpButton = new System.Windows.Forms.Button();
            this.RollLeftButton = new System.Windows.Forms.Button();
            this.RollRightButton = new System.Windows.Forms.Button();
            this.PitchRatioTextBox = new System.Windows.Forms.TextBox();
            this.RollRatioTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.YawRatioTextBox = new System.Windows.Forms.TextBox();
            this.YawRightButton = new System.Windows.Forms.Button();
            this.YawLeftButton = new System.Windows.Forms.Button();
            this.rollTimer = new System.Windows.Forms.Timer(this.components);
            this.pitchTimer = new System.Windows.Forms.Timer(this.components);
            this.yawTimer = new System.Windows.Forms.Timer(this.components);
            this.SinusoidalPanel = new System.Windows.Forms.Panel();
            this.ControllerPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OpenFile = new System.Windows.Forms.Button();
            this.openInputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.controllerTimer = new System.Windows.Forms.Timer(this.components);
            this.recalibrateButton = new System.Windows.Forms.Button();
            this.SinusoidalPanel.SuspendLayout();
            this.ControllerPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AxText
            // 
            this.AxText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AxText.BackColor = System.Drawing.Color.White;
            this.AxText.Location = new System.Drawing.Point(438, 69);
            this.AxText.Name = "AxText";
            this.AxText.ReadOnly = true;
            this.AxText.Size = new System.Drawing.Size(70, 20);
            this.AxText.TabIndex = 1;
            // 
            // AyText
            // 
            this.AyText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AyText.BackColor = System.Drawing.Color.White;
            this.AyText.Location = new System.Drawing.Point(438, 95);
            this.AyText.Name = "AyText";
            this.AyText.ReadOnly = true;
            this.AyText.Size = new System.Drawing.Size(70, 20);
            this.AyText.TabIndex = 1;
            // 
            // AzText
            // 
            this.AzText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AzText.BackColor = System.Drawing.Color.White;
            this.AzText.Location = new System.Drawing.Point(438, 121);
            this.AzText.Name = "AzText";
            this.AzText.ReadOnly = true;
            this.AzText.Size = new System.Drawing.Size(70, 20);
            this.AzText.TabIndex = 1;
            // 
            // ArollText
            // 
            this.ArollText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ArollText.BackColor = System.Drawing.Color.White;
            this.ArollText.Location = new System.Drawing.Point(438, 147);
            this.ArollText.Name = "ArollText";
            this.ArollText.ReadOnly = true;
            this.ArollText.Size = new System.Drawing.Size(70, 20);
            this.ArollText.TabIndex = 1;
            // 
            // ApitchText
            // 
            this.ApitchText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ApitchText.BackColor = System.Drawing.Color.White;
            this.ApitchText.Location = new System.Drawing.Point(438, 173);
            this.ApitchText.Name = "ApitchText";
            this.ApitchText.ReadOnly = true;
            this.ApitchText.Size = new System.Drawing.Size(70, 20);
            this.ApitchText.TabIndex = 1;
            // 
            // AyawText
            // 
            this.AyawText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AyawText.BackColor = System.Drawing.Color.White;
            this.AyawText.Location = new System.Drawing.Point(438, 199);
            this.AyawText.Name = "AyawText";
            this.AyawText.ReadOnly = true;
            this.AyawText.Size = new System.Drawing.Size(70, 20);
            this.AyawText.TabIndex = 1;
            // 
            // VpitchText
            // 
            this.VpitchText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VpitchText.BackColor = System.Drawing.Color.White;
            this.VpitchText.Location = new System.Drawing.Point(438, 251);
            this.VpitchText.Name = "VpitchText";
            this.VpitchText.ReadOnly = true;
            this.VpitchText.Size = new System.Drawing.Size(70, 20);
            this.VpitchText.TabIndex = 1;
            // 
            // VrollText
            // 
            this.VrollText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VrollText.BackColor = System.Drawing.Color.White;
            this.VrollText.Location = new System.Drawing.Point(438, 225);
            this.VrollText.Name = "VrollText";
            this.VrollText.ReadOnly = true;
            this.VrollText.Size = new System.Drawing.Size(70, 20);
            this.VrollText.TabIndex = 1;
            // 
            // VyawText
            // 
            this.VyawText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VyawText.BackColor = System.Drawing.Color.White;
            this.VyawText.Location = new System.Drawing.Point(438, 277);
            this.VyawText.Name = "VyawText";
            this.VyawText.ReadOnly = true;
            this.VyawText.Size = new System.Drawing.Size(70, 20);
            this.VyawText.TabIndex = 1;
            // 
            // RollText
            // 
            this.RollText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RollText.BackColor = System.Drawing.Color.White;
            this.RollText.Location = new System.Drawing.Point(438, 303);
            this.RollText.Name = "RollText";
            this.RollText.ReadOnly = true;
            this.RollText.Size = new System.Drawing.Size(70, 20);
            this.RollText.TabIndex = 1;
            // 
            // PitchText
            // 
            this.PitchText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PitchText.BackColor = System.Drawing.Color.White;
            this.PitchText.Location = new System.Drawing.Point(438, 329);
            this.PitchText.Name = "PitchText";
            this.PitchText.ReadOnly = true;
            this.PitchText.Size = new System.Drawing.Size(70, 20);
            this.PitchText.TabIndex = 1;
            // 
            // YawText
            // 
            this.YawText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.YawText.BackColor = System.Drawing.Color.White;
            this.YawText.Location = new System.Drawing.Point(438, 355);
            this.YawText.Name = "YawText";
            this.YawText.ReadOnly = true;
            this.YawText.Size = new System.Drawing.Size(70, 20);
            this.YawText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(386, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X Accel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(386, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Accel";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(386, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Z Accel";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(387, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "p Accel";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(387, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "q Accel";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(390, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "r Accel";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(393, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "p Velo";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(393, 254);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "q Velo";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(396, 280);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "r Velo";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(405, 306);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Roll";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(399, 332);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Pitch";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(402, 358);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Yaw";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorBar
            // 
            this.errorBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorBar.BackColor = System.Drawing.Color.White;
            this.errorBar.Location = new System.Drawing.Point(43, 355);
            this.errorBar.Name = "errorBar";
            this.errorBar.ReadOnly = true;
            this.errorBar.Size = new System.Drawing.Size(300, 20);
            this.errorBar.TabIndex = 4;
            // 
            // InputRecordingCheckbox
            // 
            this.InputRecordingCheckbox.AutoSize = true;
            this.InputRecordingCheckbox.Location = new System.Drawing.Point(105, 325);
            this.InputRecordingCheckbox.Name = "InputRecordingCheckbox";
            this.InputRecordingCheckbox.Size = new System.Drawing.Size(87, 17);
            this.InputRecordingCheckbox.TabIndex = 24;
            this.InputRecordingCheckbox.Text = "Record Data";
            this.InputRecordingCheckbox.UseVisualStyleBackColor = true;
            this.InputRecordingCheckbox.CheckedChanged += new System.EventHandler(this.InputRecording_CheckedChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(88, 13);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(59, 13);
            this.label28.TabIndex = 29;
            this.label28.Text = "Time offset";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label28.Visible = false;
            // 
            // TimeOffsetText
            // 
            this.TimeOffsetText.BackColor = System.Drawing.Color.White;
            this.TimeOffsetText.Location = new System.Drawing.Point(91, 39);
            this.TimeOffsetText.Name = "TimeOffsetText";
            this.TimeOffsetText.ReadOnly = true;
            this.TimeOffsetText.Size = new System.Drawing.Size(70, 20);
            this.TimeOffsetText.TabIndex = 28;
            this.TimeOffsetText.Visible = false;
            // 
            // SendDeltaTimeTextbox
            // 
            this.SendDeltaTimeTextbox.BackColor = System.Drawing.Color.White;
            this.SendDeltaTimeTextbox.Location = new System.Drawing.Point(91, 69);
            this.SendDeltaTimeTextbox.Name = "SendDeltaTimeTextbox";
            this.SendDeltaTimeTextbox.ReadOnly = true;
            this.SendDeltaTimeTextbox.Size = new System.Drawing.Size(70, 20);
            this.SendDeltaTimeTextbox.TabIndex = 30;
            this.SendDeltaTimeTextbox.Visible = false;
            // 
            // ConnectInputUDPButton
            // 
            this.ConnectInputUDPButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectInputUDPButton.Location = new System.Drawing.Point(355, 34);
            this.ConnectInputUDPButton.Name = "ConnectInputUDPButton";
            this.ConnectInputUDPButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectInputUDPButton.TabIndex = 32;
            this.ConnectInputUDPButton.Text = "UDP port";
            this.ConnectInputUDPButton.UseVisualStyleBackColor = true;
            this.ConnectInputUDPButton.Click += new System.EventHandler(this.ConnectMotionInputUDPButton_Click);
            // 
            // CreateInputDataButton
            // 
            this.CreateInputDataButton.Location = new System.Drawing.Point(35, 34);
            this.CreateInputDataButton.Name = "CreateInputDataButton";
            this.CreateInputDataButton.Size = new System.Drawing.Size(75, 23);
            this.CreateInputDataButton.TabIndex = 32;
            this.CreateInputDataButton.Text = "Sinusoidal";
            this.CreateInputDataButton.UseVisualStyleBackColor = true;
            this.CreateInputDataButton.Click += new System.EventHandler(this.CreateInputDataButton_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(38, 42);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(47, 13);
            this.label30.TabIndex = 33;
            this.label30.Text = "Receive";
            this.label30.Visible = false;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(38, 72);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(32, 13);
            this.label35.TabIndex = 34;
            this.label35.Text = "Send";
            this.label35.Visible = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(118, 12);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(68, 13);
            this.label29.TabIndex = 35;
            this.label29.Text = "Input Source";
            // 
            // InputUDPPortBox
            // 
            this.InputUDPPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InputUDPPortBox.BackColor = System.Drawing.Color.White;
            this.InputUDPPortBox.Location = new System.Drawing.Point(436, 36);
            this.InputUDPPortBox.Name = "InputUDPPortBox";
            this.InputUDPPortBox.Size = new System.Drawing.Size(70, 20);
            this.InputUDPPortBox.TabIndex = 28;
            this.InputUDPPortBox.Text = "5345";
            this.InputUDPPortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RollFreqBox
            // 
            this.RollFreqBox.BackColor = System.Drawing.Color.White;
            this.RollFreqBox.Location = new System.Drawing.Point(136, 45);
            this.RollFreqBox.Name = "RollFreqBox";
            this.RollFreqBox.Size = new System.Drawing.Size(51, 20);
            this.RollFreqBox.TabIndex = 28;
            this.RollFreqBox.Text = "0";
            this.RollFreqBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // PitchFreqBox
            // 
            this.PitchFreqBox.BackColor = System.Drawing.Color.White;
            this.PitchFreqBox.Location = new System.Drawing.Point(136, 71);
            this.PitchFreqBox.Name = "PitchFreqBox";
            this.PitchFreqBox.Size = new System.Drawing.Size(51, 20);
            this.PitchFreqBox.TabIndex = 28;
            this.PitchFreqBox.Text = "0";
            this.PitchFreqBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // YawFreqBox
            // 
            this.YawFreqBox.BackColor = System.Drawing.Color.White;
            this.YawFreqBox.Location = new System.Drawing.Point(136, 129);
            this.YawFreqBox.Name = "YawFreqBox";
            this.YawFreqBox.Size = new System.Drawing.Size(51, 20);
            this.YawFreqBox.TabIndex = 28;
            this.YawFreqBox.Text = "0";
            this.YawFreqBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // YawAmpBox
            // 
            this.YawAmpBox.BackColor = System.Drawing.Color.White;
            this.YawAmpBox.Location = new System.Drawing.Point(79, 129);
            this.YawAmpBox.Name = "YawAmpBox";
            this.YawAmpBox.Size = new System.Drawing.Size(51, 20);
            this.YawAmpBox.TabIndex = 36;
            this.YawAmpBox.Text = "0";
            this.YawAmpBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // PitchAmpBox
            // 
            this.PitchAmpBox.BackColor = System.Drawing.Color.White;
            this.PitchAmpBox.Location = new System.Drawing.Point(79, 71);
            this.PitchAmpBox.Name = "PitchAmpBox";
            this.PitchAmpBox.Size = new System.Drawing.Size(51, 20);
            this.PitchAmpBox.TabIndex = 37;
            this.PitchAmpBox.Text = "0";
            this.PitchAmpBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RollAmpBox
            // 
            this.RollAmpBox.BackColor = System.Drawing.Color.White;
            this.RollAmpBox.Location = new System.Drawing.Point(79, 45);
            this.RollAmpBox.Name = "RollAmpBox";
            this.RollAmpBox.Size = new System.Drawing.Size(51, 20);
            this.RollAmpBox.TabIndex = 38;
            this.RollAmpBox.Text = "0";
            this.RollAmpBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RollCheckBox
            // 
            this.RollCheckBox.AutoSize = true;
            this.RollCheckBox.Location = new System.Drawing.Point(10, 47);
            this.RollCheckBox.Name = "RollCheckBox";
            this.RollCheckBox.Size = new System.Drawing.Size(68, 17);
            this.RollCheckBox.TabIndex = 42;
            this.RollCheckBox.Text = "Roll (rad)";
            this.RollCheckBox.UseVisualStyleBackColor = true;
            this.RollCheckBox.CheckedChanged += new System.EventHandler(this.RollCheckBox_CheckedChanged);
            // 
            // PitchCheckBox
            // 
            this.PitchCheckBox.AutoSize = true;
            this.PitchCheckBox.Location = new System.Drawing.Point(10, 73);
            this.PitchCheckBox.Name = "PitchCheckBox";
            this.PitchCheckBox.Size = new System.Drawing.Size(74, 17);
            this.PitchCheckBox.TabIndex = 42;
            this.PitchCheckBox.Text = "Pitch (rad)";
            this.PitchCheckBox.UseVisualStyleBackColor = true;
            this.PitchCheckBox.CheckedChanged += new System.EventHandler(this.PitchCheckBox_CheckedChanged);
            // 
            // YawCheckBox
            // 
            this.YawCheckBox.AutoSize = true;
            this.YawCheckBox.Location = new System.Drawing.Point(10, 131);
            this.YawCheckBox.Name = "YawCheckBox";
            this.YawCheckBox.Size = new System.Drawing.Size(71, 17);
            this.YawCheckBox.TabIndex = 42;
            this.YawCheckBox.Text = "Yaw (rad)";
            this.YawCheckBox.UseVisualStyleBackColor = true;
            this.YawCheckBox.CheckedChanged += new System.EventHandler(this.YawCheckBox_CheckedChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(92, 12);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(28, 13);
            this.label36.TabIndex = 35;
            this.label36.Text = "Amp";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(143, 4);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(38, 26);
            this.label37.TabIndex = 35;
            this.label37.Text = "Freq\r\n(rad/s)";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(405, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "X-Plane Data";
            // 
            // ControllerInitButton
            // 
            this.ControllerInitButton.Location = new System.Drawing.Point(116, 34);
            this.ControllerInitButton.Name = "ControllerInitButton";
            this.ControllerInitButton.Size = new System.Drawing.Size(75, 23);
            this.ControllerInitButton.TabIndex = 43;
            this.ControllerInitButton.Text = "Controller";
            this.ControllerInitButton.UseVisualStyleBackColor = true;
            this.ControllerInitButton.Click += new System.EventHandler(this.ControllerInitButton_Click);
            // 
            // PitchDownButton
            // 
            this.PitchDownButton.Location = new System.Drawing.Point(52, 4);
            this.PitchDownButton.Name = "PitchDownButton";
            this.PitchDownButton.Size = new System.Drawing.Size(45, 25);
            this.PitchDownButton.TabIndex = 44;
            this.PitchDownButton.Text = "Down";
            this.PitchDownButton.UseVisualStyleBackColor = true;
            this.PitchDownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PitchDown_MouseDown);
            this.PitchDownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Pitch_MouseUp);
            // 
            // PitchUpButton
            // 
            this.PitchUpButton.Location = new System.Drawing.Point(52, 73);
            this.PitchUpButton.Name = "PitchUpButton";
            this.PitchUpButton.Size = new System.Drawing.Size(45, 25);
            this.PitchUpButton.TabIndex = 44;
            this.PitchUpButton.Text = "Up";
            this.PitchUpButton.UseVisualStyleBackColor = true;
            this.PitchUpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PitchUp_MouseDown);
            this.PitchUpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Pitch_MouseUp);
            // 
            // RollLeftButton
            // 
            this.RollLeftButton.Location = new System.Drawing.Point(3, 36);
            this.RollLeftButton.Name = "RollLeftButton";
            this.RollLeftButton.Size = new System.Drawing.Size(51, 27);
            this.RollLeftButton.TabIndex = 44;
            this.RollLeftButton.Text = "Left";
            this.RollLeftButton.UseVisualStyleBackColor = true;
            this.RollLeftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RollLeft_MouseDown);
            this.RollLeftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Roll_MouseUp);
            // 
            // RollRightButton
            // 
            this.RollRightButton.Location = new System.Drawing.Point(94, 36);
            this.RollRightButton.Name = "RollRightButton";
            this.RollRightButton.Size = new System.Drawing.Size(45, 25);
            this.RollRightButton.TabIndex = 44;
            this.RollRightButton.Text = "Right";
            this.RollRightButton.UseVisualStyleBackColor = true;
            this.RollRightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RollRight_MouseDown);
            this.RollRightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Roll_MouseUp);
            // 
            // PitchRatioTextBox
            // 
            this.PitchRatioTextBox.BackColor = System.Drawing.Color.White;
            this.PitchRatioTextBox.Location = new System.Drawing.Point(270, 195);
            this.PitchRatioTextBox.Name = "PitchRatioTextBox";
            this.PitchRatioTextBox.Size = new System.Drawing.Size(51, 20);
            this.PitchRatioTextBox.TabIndex = 41;
            this.PitchRatioTextBox.Text = "0";
            this.PitchRatioTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RollRatioTextBox
            // 
            this.RollRatioTextBox.BackColor = System.Drawing.Color.White;
            this.RollRatioTextBox.Location = new System.Drawing.Point(270, 169);
            this.RollRatioTextBox.Name = "RollRatioTextBox";
            this.RollRatioTextBox.Size = new System.Drawing.Size(51, 20);
            this.RollRatioTextBox.TabIndex = 41;
            this.RollRatioTextBox.Text = "0";
            this.RollRatioTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(225, 198);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Pitch";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(225, 172);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 13);
            this.label15.TabIndex = 35;
            this.label15.Text = "Roll";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(225, 257);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(28, 13);
            this.label16.TabIndex = 35;
            this.label16.Text = "Yaw";
            // 
            // YawRatioTextBox
            // 
            this.YawRatioTextBox.BackColor = System.Drawing.Color.White;
            this.YawRatioTextBox.Location = new System.Drawing.Point(270, 254);
            this.YawRatioTextBox.Name = "YawRatioTextBox";
            this.YawRatioTextBox.Size = new System.Drawing.Size(51, 20);
            this.YawRatioTextBox.TabIndex = 41;
            this.YawRatioTextBox.Text = "0";
            this.YawRatioTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // YawRightButton
            // 
            this.YawRightButton.Location = new System.Drawing.Point(94, 126);
            this.YawRightButton.Name = "YawRightButton";
            this.YawRightButton.Size = new System.Drawing.Size(45, 25);
            this.YawRightButton.TabIndex = 44;
            this.YawRightButton.Text = "Right";
            this.YawRightButton.UseVisualStyleBackColor = true;
            this.YawRightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.YawRight_MouseDown);
            this.YawRightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Yaw_MouseUp);
            // 
            // YawLeftButton
            // 
            this.YawLeftButton.Location = new System.Drawing.Point(3, 126);
            this.YawLeftButton.Name = "YawLeftButton";
            this.YawLeftButton.Size = new System.Drawing.Size(51, 27);
            this.YawLeftButton.TabIndex = 44;
            this.YawLeftButton.Text = "Left";
            this.YawLeftButton.UseVisualStyleBackColor = true;
            this.YawLeftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.YawLeft_MouseDown);
            this.YawLeftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Yaw_MouseUp);
            // 
            // rollTimer
            // 
            this.rollTimer.Interval = 10;
            this.rollTimer.Tick += new System.EventHandler(this.rollTimer_Tick);
            // 
            // pitchTimer
            // 
            this.pitchTimer.Interval = 10;
            this.pitchTimer.Tick += new System.EventHandler(this.pitchTimer_Tick);
            // 
            // yawTimer
            // 
            this.yawTimer.Interval = 5;
            this.yawTimer.Tick += new System.EventHandler(this.yawTimer_Tick);
            // 
            // SinusoidalPanel
            // 
            this.SinusoidalPanel.Controls.Add(this.PitchAmpBox);
            this.SinusoidalPanel.Controls.Add(this.label37);
            this.SinusoidalPanel.Controls.Add(this.RollFreqBox);
            this.SinusoidalPanel.Controls.Add(this.PitchFreqBox);
            this.SinusoidalPanel.Controls.Add(this.YawFreqBox);
            this.SinusoidalPanel.Controls.Add(this.label36);
            this.SinusoidalPanel.Controls.Add(this.RollAmpBox);
            this.SinusoidalPanel.Controls.Add(this.RollCheckBox);
            this.SinusoidalPanel.Controls.Add(this.PitchCheckBox);
            this.SinusoidalPanel.Controls.Add(this.YawCheckBox);
            this.SinusoidalPanel.Controls.Add(this.YawAmpBox);
            this.SinusoidalPanel.Location = new System.Drawing.Point(10, 125);
            this.SinusoidalPanel.Name = "SinusoidalPanel";
            this.SinusoidalPanel.Size = new System.Drawing.Size(209, 168);
            this.SinusoidalPanel.TabIndex = 45;
            this.SinusoidalPanel.Visible = false;
            // 
            // ControllerPanel
            // 
            this.ControllerPanel.Controls.Add(this.PitchDownButton);
            this.ControllerPanel.Controls.Add(this.PitchUpButton);
            this.ControllerPanel.Controls.Add(this.YawLeftButton);
            this.ControllerPanel.Controls.Add(this.RollRightButton);
            this.ControllerPanel.Controls.Add(this.YawRightButton);
            this.ControllerPanel.Controls.Add(this.RollLeftButton);
            this.ControllerPanel.Location = new System.Drawing.Point(66, 124);
            this.ControllerPanel.Name = "ControllerPanel";
            this.ControllerPanel.Size = new System.Drawing.Size(153, 188);
            this.ControllerPanel.TabIndex = 46;
            this.ControllerPanel.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label28);
            this.panel1.Controls.Add(this.TimeOffsetText);
            this.panel1.Controls.Add(this.SendDeltaTimeTextbox);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Location = new System.Drawing.Point(12, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 47;
            this.panel1.Visible = false;
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(197, 34);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(75, 23);
            this.OpenFile.TabIndex = 48;
            this.OpenFile.Text = "File";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // openInputFileDialog
            // 
            this.openInputFileDialog.FileName = "openFileDialog1";
            this.openInputFileDialog.Filter = "\"CSV Files|*.csv|All files|*.*\"";
            // 
            // controllerTimer
            // 
            this.controllerTimer.Interval = 10;
            this.controllerTimer.Tick += new System.EventHandler(this.controllerTimer_Tick);
            // 
            // recalibrateButton
            // 
            this.recalibrateButton.Location = new System.Drawing.Point(116, 73);
            this.recalibrateButton.Name = "recalibrateButton";
            this.recalibrateButton.Size = new System.Drawing.Size(75, 23);
            this.recalibrateButton.TabIndex = 49;
            this.recalibrateButton.Text = "Recalibrate";
            this.recalibrateButton.UseVisualStyleBackColor = true;
            this.recalibrateButton.Click += new System.EventHandler(this.recalibrateButton_Click);
            // 
            // ControlGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 436);
            this.Controls.Add(this.recalibrateButton);
            this.Controls.Add(this.OpenFile);
            this.Controls.Add(this.ControllerPanel);
            this.Controls.Add(this.SinusoidalPanel);
            this.Controls.Add(this.ControllerInitButton);
            this.Controls.Add(this.YawRatioTextBox);
            this.Controls.Add(this.RollRatioTextBox);
            this.Controls.Add(this.PitchRatioTextBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.CreateInputDataButton);
            this.Controls.Add(this.ConnectInputUDPButton);
            this.Controls.Add(this.InputUDPPortBox);
            this.Controls.Add(this.InputRecordingCheckbox);
            this.Controls.Add(this.errorBar);
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
            this.Controls.Add(this.label2);
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
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControlGUI";
            this.Text = "KatanaSim Flight Control Input";
            this.SinusoidalPanel.ResumeLayout(false);
            this.SinusoidalPanel.PerformLayout();
            this.ControllerPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TextBox errorBar;
        private System.Windows.Forms.CheckBox InputRecordingCheckbox;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox TimeOffsetText;
        private System.Windows.Forms.TextBox SendDeltaTimeTextbox;
        private System.Windows.Forms.Button ConnectInputUDPButton;
        private System.Windows.Forms.Button CreateInputDataButton;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox InputUDPPortBox;
        private System.Windows.Forms.TextBox RollFreqBox;
        private System.Windows.Forms.TextBox PitchFreqBox;
        private System.Windows.Forms.TextBox YawFreqBox;
        private System.Windows.Forms.TextBox YawAmpBox;
        private System.Windows.Forms.TextBox PitchAmpBox;
        private System.Windows.Forms.TextBox RollAmpBox;
        private System.Windows.Forms.CheckBox RollCheckBox;
        private System.Windows.Forms.CheckBox PitchCheckBox;
        private System.Windows.Forms.CheckBox YawCheckBox;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button ControllerInitButton;
        private System.Windows.Forms.Button PitchDownButton;
        private System.Windows.Forms.Button PitchUpButton;
        private System.Windows.Forms.Button RollLeftButton;
        private System.Windows.Forms.Button RollRightButton;
        private System.Windows.Forms.TextBox PitchRatioTextBox;
        private System.Windows.Forms.TextBox RollRatioTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox YawRatioTextBox;
        private System.Windows.Forms.Button YawRightButton;
        private System.Windows.Forms.Button YawLeftButton;
        private System.Windows.Forms.Timer rollTimer;
        private System.Windows.Forms.Timer pitchTimer;
        private System.Windows.Forms.Timer yawTimer;
        private System.Windows.Forms.Panel SinusoidalPanel;
        private System.Windows.Forms.Panel ControllerPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OpenFile;
        private System.Windows.Forms.OpenFileDialog openInputFileDialog;
        private System.Windows.Forms.Timer controllerTimer;
        private System.Windows.Forms.Button recalibrateButton;
    }
}

