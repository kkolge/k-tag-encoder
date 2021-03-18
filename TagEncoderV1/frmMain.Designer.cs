
namespace TagEncoderV1
{
    partial class frmMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbCopyTidToEpc = new System.Windows.Forms.RadioButton();
            this.rbInputFile = new System.Windows.Forms.RadioButton();
            this.gbpInputFile = new System.Windows.Forms.GroupBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbpSetPassword = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAccessPassword = new System.Windows.Forms.TextBox();
            this.txtKillPassword = new System.Windows.Forms.TextBox();
            this.cbPermaLock = new System.Windows.Forms.CheckBox();
            this.gbpOptions = new System.Windows.Forms.GroupBox();
            this.rbSpecify = new System.Windows.Forms.RadioButton();
            this.rbFromFile = new System.Windows.Forms.RadioButton();
            this.cbLockTags = new System.Windows.Forms.CheckBox();
            this.btnStartEncoding = new System.Windows.Forms.Button();
            this.btnStopEncoding = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbpInputFile.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbpSetPassword.SuspendLayout();
            this.gbpOptions.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbCopyTidToEpc);
            this.groupBox1.Controls.Add(this.rbInputFile);
            this.groupBox1.Location = new System.Drawing.Point(13, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select source";
            // 
            // rbCopyTidToEpc
            // 
            this.rbCopyTidToEpc.AutoSize = true;
            this.rbCopyTidToEpc.Location = new System.Drawing.Point(7, 76);
            this.rbCopyTidToEpc.Name = "rbCopyTidToEpc";
            this.rbCopyTidToEpc.Size = new System.Drawing.Size(134, 21);
            this.rbCopyTidToEpc.TabIndex = 1;
            this.rbCopyTidToEpc.TabStop = true;
            this.rbCopyTidToEpc.Text = "Copy TID to EPC";
            this.rbCopyTidToEpc.UseVisualStyleBackColor = true;
            this.rbCopyTidToEpc.CheckedChanged += new System.EventHandler(this.rbCopyTidToEpc_CheckedChanged);
            // 
            // rbInputFile
            // 
            this.rbInputFile.AutoSize = true;
            this.rbInputFile.Location = new System.Drawing.Point(7, 35);
            this.rbInputFile.Name = "rbInputFile";
            this.rbInputFile.Size = new System.Drawing.Size(82, 21);
            this.rbInputFile.TabIndex = 0;
            this.rbInputFile.TabStop = true;
            this.rbInputFile.Text = "Input file";
            this.rbInputFile.UseVisualStyleBackColor = true;
            this.rbInputFile.CheckedChanged += new System.EventHandler(this.rbInputFile_CheckedChanged);
            // 
            // gbpInputFile
            // 
            this.gbpInputFile.Controls.Add(this.lblPath);
            this.gbpInputFile.Controls.Add(this.lblOutputFile);
            this.gbpInputFile.Controls.Add(this.lblInputFile);
            this.gbpInputFile.Controls.Add(this.btnBrowse);
            this.gbpInputFile.Location = new System.Drawing.Point(189, 49);
            this.gbpInputFile.Name = "gbpInputFile";
            this.gbpInputFile.Size = new System.Drawing.Size(581, 116);
            this.gbpInputFile.TabIndex = 1;
            this.gbpInputFile.TabStop = false;
            this.gbpInputFile.Text = "Input file";
            // 
            // lblPath
            // 
            this.lblPath.Location = new System.Drawing.Point(111, 49);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(464, 48);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "Path";
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(342, 22);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(77, 17);
            this.lblOutputFile.TabIndex = 2;
            this.lblOutputFile.Text = "Output File";
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Location = new System.Drawing.Point(111, 22);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(65, 17);
            this.lblInputFile.TabIndex = 1;
            this.lblInputFile.Text = "Input File";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(7, 22);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 75);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbpSetPassword);
            this.groupBox3.Controls.Add(this.cbPermaLock);
            this.groupBox3.Controls.Add(this.gbpOptions);
            this.groupBox3.Controls.Add(this.cbLockTags);
            this.groupBox3.Location = new System.Drawing.Point(13, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(757, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Passwords";
            // 
            // gbpSetPassword
            // 
            this.gbpSetPassword.Controls.Add(this.label1);
            this.gbpSetPassword.Controls.Add(this.label5);
            this.gbpSetPassword.Controls.Add(this.txtAccessPassword);
            this.gbpSetPassword.Controls.Add(this.txtKillPassword);
            this.gbpSetPassword.Location = new System.Drawing.Point(427, 12);
            this.gbpSetPassword.Name = "gbpSetPassword";
            this.gbpSetPassword.Size = new System.Drawing.Size(324, 82);
            this.gbpSetPassword.TabIndex = 7;
            this.gbpSetPassword.TabStop = false;
            this.gbpSetPassword.Text = "Set Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "KILL PWD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "ACCESS PWD";
            // 
            // txtAccessPassword
            // 
            this.txtAccessPassword.Location = new System.Drawing.Point(173, 45);
            this.txtAccessPassword.Name = "txtAccessPassword";
            this.txtAccessPassword.Size = new System.Drawing.Size(100, 22);
            this.txtAccessPassword.TabIndex = 5;
            this.txtAccessPassword.Text = "00 00 00 00";
            // 
            // txtKillPassword
            // 
            this.txtKillPassword.Location = new System.Drawing.Point(173, 18);
            this.txtKillPassword.Name = "txtKillPassword";
            this.txtKillPassword.Size = new System.Drawing.Size(100, 22);
            this.txtKillPassword.TabIndex = 4;
            this.txtKillPassword.Text = "00 00 00 00";
            // 
            // cbPermaLock
            // 
            this.cbPermaLock.AutoSize = true;
            this.cbPermaLock.Location = new System.Drawing.Point(7, 61);
            this.cbPermaLock.Name = "cbPermaLock";
            this.cbPermaLock.Size = new System.Drawing.Size(105, 21);
            this.cbPermaLock.TabIndex = 6;
            this.cbPermaLock.Text = "Perma Lock";
            this.cbPermaLock.UseVisualStyleBackColor = true;
            // 
            // gbpOptions
            // 
            this.gbpOptions.Controls.Add(this.rbSpecify);
            this.gbpOptions.Controls.Add(this.rbFromFile);
            this.gbpOptions.Location = new System.Drawing.Point(221, 10);
            this.gbpOptions.Name = "gbpOptions";
            this.gbpOptions.Size = new System.Drawing.Size(200, 84);
            this.gbpOptions.TabIndex = 1;
            this.gbpOptions.TabStop = false;
            this.gbpOptions.Text = "Options";
            // 
            // rbSpecify
            // 
            this.rbSpecify.AutoSize = true;
            this.rbSpecify.Location = new System.Drawing.Point(13, 50);
            this.rbSpecify.Name = "rbSpecify";
            this.rbSpecify.Size = new System.Drawing.Size(75, 21);
            this.rbSpecify.TabIndex = 1;
            this.rbSpecify.TabStop = true;
            this.rbSpecify.Text = "Specify";
            this.rbSpecify.UseVisualStyleBackColor = true;
            this.rbSpecify.CheckedChanged += new System.EventHandler(this.rbSpecify_CheckedChanged);
            // 
            // rbFromFile
            // 
            this.rbFromFile.AutoSize = true;
            this.rbFromFile.Location = new System.Drawing.Point(13, 23);
            this.rbFromFile.Name = "rbFromFile";
            this.rbFromFile.Size = new System.Drawing.Size(118, 21);
            this.rbFromFile.TabIndex = 0;
            this.rbFromFile.TabStop = true;
            this.rbFromFile.Text = "From input file";
            this.rbFromFile.UseVisualStyleBackColor = true;
            this.rbFromFile.CheckedChanged += new System.EventHandler(this.rbFromFile_CheckedChanged);
            // 
            // cbLockTags
            // 
            this.cbLockTags.AutoSize = true;
            this.cbLockTags.Location = new System.Drawing.Point(7, 33);
            this.cbLockTags.Name = "cbLockTags";
            this.cbLockTags.Size = new System.Drawing.Size(96, 21);
            this.cbLockTags.TabIndex = 0;
            this.cbLockTags.Text = "Lock Tags";
            this.cbLockTags.UseVisualStyleBackColor = true;
            this.cbLockTags.CheckedChanged += new System.EventHandler(this.cbLockTags_CheckedChanged);
            // 
            // btnStartEncoding
            // 
            this.btnStartEncoding.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartEncoding.Location = new System.Drawing.Point(197, 279);
            this.btnStartEncoding.Name = "btnStartEncoding";
            this.btnStartEncoding.Size = new System.Drawing.Size(161, 37);
            this.btnStartEncoding.TabIndex = 4;
            this.btnStartEncoding.Text = "Start Encoding";
            this.btnStartEncoding.UseVisualStyleBackColor = true;
            this.btnStartEncoding.Click += new System.EventHandler(this.btnStartEncoding_Click);
            // 
            // btnStopEncoding
            // 
            this.btnStopEncoding.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopEncoding.Location = new System.Drawing.Point(425, 279);
            this.btnStopEncoding.Name = "btnStopEncoding";
            this.btnStopEncoding.Size = new System.Drawing.Size(161, 37);
            this.btnStopEncoding.TabIndex = 5;
            this.btnStopEncoding.Text = "Stop Encoding";
            this.btnStopEncoding.UseVisualStyleBackColor = true;
            this.btnStopEncoding.Click += new System.EventHandler(this.btnStopEncoding_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblProcessing);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Location = new System.Drawing.Point(13, 322);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(751, 199);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Status";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(501, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 46);
            this.label10.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(101, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 46);
            this.label9.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(237, 524);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(309, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Copyright 20201 - Ko-Aaham Technologies LLP";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(638, 524);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(132, 17);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "www.ko-aaham.com";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(256, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(271, 32);
            this.label8.TabIndex = 10;
            this.label8.Text = "K-Tag Encoder V1.0";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Location = new System.Drawing.Point(17, 524);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(46, 17);
            this.lblConnectionStatus.TabIndex = 11;
            this.lblConnectionStatus.Text = "label2";
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoEllipsis = true;
            this.lblProcessing.Location = new System.Drawing.Point(7, 22);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(738, 174);
            this.lblProcessing.TabIndex = 2;
            this.lblProcessing.Text = "label2";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnStopEncoding);
            this.Controls.Add(this.btnStartEncoding);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbpInputFile);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbpInputFile.ResumeLayout(false);
            this.gbpInputFile.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbpSetPassword.ResumeLayout(false);
            this.gbpSetPassword.PerformLayout();
            this.gbpOptions.ResumeLayout(false);
            this.gbpOptions.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbCopyTidToEpc;
        private System.Windows.Forms.RadioButton rbInputFile;
        private System.Windows.Forms.GroupBox gbpInputFile;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbPermaLock;
        private System.Windows.Forms.TextBox txtAccessPassword;
        private System.Windows.Forms.TextBox txtKillPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbpOptions;
        private System.Windows.Forms.RadioButton rbSpecify;
        private System.Windows.Forms.RadioButton rbFromFile;
        private System.Windows.Forms.CheckBox cbLockTags;
        private System.Windows.Forms.Button btnStartEncoding;
        private System.Windows.Forms.Button btnStopEncoding;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbpSetPassword;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblProcessing;
    }
}