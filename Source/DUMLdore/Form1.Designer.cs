namespace DUMLdore
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
            this.components = new System.ComponentModel.Container();
            this.btnLoadFW = new System.Windows.Forms.Button();
            this.btnFlashFW = new System.Windows.Forms.Button();
            this.btnBackupFW = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoGoggles = new System.Windows.Forms.RadioButton();
            this.rdoRemote = new System.Windows.Forms.RadioButton();
            this.rdoAircraft = new System.Windows.Forms.RadioButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadFW
            // 
            this.btnLoadFW.Enabled = false;
            this.btnLoadFW.Location = new System.Drawing.Point(156, 175);
            this.btnLoadFW.Name = "btnLoadFW";
            this.btnLoadFW.Size = new System.Drawing.Size(156, 31);
            this.btnLoadFW.TabIndex = 0;
            this.btnLoadFW.Text = "Load Firmware";
            this.btnLoadFW.UseVisualStyleBackColor = true;
            this.btnLoadFW.Click += new System.EventHandler(this.btnLoadFW_Click);
            // 
            // btnFlashFW
            // 
            this.btnFlashFW.Enabled = false;
            this.btnFlashFW.Location = new System.Drawing.Point(156, 212);
            this.btnFlashFW.Name = "btnFlashFW";
            this.btnFlashFW.Size = new System.Drawing.Size(156, 31);
            this.btnFlashFW.TabIndex = 1;
            this.btnFlashFW.Text = "Flash Firmware";
            this.btnFlashFW.UseVisualStyleBackColor = true;
            this.btnFlashFW.Click += new System.EventHandler(this.btnFlashFW_Click);
            // 
            // btnBackupFW
            // 
            this.btnBackupFW.Enabled = false;
            this.btnBackupFW.Location = new System.Drawing.Point(156, 249);
            this.btnBackupFW.Name = "btnBackupFW";
            this.btnBackupFW.Size = new System.Drawing.Size(156, 31);
            this.btnBackupFW.TabIndex = 2;
            this.btnBackupFW.Text = "Backup Firmware";
            this.btnBackupFW.UseVisualStyleBackColor = true;
            this.btnBackupFW.Click += new System.EventHandler(this.btnBackupFW_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(107, 348);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(13, 20);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = " ";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Drone Firmware Utility";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoGoggles);
            this.groupBox1.Controls.Add(this.rdoRemote);
            this.groupBox1.Controls.Add(this.rdoAircraft);
            this.groupBox1.Location = new System.Drawing.Point(-109, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(609, 115);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // rdoGoggles
            // 
            this.rdoGoggles.AutoSize = true;
            this.rdoGoggles.Location = new System.Drawing.Point(157, 75);
            this.rdoGoggles.Name = "rdoGoggles";
            this.rdoGoggles.Size = new System.Drawing.Size(94, 24);
            this.rdoGoggles.TabIndex = 2;
            this.rdoGoggles.TabStop = true;
            this.rdoGoggles.Text = "Goggles";
            this.rdoGoggles.UseVisualStyleBackColor = true;
            // 
            // rdoRemote
            // 
            this.rdoRemote.AutoSize = true;
            this.rdoRemote.Location = new System.Drawing.Point(157, 45);
            this.rdoRemote.Name = "rdoRemote";
            this.rdoRemote.Size = new System.Drawing.Size(146, 24);
            this.rdoRemote.TabIndex = 1;
            this.rdoRemote.TabStop = true;
            this.rdoRemote.Text = "Remote Control";
            this.rdoRemote.UseVisualStyleBackColor = true;
            // 
            // rdoAircraft
            // 
            this.rdoAircraft.AutoSize = true;
            this.rdoAircraft.Checked = true;
            this.rdoAircraft.Location = new System.Drawing.Point(157, 13);
            this.rdoAircraft.Name = "rdoAircraft";
            this.rdoAircraft.Size = new System.Drawing.Size(85, 24);
            this.rdoAircraft.TabIndex = 0;
            this.rdoAircraft.TabStop = true;
            this.rdoAircraft.Text = "Aircraft";
            this.rdoAircraft.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(29, 309);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(417, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 9;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 380);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(475, 30);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 410);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnBackupFW);
            this.Controls.Add(this.btnFlashFW);
            this.Controls.Add(this.btnLoadFW);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = " DUMLdore V1.81 - jezzab";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFW;
        private System.Windows.Forms.Button btnFlashFW;
        private System.Windows.Forms.Button btnBackupFW;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoGoggles;
        private System.Windows.Forms.RadioButton rdoRemote;
        private System.Windows.Forms.RadioButton rdoAircraft;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

