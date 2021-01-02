namespace Phychips.Driver
{
    partial class FormSioConfig
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSioConfig));
            this.comboBox_UARTConfiguration_Baudrate = new System.Windows.Forms.ComboBox();
            this.label_UARTConfiguration_Baudrate = new System.Windows.Forms.Label();
            this.label_UARTConfiguration_Port = new System.Windows.Forms.Label();
            this.label_UARTConfiguration_Parity = new System.Windows.Forms.Label();
            this.label_UARTConfiguration_DataBits = new System.Windows.Forms.Label();
            this.comboBox_UARTConfiguration_Port = new System.Windows.Forms.ComboBox();
            this.comboBox_UARTConfiguration_DataBits = new System.Windows.Forms.ComboBox();
            this.comboBox_UARTConfiguration_Parity = new System.Windows.Forms.ComboBox();
            this.comboBox_UARTConfiguration_StopBits = new System.Windows.Forms.ComboBox();
            this.label_UARTConfiguration_StopBits = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_UARTConfiguration_Baudrate
            // 
            this.comboBox_UARTConfiguration_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_UARTConfiguration_Baudrate.Items.AddRange(new object[] {
            "75",
            "110",
            "134",
            "150",
            "300",
            "600",
            "1200",
            "1800",
            "2400",
            "4800",
            "7200",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "128000"});
            this.comboBox_UARTConfiguration_Baudrate.Location = new System.Drawing.Point(93, 65);
            this.comboBox_UARTConfiguration_Baudrate.Name = "comboBox_UARTConfiguration_Baudrate";
            this.comboBox_UARTConfiguration_Baudrate.Size = new System.Drawing.Size(77, 21);
            this.comboBox_UARTConfiguration_Baudrate.TabIndex = 2;
            this.comboBox_UARTConfiguration_Baudrate.SelectedIndexChanged += new System.EventHandler(this.comboBox_UARTConfiguration_Baudrate_SelectedIndexChanged);
            // 
            // label_UARTConfiguration_Baudrate
            // 
            this.label_UARTConfiguration_Baudrate.Location = new System.Drawing.Point(18, 67);
            this.label_UARTConfiguration_Baudrate.Name = "label_UARTConfiguration_Baudrate";
            this.label_UARTConfiguration_Baudrate.Size = new System.Drawing.Size(60, 18);
            this.label_UARTConfiguration_Baudrate.TabIndex = 13;
            this.label_UARTConfiguration_Baudrate.Text = "Baud";
            this.label_UARTConfiguration_Baudrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_UARTConfiguration_Baudrate.UseMnemonic = false;
            // 
            // label_UARTConfiguration_Port
            // 
            this.label_UARTConfiguration_Port.Location = new System.Drawing.Point(18, 31);
            this.label_UARTConfiguration_Port.Name = "label_UARTConfiguration_Port";
            this.label_UARTConfiguration_Port.Size = new System.Drawing.Size(60, 18);
            this.label_UARTConfiguration_Port.TabIndex = 12;
            this.label_UARTConfiguration_Port.Text = "Port";
            this.label_UARTConfiguration_Port.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_UARTConfiguration_Parity
            // 
            this.label_UARTConfiguration_Parity.Location = new System.Drawing.Point(18, 138);
            this.label_UARTConfiguration_Parity.Name = "label_UARTConfiguration_Parity";
            this.label_UARTConfiguration_Parity.Size = new System.Drawing.Size(60, 18);
            this.label_UARTConfiguration_Parity.TabIndex = 10;
            this.label_UARTConfiguration_Parity.Text = "Parity";
            this.label_UARTConfiguration_Parity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_UARTConfiguration_DataBits
            // 
            this.label_UARTConfiguration_DataBits.Location = new System.Drawing.Point(18, 103);
            this.label_UARTConfiguration_DataBits.Name = "label_UARTConfiguration_DataBits";
            this.label_UARTConfiguration_DataBits.Size = new System.Drawing.Size(60, 18);
            this.label_UARTConfiguration_DataBits.TabIndex = 9;
            this.label_UARTConfiguration_DataBits.Text = "DataBits";
            this.label_UARTConfiguration_DataBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox_UARTConfiguration_Port
            //             
            this.comboBox_UARTConfiguration_Port.Location = new System.Drawing.Point(93, 29);
            this.comboBox_UARTConfiguration_Port.Name = "comboBox_UARTConfiguration_Port";
            this.comboBox_UARTConfiguration_Port.Size = new System.Drawing.Size(77, 21);
            this.comboBox_UARTConfiguration_Port.TabIndex = 0;
            this.comboBox_UARTConfiguration_Port.SelectedIndexChanged += new System.EventHandler(this.comboBox_UARTConfiguration_Port_SelectedIndexChanged);
            // 
            // comboBox_UARTConfiguration_DataBits
            // 
            this.comboBox_UARTConfiguration_DataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_UARTConfiguration_DataBits.Items.AddRange(new object[] {
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.comboBox_UARTConfiguration_DataBits.Location = new System.Drawing.Point(93, 101);
            this.comboBox_UARTConfiguration_DataBits.Name = "comboBox_UARTConfiguration_DataBits";
            this.comboBox_UARTConfiguration_DataBits.Size = new System.Drawing.Size(76, 21);
            this.comboBox_UARTConfiguration_DataBits.TabIndex = 6;
            this.comboBox_UARTConfiguration_DataBits.SelectedIndexChanged += new System.EventHandler(this.comboBox_UARTConfiguration_DataBits_SelectedIndexChanged);
            // 
            // comboBox_UARTConfiguration_Parity
            // 
            this.comboBox_UARTConfiguration_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_UARTConfiguration_Parity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.comboBox_UARTConfiguration_Parity.Location = new System.Drawing.Point(94, 137);
            this.comboBox_UARTConfiguration_Parity.Name = "comboBox_UARTConfiguration_Parity";
            this.comboBox_UARTConfiguration_Parity.Size = new System.Drawing.Size(76, 21);
            this.comboBox_UARTConfiguration_Parity.TabIndex = 4;
            this.comboBox_UARTConfiguration_Parity.SelectedIndexChanged += new System.EventHandler(this.comboBox_UARTConfiguration_Parity_SelectedIndexChanged);
            // 
            // comboBox_UARTConfiguration_StopBits
            // 
            this.comboBox_UARTConfiguration_StopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_UARTConfiguration_StopBits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.comboBox_UARTConfiguration_StopBits.Location = new System.Drawing.Point(92, 173);
            this.comboBox_UARTConfiguration_StopBits.Name = "comboBox_UARTConfiguration_StopBits";
            this.comboBox_UARTConfiguration_StopBits.Size = new System.Drawing.Size(77, 21);
            this.comboBox_UARTConfiguration_StopBits.TabIndex = 8;
            this.comboBox_UARTConfiguration_StopBits.SelectedIndexChanged += new System.EventHandler(this.comboBox_UARTConfiguration_StopBits_SelectedIndexChanged);
            // 
            // label_UARTConfiguration_StopBits
            // 
            this.label_UARTConfiguration_StopBits.Location = new System.Drawing.Point(18, 173);
            this.label_UARTConfiguration_StopBits.Name = "label_UARTConfiguration_StopBits";
            this.label_UARTConfiguration_StopBits.Size = new System.Drawing.Size(60, 18);
            this.label_UARTConfiguration_StopBits.TabIndex = 11;
            this.label_UARTConfiguration_StopBits.Text = "StopBits";
            this.label_UARTConfiguration_StopBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(69, 238);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 28);
            this.button1.TabIndex = 14;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_UARTConfiguration_Port);
            this.groupBox1.Controls.Add(this.label_UARTConfiguration_Port);
            this.groupBox1.Controls.Add(this.label_UARTConfiguration_Baudrate);
            this.groupBox1.Controls.Add(this.comboBox_UARTConfiguration_Baudrate);
            this.groupBox1.Controls.Add(this.label_UARTConfiguration_StopBits);
            this.groupBox1.Controls.Add(this.comboBox_UARTConfiguration_DataBits);
            this.groupBox1.Controls.Add(this.comboBox_UARTConfiguration_StopBits);
            this.groupBox1.Controls.Add(this.label_UARTConfiguration_DataBits);
            this.groupBox1.Controls.Add(this.label_UARTConfiguration_Parity);
            this.groupBox1.Controls.Add(this.comboBox_UARTConfiguration_Parity);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 220);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Port";
            // 
            // FormSioConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(222, 279);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSioConfig";
            this.Text = "FormSioConfig";
            this.Load += new System.EventHandler(this.FormSioConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_UARTConfiguration_Baudrate;
        private System.Windows.Forms.Label label_UARTConfiguration_Baudrate;
        private System.Windows.Forms.Label label_UARTConfiguration_Port;
        private System.Windows.Forms.Label label_UARTConfiguration_Parity;
        private System.Windows.Forms.Label label_UARTConfiguration_DataBits;
        private System.Windows.Forms.ComboBox comboBox_UARTConfiguration_Port;
        private System.Windows.Forms.ComboBox comboBox_UARTConfiguration_DataBits;
        private System.Windows.Forms.ComboBox comboBox_UARTConfiguration_Parity;
        private System.Windows.Forms.ComboBox comboBox_UARTConfiguration_StopBits;
        private System.Windows.Forms.Label label_UARTConfiguration_StopBits;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}