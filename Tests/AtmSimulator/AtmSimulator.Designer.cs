namespace AtmSimulatorWinForm
{
    partial class AtmSimulator
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
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textAtm = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextResults = new System.Windows.Forms.RichTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.interval = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textTerminalId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Red;
            this.btnStop.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.Yellow;
            this.btnStop.Location = new System.Drawing.Point(844, 23);
            this.btnStop.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(216, 92);
            this.btnStop.TabIndex = 37;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.Yellow;
            this.btnStart.Location = new System.Drawing.Point(604, 23);
            this.btnStart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(216, 92);
            this.btnStart.TabIndex = 35;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(248, 67);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(92, 31);
            this.textBoxPort.TabIndex = 34;
            this.textBoxPort.Text = "9100";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(44, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 31);
            this.label2.TabIndex = 33;
            this.label2.Text = "Triton Port";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(44, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 31);
            this.label1.TabIndex = 32;
            this.label1.Text = "Triton IP Address";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(248, 23);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(300, 31);
            this.textBoxIP.TabIndex = 31;
            // 
            // textAtm
            // 
            this.textAtm.Location = new System.Drawing.Point(188, 260);
            this.textAtm.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textAtm.Name = "textAtm";
            this.textAtm.Size = new System.Drawing.Size(130, 31);
            this.textAtm.TabIndex = 39;
            this.textAtm.Text = "1";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(44, 265);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 31);
            this.label3.TabIndex = 38;
            this.label3.Text = "Current ATM";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(334, 265);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 31);
            this.label4.TabIndex = 40;
            this.label4.Text = "Machine";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(44, 348);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(384, 31);
            this.label5.TabIndex = 42;
            this.label5.Text = "Results";
            // 
            // richTextResults
            // 
            this.richTextResults.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.richTextResults.ForeColor = System.Drawing.Color.White;
            this.richTextResults.Location = new System.Drawing.Point(24, 385);
            this.richTextResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.richTextResults.Name = "richTextResults";
            this.richTextResults.ReadOnly = true;
            this.richTextResults.Size = new System.Drawing.Size(1032, 1094);
            this.richTextResults.TabIndex = 41;
            this.richTextResults.Text = "";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(804, 1527);
            this.btnClear.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(128, 46);
            this.btnClear.TabIndex = 44;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(932, 1527);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(128, 46);
            this.btnClose.TabIndex = 43;
            this.btnClose.Text = "Close";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(44, 138);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 31);
            this.label6.TabIndex = 46;
            this.label6.Text = "Terminal Id";
            // 
            // interval
            // 
            this.interval.Location = new System.Drawing.Point(132, 194);
            this.interval.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.interval.Name = "interval";
            this.interval.Size = new System.Drawing.Size(208, 31);
            this.interval.TabIndex = 48;
            this.interval.Text = "6000";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(44, 200);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 31);
            this.label7.TabIndex = 47;
            this.label7.Text = "Interval";
            // 
            // textTerminalId
            // 
            this.textTerminalId.Location = new System.Drawing.Point(164, 133);
            this.textTerminalId.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textTerminalId.Name = "textTerminalId";
            this.textTerminalId.Size = new System.Drawing.Size(300, 31);
            this.textTerminalId.TabIndex = 49;
            this.textTerminalId.Text = "SM200005";
            // 
            // AtmSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 1441);
            this.Controls.Add(this.textTerminalId);
            this.Controls.Add(this.interval);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextResults);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textAtm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxIP);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "AtmSimulator";
            this.Text = "AtmSimulator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textAtm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.RichTextBox richTextResults;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox interval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textTerminalId;
    }
}

