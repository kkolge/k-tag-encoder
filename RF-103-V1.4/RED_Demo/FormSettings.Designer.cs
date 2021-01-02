namespace Phychips.Red
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStopConditions = new System.Windows.Forms.Button();
            this.buttonSpeakerBeep = new System.Windows.Forms.Button();
            this.buttonRssi = new System.Windows.Forms.Button();
            this.buttonEncoding = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelEncodingType = new Phychips.Red.CustLabel();
            this.toggleButtonRssi = new Phychips.Red.CustToggleButton();
            this.labelStopCondition = new Phychips.Red.CustLabel();
            this.toggleButtonSpeakerBeep = new Phychips.Red.CustToggleButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(130, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 29);
            this.label1.TabIndex = 92;
            this.label1.Text = "Settings";
            // 
            // buttonStopConditions
            // 
            this.buttonStopConditions.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonStopConditions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonStopConditions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonStopConditions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStopConditions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStopConditions.Location = new System.Drawing.Point(0, 54);
            this.buttonStopConditions.Name = "buttonStopConditions";
            this.buttonStopConditions.Size = new System.Drawing.Size(355, 39);
            this.buttonStopConditions.TabIndex = 105;
            this.buttonStopConditions.Text = "Stop Conditions";
            this.buttonStopConditions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStopConditions.UseVisualStyleBackColor = true;
            this.buttonStopConditions.Click += new System.EventHandler(this.buttonStopConditions_Click);
            // 
            // buttonSpeakerBeep
            // 
            this.buttonSpeakerBeep.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonSpeakerBeep.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonSpeakerBeep.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonSpeakerBeep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSpeakerBeep.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSpeakerBeep.Location = new System.Drawing.Point(0, 99);
            this.buttonSpeakerBeep.Name = "buttonSpeakerBeep";
            this.buttonSpeakerBeep.Size = new System.Drawing.Size(355, 39);
            this.buttonSpeakerBeep.TabIndex = 109;
            this.buttonSpeakerBeep.Text = "Speaker Beep";
            this.buttonSpeakerBeep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSpeakerBeep.UseVisualStyleBackColor = true;
            this.buttonSpeakerBeep.Click += new System.EventHandler(this.buttonSpeakerBeep_Click);
            // 
            // buttonRssi
            // 
            this.buttonRssi.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonRssi.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonRssi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonRssi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRssi.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRssi.Location = new System.Drawing.Point(0, 144);
            this.buttonRssi.Name = "buttonRssi";
            this.buttonRssi.Size = new System.Drawing.Size(355, 39);
            this.buttonRssi.TabIndex = 116;
            this.buttonRssi.Text = "Display Tag RSSI";
            this.buttonRssi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRssi.UseVisualStyleBackColor = true;
            this.buttonRssi.Click += new System.EventHandler(this.buttonRssi_Click);
            // 
            // buttonEncoding
            // 
            this.buttonEncoding.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonEncoding.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonEncoding.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonEncoding.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEncoding.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEncoding.Location = new System.Drawing.Point(0, 189);
            this.buttonEncoding.Name = "buttonEncoding";
            this.buttonEncoding.Size = new System.Drawing.Size(355, 39);
            this.buttonEncoding.TabIndex = 120;
            this.buttonEncoding.Text = "Encoding Type";
            this.buttonEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEncoding.UseVisualStyleBackColor = true;
            this.buttonEncoding.Click += new System.EventHandler(this.buttonEncoding_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.BackgroundImage = global::Phychips.Red.Properties.Resources.nav_btn_back;
            this.buttonBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonBack.FlatAppearance.BorderSize = 0;
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(131)))), ((int)(((byte)(255)))));
            this.buttonBack.Location = new System.Drawing.Point(4, 5);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(79, 33);
            this.buttonBack.TabIndex = 100;
            this.buttonBack.Text = "More";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(355, 437);
            this.label2.TabIndex = 140;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEncodingType
            // 
            this.labelEncodingType.BackColor = System.Drawing.Color.Transparent;
            this.labelEncodingType.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEncodingType.ForeColor = System.Drawing.Color.SteelBlue;
            this.labelEncodingType.Image = ((System.Drawing.Image)(resources.GetObject("labelEncodingType.Image")));
            this.labelEncodingType.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelEncodingType.Location = new System.Drawing.Point(193, 196);
            this.labelEncodingType.Name = "labelEncodingType";
            this.labelEncodingType.Size = new System.Drawing.Size(150, 25);
            this.labelEncodingType.TabIndex = 139;
            this.labelEncodingType.Text = "-  ";
            this.labelEncodingType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toggleButtonRssi
            // 
            this.toggleButtonRssi.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toggleButtonRssi.BackgroundImage")));
            this.toggleButtonRssi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toggleButtonRssi.Checked = false;
            this.toggleButtonRssi.FlatAppearance.BorderSize = 0;
            this.toggleButtonRssi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleButtonRssi.Location = new System.Drawing.Point(298, 147);
            this.toggleButtonRssi.Name = "toggleButtonRssi";
            this.toggleButtonRssi.Size = new System.Drawing.Size(45, 32);
            this.toggleButtonRssi.TabIndex = 134;
            this.toggleButtonRssi.UseVisualStyleBackColor = true;
            this.toggleButtonRssi.Click += new System.EventHandler(this.toggleButtonRssi_Click);
            // 
            // labelStopCondition
            // 
            this.labelStopCondition.BackColor = System.Drawing.Color.Transparent;
            this.labelStopCondition.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStopCondition.ForeColor = System.Drawing.Color.SteelBlue;
            this.labelStopCondition.Image = ((System.Drawing.Image)(resources.GetObject("labelStopCondition.Image")));
            this.labelStopCondition.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelStopCondition.Location = new System.Drawing.Point(193, 61);
            this.labelStopCondition.Name = "labelStopCondition";
            this.labelStopCondition.Size = new System.Drawing.Size(150, 25);
            this.labelStopCondition.TabIndex = 137;
            this.labelStopCondition.Text = "-  ";
            this.labelStopCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toggleButtonSpeakerBeep
            // 
            this.toggleButtonSpeakerBeep.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toggleButtonSpeakerBeep.BackgroundImage")));
            this.toggleButtonSpeakerBeep.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toggleButtonSpeakerBeep.Checked = false;
            this.toggleButtonSpeakerBeep.FlatAppearance.BorderSize = 0;
            this.toggleButtonSpeakerBeep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleButtonSpeakerBeep.Location = new System.Drawing.Point(298, 102);
            this.toggleButtonSpeakerBeep.Name = "toggleButtonSpeakerBeep";
            this.toggleButtonSpeakerBeep.Size = new System.Drawing.Size(45, 32);
            this.toggleButtonSpeakerBeep.TabIndex = 133;
            this.toggleButtonSpeakerBeep.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(355, 476);
            this.Controls.Add(this.labelEncodingType);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.toggleButtonRssi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelStopCondition);
            this.Controls.Add(this.buttonRssi);
            this.Controls.Add(this.buttonStopConditions);
            this.Controls.Add(this.toggleButtonSpeakerBeep);
            this.Controls.Add(this.buttonEncoding);
            this.Controls.Add(this.buttonSpeakerBeep);
            this.Controls.Add(this.label2);
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.Text = "RED DEMO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.VisibleChanged += new System.EventHandler(this.Form_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonStopConditions;
        private System.Windows.Forms.Button buttonSpeakerBeep;
        private System.Windows.Forms.Button buttonRssi;
        private System.Windows.Forms.Button buttonEncoding;
        private CustLabel labelEncodingType;
        private CustToggleButton toggleButtonRssi;
        private CustLabel labelStopCondition;
        private CustToggleButton toggleButtonSpeakerBeep;
        private System.Windows.Forms.Label label2;
    }
}