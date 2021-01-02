
namespace TagEncoderV1
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.btnStartRead = new System.Windows.Forms.Button();
            this.btnStopRead = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 424);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection Status";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Location = new System.Drawing.Point(142, 424);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(128, 17);
            this.lblConnectionStatus.TabIndex = 1;
            this.lblConnectionStatus.Text = "NOT CONNECTED";
            // 
            // btnStartRead
            // 
            this.btnStartRead.Location = new System.Drawing.Point(12, 12);
            this.btnStartRead.Name = "btnStartRead";
            this.btnStartRead.Size = new System.Drawing.Size(86, 23);
            this.btnStartRead.TabIndex = 2;
            this.btnStartRead.Text = "Start Read";
            this.btnStartRead.UseVisualStyleBackColor = true;
            this.btnStartRead.Click += new System.EventHandler(this.btnStartRead_Click);
            // 
            // btnStopRead
            // 
            this.btnStopRead.Location = new System.Drawing.Point(104, 12);
            this.btnStopRead.Name = "btnStopRead";
            this.btnStopRead.Size = new System.Drawing.Size(89, 23);
            this.btnStopRead.TabIndex = 3;
            this.btnStopRead.Text = "Stop Read";
            this.btnStopRead.UseVisualStyleBackColor = true;
            this.btnStopRead.Click += new System.EventHandler(this.btnStopRead_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnStopRead);
            this.Controls.Add(this.btnStartRead);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Button btnStartRead;
        private System.Windows.Forms.Button btnStopRead;
    }
}

