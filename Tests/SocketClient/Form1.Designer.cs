namespace AtmSocketClient
{
    partial class Form1
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
            this.btnClear = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxConnectStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.richTextTxMessage = new System.Windows.Forms.RichTextBox();
            this.richTextRxMessage = new System.Windows.Forms.RichTextBox();
            this.btnStx = new System.Windows.Forms.Button();
            this.btnAck = new System.Windows.Forms.Button();
            this.btnEot = new System.Windows.Forms.Button();
            this.radioHexFormat = new System.Windows.Forms.CheckBox();
            this.radioHeaderLength = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(398, 503);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(64, 24);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.BackColor = System.Drawing.Color.Red;
            this.buttonDisconnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDisconnect.ForeColor = System.Drawing.Color.Yellow;
            this.buttonDisconnect.Location = new System.Drawing.Point(436, 9);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(72, 48);
            this.buttonDisconnect.TabIndex = 30;
            this.buttonDisconnect.Text = "Disconnet From Server";
            this.buttonDisconnect.UseVisualStyleBackColor = false;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(7, 355);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(240, 24);
            this.buttonSendMessage.TabIndex = 29;
            this.buttonSendMessage.Text = "Send Message";
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 509);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "Connection Status";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(462, 503);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(64, 24);
            this.buttonClose.TabIndex = 27;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxConnectStatus
            // 
            this.textBoxConnectStatus.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxConnectStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnectStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.textBoxConnectStatus.Location = new System.Drawing.Point(132, 509);
            this.textBoxConnectStatus.Name = "textBoxConnectStatus";
            this.textBoxConnectStatus.ReadOnly = true;
            this.textBoxConnectStatus.Size = new System.Drawing.Size(240, 13);
            this.textBoxConnectStatus.TabIndex = 26;
            this.textBoxConnectStatus.Text = "Not Connected";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Message To Server";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(260, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Message From Server";
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.ForeColor = System.Drawing.Color.Yellow;
            this.buttonConnect.Location = new System.Drawing.Point(348, 9);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 48);
            this.buttonConnect.TabIndex = 23;
            this.buttonConnect.Text = "Connect To Server";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(116, 32);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(48, 20);
            this.textBoxPort.TabIndex = 22;
            this.textBoxPort.Text = "9100";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 21;
            this.label2.Text = "Server Port";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "Server IP Address";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(116, 9);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(152, 20);
            this.textBoxIP.TabIndex = 19;
            // 
            // richTextTxMessage
            // 
            this.richTextTxMessage.Location = new System.Drawing.Point(12, 103);
            this.richTextTxMessage.Name = "richTextTxMessage";
            this.richTextTxMessage.Size = new System.Drawing.Size(240, 246);
            this.richTextTxMessage.TabIndex = 18;
            this.richTextTxMessage.Text = "";
            // 
            // richTextRxMessage
            // 
            this.richTextRxMessage.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.richTextRxMessage.ForeColor = System.Drawing.Color.White;
            this.richTextRxMessage.Location = new System.Drawing.Point(269, 81);
            this.richTextRxMessage.Name = "richTextRxMessage";
            this.richTextRxMessage.ReadOnly = true;
            this.richTextRxMessage.Size = new System.Drawing.Size(248, 382);
            this.richTextRxMessage.TabIndex = 17;
            this.richTextRxMessage.Text = "";
            // 
            // btnStx
            // 
            this.btnStx.Location = new System.Drawing.Point(7, 395);
            this.btnStx.Name = "btnStx";
            this.btnStx.Size = new System.Drawing.Size(109, 24);
            this.btnStx.TabIndex = 32;
            this.btnStx.Text = "Send STX";
            this.btnStx.Click += new System.EventHandler(this.btnStx_Click);
            // 
            // btnAck
            // 
            this.btnAck.Location = new System.Drawing.Point(7, 425);
            this.btnAck.Name = "btnAck";
            this.btnAck.Size = new System.Drawing.Size(109, 24);
            this.btnAck.TabIndex = 33;
            this.btnAck.Text = "Send ACK";
            this.btnAck.Click += new System.EventHandler(this.btnAck_Click);
            // 
            // btnEot
            // 
            this.btnEot.Location = new System.Drawing.Point(7, 455);
            this.btnEot.Name = "btnEot";
            this.btnEot.Size = new System.Drawing.Size(109, 24);
            this.btnEot.TabIndex = 34;
            this.btnEot.Text = "Send EOT";
            this.btnEot.Click += new System.EventHandler(this.btnEot_Click);
            // 
            // radioHexFormat
            // 
            this.radioHexFormat.AutoSize = true;
            this.radioHexFormat.Location = new System.Drawing.Point(15, 78);
            this.radioHexFormat.Name = "radioHexFormat";
            this.radioHexFormat.Size = new System.Drawing.Size(77, 17);
            this.radioHexFormat.TabIndex = 37;
            this.radioHexFormat.Text = "HexFormat";
            this.radioHexFormat.UseVisualStyleBackColor = true;
            // 
            // radioHeaderLength
            // 
            this.radioHeaderLength.AutoSize = true;
            this.radioHeaderLength.Location = new System.Drawing.Point(98, 78);
            this.radioHeaderLength.Name = "radioHeaderLength";
            this.radioHeaderLength.Size = new System.Drawing.Size(135, 17);
            this.radioHeaderLength.TabIndex = 38;
            this.radioHeaderLength.Text = "Include Header Length";
            this.radioHeaderLength.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 534);
            this.Controls.Add(this.radioHeaderLength);
            this.Controls.Add(this.radioHexFormat);
            this.Controls.Add(this.btnEot);
            this.Controls.Add(this.btnAck);
            this.Controls.Add(this.btnStx);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxConnectStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.richTextTxMessage);
            this.Controls.Add(this.richTextRxMessage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxConnectStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.RichTextBox richTextTxMessage;
        private System.Windows.Forms.RichTextBox richTextRxMessage;
        private System.Windows.Forms.Button btnStx;
        private System.Windows.Forms.Button btnAck;
        private System.Windows.Forms.Button btnEot;
        private System.Windows.Forms.CheckBox radioHexFormat;
        private System.Windows.Forms.CheckBox radioHeaderLength;
    }
}

