using Phychips.Helper;
using Phychips.Rcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Phychips.Red
{
    public partial class FormSettings : Form, IRcpEvent2
    {
        private string pad = "   ";

        public FormSettings()
        {
            InitializeComponent();
        }
        
        private void Form_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Font = new Font("Arial", 10);              
        }

        private void Form_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.StartPosition = FormStartPosition.Manual;            
                onResume();
            }
        }
        
        private void onResume()
        {
            RcpApi2.Instance.setOnRcpEventListener(this);            

            RcpApi2.Instance.getReaderInfo(0xB0);

            this.labelStopCondition.Text
                = Properties.Settings.Default.max_tag.ToString() + "/"
                + Properties.Settings.Default.max_time.ToString() + "/"
                + Properties.Settings.Default.repeat_cycle.ToString() + pad;

            this.toggleButtonSpeakerBeep.Checked = Properties.Settings.Default.speakerBeep;
            this.toggleButtonRssi.Checked = Properties.Settings.Default.displayRSSI;

            this.labelEncodingType.Text = EpcConverter.toTypeString(Properties.Settings.Default.encodingType) + pad;        
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();

            var form = (Form)Tag;
            form.Show();                         
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void onPortOpened(string port)
        {
            //throw new NotImplementedException();
        }

        public void onPortClosed(string port)
        {
            //throw new NotImplementedException();
            this.Close();
        }

        public void onPlugged(bool plug, string port)
        {
            throw new NotImplementedException();
        }

        public void onSuccessReceived(byte[] data, int cmdCode)
        {
            //throw new NotImplementedException();
        }

        public void onFailureReceived(byte[] errCode)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    onFailureReceived(errCode);
                }));
                return;
            }

            MessageBox.Show("Error code: " + new ByteBuilder(errCode).ToString());
        }

        public void onReaderInfoReceived(byte[] info)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    onReaderInfoReceived(info);
                }));
                return;
            }
        }

        public void onRegionReceived(int region)
        {
            throw new NotImplementedException();
        }

        public void onSelectParamReceived(int target, int action, int memBank, long pointer, int length, int truncate, byte[] mask)
        {
            throw new NotImplementedException();
        }

        public void onQueryParamReceived(int dr, int m, int trext, int sel, int session, int target, int q)
        {
            throw new NotImplementedException();
        }

        public void onChannelReceived(int ch, int chOffset)
        {
            throw new NotImplementedException();
        }

        public void onFhLbtParamReceived(int rTime, int iTime, int csTime, int rfLevel, int fh, int lbt, int cw)
        {
            throw new NotImplementedException();
        }

        public void onTxPowerLevelReceived(int currPower, int minPower, int maxPower)
        {
            throw new NotImplementedException();
        }

        public void onTagMemoryReceived(int wordCnt, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void onTagMemoryLongReceived(int rspType, int startAddr, int wordCnt, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void onSessionReceived(int session)
        {
            throw new NotImplementedException();
        }

        public void onFHTableReceived(int tblSize, byte[] table)
        {
            throw new NotImplementedException();
        }

        public void onModulationReceived(int blf, int rxMod, int dr)
        {
            throw new NotImplementedException();
        }

        public void onAntiColModeReceived(int mode, int qStart, int qMax, int qMin)
        {
            throw new NotImplementedException();
        }

        public void onTagReceived(byte[] pcEpc)
        {
            throw new NotImplementedException();
        }

        public void onTagWithRssiReceived(byte[] pcEpc, int rssi)
        {
            throw new NotImplementedException();
        }

        public void onTagWithTidReceived(byte[] pcEpc, byte[] tid)
        {
            throw new NotImplementedException();
        }

        public void onFHModeReceived(int mode)
        {
            throw new NotImplementedException();
        }

        public void onFHModeRefLevelReceived(int refLevel)
        {
            throw new NotImplementedException();
        }


// >> 20180611, HYO
        public void onNativeReceived(byte[] packet)
        {
            //throw new NotImplementedException();
        }
// << 20180611, HYO
// >> 20200121, KK
        public void onGetReaderSerial(byte[] packet)
        {
            //throw new NotImplementedException();
        }
// << 20200121, KK


        private void buttonStopConditions_Click(object sender, EventArgs e)
        {
            FormStopCondition form = new FormStopCondition();
            form.Tag = this;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X, this.Location.Y);

            form.Show();
            this.Hide();
        }

        private void labelStopCondition_Click(object sender, EventArgs e)
        {
            buttonStopConditions.PerformClick();
        }
        
        private void buttonEncoding_Click(object sender, EventArgs e)
        {
            FormEncodingType form = new FormEncodingType();
            form.Tag = this;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X, this.Location.Y);

            form.Show();
            this.Hide();
        }

        private void labelEncodingType_Click(object sender, EventArgs e)
        {
            buttonEncoding.PerformClick();
        }

        private void toggleButtonSpeakerBeep_Click(object sender, EventArgs e)
        {
            if(this.toggleButtonSpeakerBeep.Checked)
            {
                Properties.Settings.Default.speakerBeep = true;
            }
            else
            {
                Properties.Settings.Default.speakerBeep = false;
            }
        }

        private void buttonSpeakerBeep_Click(object sender, EventArgs e)
        {
            toggleButtonSpeakerBeep.PerformClick();
        }

        private void toggleButtonRssi_Click(object sender, EventArgs e)
        {
            if (this.toggleButtonRssi.Checked)
            {
                Properties.Settings.Default.displayRSSI = true;
            }
            else
            {
                Properties.Settings.Default.displayRSSI = false;
            }
        }

        private void buttonRssi_Click(object sender, EventArgs e)
        {
            toggleButtonRssi.PerformClick();
        }
    }
}
