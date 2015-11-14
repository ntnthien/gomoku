namespace Client
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
            this.btConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.bgwReceiveData = new System.ComponentModel.BackgroundWorker();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblFlag = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(191, 72);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(81, 23);
            this.btConnect.TabIndex = 0;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(21, 82);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Wait...";
            // 
            // bgwReceiveData
            // 
            this.bgwReceiveData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwReceiveData_DoWork);
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(21, 15);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 13);
            this.lblIP.TabIndex = 3;
            this.lblIP.Text = "IP:";
            // 
            // lblFlag
            // 
            this.lblFlag.AutoSize = true;
            this.lblFlag.Location = new System.Drawing.Point(12, 49);
            this.lblFlag.Name = "lblFlag";
            this.lblFlag.Size = new System.Drawing.Size(30, 13);
            this.lblFlag.TabIndex = 4;
            this.lblFlag.Text = "Flag:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(47, 12);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(225, 20);
            this.txtIP.TabIndex = 5;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(47, 46);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(225, 20);
            this.txtPort.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 110);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.lblFlag);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bgwReceiveData;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblFlag;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
    }
}

