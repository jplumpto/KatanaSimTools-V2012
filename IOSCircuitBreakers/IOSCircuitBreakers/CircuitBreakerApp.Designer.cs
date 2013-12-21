namespace IOSCircuitBreakers
{
    partial class CircuitBreakerApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CircuitBreakerApp));
            this._arduinoPort = new System.IO.Ports.SerialPort(this.components);
            this._arduinoTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this._arduinoCompileDate = new System.Windows.Forms.TextBox();
            this.transpCtrl12 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl11 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl10 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl9 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl8 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl7 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl6 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl5 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl4 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl3 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl2 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl23 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl22 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl21 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl20 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl19 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl18 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl17 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl16 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl15 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl14 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl26 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl25 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl24 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl13 = new IOSCircuitBreakers.TranspCtrl();
            this.transpCtrl1 = new IOSCircuitBreakers.TranspCtrl();
            this.SuspendLayout();
            // 
            // _arduinoTimer
            // 
            this._arduinoTimer.Interval = 2000;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(337, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Arduino Code Compile Date\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _arduinoCompileDate
            // 
            this._arduinoCompileDate.Location = new System.Drawing.Point(337, 23);
            this._arduinoCompileDate.Name = "_arduinoCompileDate";
            this._arduinoCompileDate.Size = new System.Drawing.Size(137, 20);
            this._arduinoCompileDate.TabIndex = 4;
            this._arduinoCompileDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // transpCtrl12
            // 
            this.transpCtrl12.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl12.Location = new System.Drawing.Point(65, 582);
            this.transpCtrl12.Name = "transpCtrl12";
            this.transpCtrl12.Opacity = 100;
            this.transpCtrl12.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl12.TabIndex = 28;
            this.transpCtrl12.Click += new System.EventHandler(this.cb12_Click);
            // 
            // transpCtrl11
            // 
            this.transpCtrl11.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl11.Location = new System.Drawing.Point(65, 524);
            this.transpCtrl11.Name = "transpCtrl11";
            this.transpCtrl11.Opacity = 100;
            this.transpCtrl11.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl11.TabIndex = 28;
            this.transpCtrl11.Click += new System.EventHandler(this.cb11_Click);
            // 
            // transpCtrl10
            // 
            this.transpCtrl10.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl10.Location = new System.Drawing.Point(65, 476);
            this.transpCtrl10.Name = "transpCtrl10";
            this.transpCtrl10.Opacity = 100;
            this.transpCtrl10.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl10.TabIndex = 28;
            this.transpCtrl10.Click += new System.EventHandler(this.cb10_Click);
            // 
            // transpCtrl9
            // 
            this.transpCtrl9.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl9.Location = new System.Drawing.Point(63, 428);
            this.transpCtrl9.Name = "transpCtrl9";
            this.transpCtrl9.Opacity = 100;
            this.transpCtrl9.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl9.TabIndex = 28;
            this.transpCtrl9.Click += new System.EventHandler(this.cb9_Click);
            // 
            // transpCtrl8
            // 
            this.transpCtrl8.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl8.Location = new System.Drawing.Point(65, 375);
            this.transpCtrl8.Name = "transpCtrl8";
            this.transpCtrl8.Opacity = 100;
            this.transpCtrl8.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl8.TabIndex = 28;
            this.transpCtrl8.Click += new System.EventHandler(this.cb8_Click);
            // 
            // transpCtrl7
            // 
            this.transpCtrl7.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl7.Location = new System.Drawing.Point(65, 324);
            this.transpCtrl7.Name = "transpCtrl7";
            this.transpCtrl7.Opacity = 100;
            this.transpCtrl7.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl7.TabIndex = 28;
            this.transpCtrl7.Click += new System.EventHandler(this.cb7_Click);
            // 
            // transpCtrl6
            // 
            this.transpCtrl6.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl6.Location = new System.Drawing.Point(64, 274);
            this.transpCtrl6.Name = "transpCtrl6";
            this.transpCtrl6.Opacity = 100;
            this.transpCtrl6.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl6.TabIndex = 28;
            this.transpCtrl6.Click += new System.EventHandler(this.cb6_Click);
            // 
            // transpCtrl5
            // 
            this.transpCtrl5.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl5.Location = new System.Drawing.Point(63, 223);
            this.transpCtrl5.Name = "transpCtrl5";
            this.transpCtrl5.Opacity = 100;
            this.transpCtrl5.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl5.TabIndex = 28;
            this.transpCtrl5.Click += new System.EventHandler(this.cb5_Click);
            // 
            // transpCtrl4
            // 
            this.transpCtrl4.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl4.Location = new System.Drawing.Point(63, 169);
            this.transpCtrl4.Name = "transpCtrl4";
            this.transpCtrl4.Opacity = 100;
            this.transpCtrl4.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl4.TabIndex = 28;
            this.transpCtrl4.Click += new System.EventHandler(this.cb4_Click);
            // 
            // transpCtrl3
            // 
            this.transpCtrl3.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl3.Location = new System.Drawing.Point(59, 115);
            this.transpCtrl3.Name = "transpCtrl3";
            this.transpCtrl3.Opacity = 100;
            this.transpCtrl3.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl3.TabIndex = 28;
            this.transpCtrl3.Click += new System.EventHandler(this.cb3_Click);
            // 
            // transpCtrl2
            // 
            this.transpCtrl2.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl2.Location = new System.Drawing.Point(59, 64);
            this.transpCtrl2.Name = "transpCtrl2";
            this.transpCtrl2.Opacity = 100;
            this.transpCtrl2.Size = new System.Drawing.Size(54, 52);
            this.transpCtrl2.TabIndex = 28;
            this.transpCtrl2.Click += new System.EventHandler(this.cb2_Click);
            // 
            // transpCtrl23
            // 
            this.transpCtrl23.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl23.Location = new System.Drawing.Point(191, 579);
            this.transpCtrl23.Name = "transpCtrl23";
            this.transpCtrl23.Opacity = 100;
            this.transpCtrl23.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl23.TabIndex = 28;
            this.transpCtrl23.Click += new System.EventHandler(this.cb23_Click);
            // 
            // transpCtrl22
            // 
            this.transpCtrl22.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl22.Location = new System.Drawing.Point(190, 525);
            this.transpCtrl22.Name = "transpCtrl22";
            this.transpCtrl22.Opacity = 100;
            this.transpCtrl22.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl22.TabIndex = 28;
            this.transpCtrl22.Click += new System.EventHandler(this.cb22_Click);
            // 
            // transpCtrl21
            // 
            this.transpCtrl21.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl21.Location = new System.Drawing.Point(190, 476);
            this.transpCtrl21.Name = "transpCtrl21";
            this.transpCtrl21.Opacity = 100;
            this.transpCtrl21.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl21.TabIndex = 28;
            this.transpCtrl21.Click += new System.EventHandler(this.cb21_Click);
            // 
            // transpCtrl20
            // 
            this.transpCtrl20.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl20.Location = new System.Drawing.Point(190, 428);
            this.transpCtrl20.Name = "transpCtrl20";
            this.transpCtrl20.Opacity = 100;
            this.transpCtrl20.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl20.TabIndex = 28;
            this.transpCtrl20.Click += new System.EventHandler(this.cb20_Click);
            // 
            // transpCtrl19
            // 
            this.transpCtrl19.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl19.Location = new System.Drawing.Point(189, 375);
            this.transpCtrl19.Name = "transpCtrl19";
            this.transpCtrl19.Opacity = 100;
            this.transpCtrl19.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl19.TabIndex = 28;
            this.transpCtrl19.Click += new System.EventHandler(this.cb19_Click);
            // 
            // transpCtrl18
            // 
            this.transpCtrl18.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl18.Location = new System.Drawing.Point(189, 324);
            this.transpCtrl18.Name = "transpCtrl18";
            this.transpCtrl18.Opacity = 100;
            this.transpCtrl18.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl18.TabIndex = 28;
            this.transpCtrl18.Click += new System.EventHandler(this.cb18_Click);
            // 
            // transpCtrl17
            // 
            this.transpCtrl17.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl17.Location = new System.Drawing.Point(189, 274);
            this.transpCtrl17.Name = "transpCtrl17";
            this.transpCtrl17.Opacity = 100;
            this.transpCtrl17.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl17.TabIndex = 28;
            this.transpCtrl17.Click += new System.EventHandler(this.cb17_Click);
            // 
            // transpCtrl16
            // 
            this.transpCtrl16.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl16.Location = new System.Drawing.Point(189, 223);
            this.transpCtrl16.Name = "transpCtrl16";
            this.transpCtrl16.Opacity = 100;
            this.transpCtrl16.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl16.TabIndex = 28;
            this.transpCtrl16.Click += new System.EventHandler(this.cb16_Click);
            // 
            // transpCtrl15
            // 
            this.transpCtrl15.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl15.Location = new System.Drawing.Point(191, 168);
            this.transpCtrl15.Name = "transpCtrl15";
            this.transpCtrl15.Opacity = 100;
            this.transpCtrl15.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl15.TabIndex = 28;
            this.transpCtrl15.Click += new System.EventHandler(this.cb15_Click);
            // 
            // transpCtrl14
            // 
            this.transpCtrl14.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl14.Location = new System.Drawing.Point(191, 113);
            this.transpCtrl14.Name = "transpCtrl14";
            this.transpCtrl14.Opacity = 100;
            this.transpCtrl14.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl14.TabIndex = 28;
            this.transpCtrl14.Click += new System.EventHandler(this.cb14_Click);
            // 
            // transpCtrl26
            // 
            this.transpCtrl26.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl26.Location = new System.Drawing.Point(312, 321);
            this.transpCtrl26.Name = "transpCtrl26";
            this.transpCtrl26.Opacity = 100;
            this.transpCtrl26.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl26.TabIndex = 28;
            this.transpCtrl26.Click += new System.EventHandler(this.cb26_Click);
            // 
            // transpCtrl25
            // 
            this.transpCtrl25.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl25.Location = new System.Drawing.Point(312, 264);
            this.transpCtrl25.Name = "transpCtrl25";
            this.transpCtrl25.Opacity = 100;
            this.transpCtrl25.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl25.TabIndex = 28;
            this.transpCtrl25.Click += new System.EventHandler(this.cb25_Click);
            // 
            // transpCtrl24
            // 
            this.transpCtrl24.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl24.Location = new System.Drawing.Point(313, 214);
            this.transpCtrl24.Name = "transpCtrl24";
            this.transpCtrl24.Opacity = 100;
            this.transpCtrl24.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl24.TabIndex = 28;
            this.transpCtrl24.Click += new System.EventHandler(this.cb24_Click);
            // 
            // transpCtrl13
            // 
            this.transpCtrl13.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl13.Location = new System.Drawing.Point(191, 58);
            this.transpCtrl13.Name = "transpCtrl13";
            this.transpCtrl13.Opacity = 100;
            this.transpCtrl13.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl13.TabIndex = 28;
            this.transpCtrl13.Click += new System.EventHandler(this.cb13_Click);
            // 
            // transpCtrl1
            // 
            this.transpCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.transpCtrl1.Location = new System.Drawing.Point(55, 10);
            this.transpCtrl1.Name = "transpCtrl1";
            this.transpCtrl1.Opacity = 100;
            this.transpCtrl1.Size = new System.Drawing.Size(58, 52);
            this.transpCtrl1.TabIndex = 28;
            this.transpCtrl1.Click += new System.EventHandler(this.cb1_Click);
            // 
            // CircuitBreakerApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(502, 631);
            this.Controls.Add(this.transpCtrl12);
            this.Controls.Add(this.transpCtrl11);
            this.Controls.Add(this.transpCtrl10);
            this.Controls.Add(this.transpCtrl9);
            this.Controls.Add(this.transpCtrl8);
            this.Controls.Add(this.transpCtrl7);
            this.Controls.Add(this.transpCtrl6);
            this.Controls.Add(this.transpCtrl5);
            this.Controls.Add(this.transpCtrl4);
            this.Controls.Add(this.transpCtrl3);
            this.Controls.Add(this.transpCtrl2);
            this.Controls.Add(this.transpCtrl23);
            this.Controls.Add(this.transpCtrl22);
            this.Controls.Add(this.transpCtrl21);
            this.Controls.Add(this.transpCtrl20);
            this.Controls.Add(this.transpCtrl19);
            this.Controls.Add(this.transpCtrl18);
            this.Controls.Add(this.transpCtrl17);
            this.Controls.Add(this.transpCtrl16);
            this.Controls.Add(this.transpCtrl15);
            this.Controls.Add(this.transpCtrl14);
            this.Controls.Add(this.transpCtrl26);
            this.Controls.Add(this.transpCtrl25);
            this.Controls.Add(this.transpCtrl24);
            this.Controls.Add(this.transpCtrl13);
            this.Controls.Add(this.transpCtrl1);
            this.Controls.Add(this._arduinoCompileDate);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CircuitBreakerApp";
            this.Text = "CircuitBreakerApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort _arduinoPort;
        private System.Windows.Forms.Timer _arduinoTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _arduinoCompileDate;
        private TranspCtrl transpCtrl1;
        private TranspCtrl transpCtrl2;
        private TranspCtrl transpCtrl3;
        private TranspCtrl transpCtrl4;
        private TranspCtrl transpCtrl5;
        private TranspCtrl transpCtrl6;
        private TranspCtrl transpCtrl7;
        private TranspCtrl transpCtrl8;
        private TranspCtrl transpCtrl9;
        private TranspCtrl transpCtrl10;
        private TranspCtrl transpCtrl11;
        private TranspCtrl transpCtrl12;
        private TranspCtrl transpCtrl13;
        private TranspCtrl transpCtrl14;
        private TranspCtrl transpCtrl15;
        private TranspCtrl transpCtrl16;
        private TranspCtrl transpCtrl17;
        private TranspCtrl transpCtrl18;
        private TranspCtrl transpCtrl19;
        private TranspCtrl transpCtrl20;
        private TranspCtrl transpCtrl21;
        private TranspCtrl transpCtrl22;
        private TranspCtrl transpCtrl23;
        private TranspCtrl transpCtrl24;
        private TranspCtrl transpCtrl25;
        private TranspCtrl transpCtrl26;
    }
}