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
    public partial class FormKill : Form, IRcpEvent2
    {
        private TagVO target;

        public TagVO Target
        {
            get { return target; }
            set { target = value; }
        }

        public FormKill()
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
            this.textBoxTarget.Text = new ByteBuilder(target.Epc).ToString();
            RcpApi2.Instance.setOnRcpEventListener(this);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            var form = (Form)Tag;
            form.Show();                         
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {               
            long pw = 0;
            			            
            try
            {
                pw = Convert.ToUInt32(textBoxAccessPassword.Text, 16);                                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RcpApi2.Instance.killTag(pw, target.Epc, 0);            
        }



        public void onPortOpened(string port)
        {
            //throw new NotImplementedException();
        }

        public void onPortClosed(string port)
        {
            //throw new NotImplementedException();
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
            throw new NotImplementedException();
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


    }
}
