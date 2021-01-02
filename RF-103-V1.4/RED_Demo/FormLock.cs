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
    public partial class FormLock : Form, IRcpEvent2
    {
        private TagVO target;

        public TagVO Target
        {
            get { return target; }
            set { target = value; }
        }

        public FormLock()
        {
            InitializeComponent();
        }

        private int seed = 0;
        private int memory = 0;
        
        public int Seed
        {
            get { return seed; }
            set 
            { 
                switch(value)
                {
                    case 0:
                        custSegButtonUnlock.Checked = true;
                        custSegButtonPUnlock.Checked = false;
                        custSegButtonLock.Checked = false;
                        custSegButtonPLock.Checked = false;                        
                        break;
                    case 1:
                        custSegButtonUnlock.Checked = false;
                        custSegButtonPUnlock.Checked = true;
                        custSegButtonLock.Checked = false;
                        custSegButtonPLock.Checked = false;                          
                        break;
                    case 2:
                        custSegButtonUnlock.Checked = false;
                        custSegButtonPUnlock.Checked = false;
                        custSegButtonLock.Checked = true;
                        custSegButtonPLock.Checked = false;
                        break;
                    case 3:
                        custSegButtonUnlock.Checked = false;
                        custSegButtonPUnlock.Checked = false;
                        custSegButtonLock.Checked = false;
                        custSegButtonPLock.Checked = true;
                        break;
                }
                seed = value; 
            }
        }

        public int Memory
        {
            get { return memory; }
            set 
            { 
                switch(value)
                {
                    case 0:
                        this.custSegButtonKill.Checked = true;
                        this.custSegButtonAcs.Checked = false;
                        this.custSegButtonEPC.Checked = false;
                        this.custSegButtonTID.Checked = false;
                        this.custSegButtonUSER.Checked = false;
                        break;
                    case 1:
                        this.custSegButtonKill.Checked = false;
                        this.custSegButtonAcs.Checked = true;
                        this.custSegButtonEPC.Checked = false;
                        this.custSegButtonTID.Checked = false;
                        this.custSegButtonUSER.Checked = false;
                        break;
                    case 2:
                        this.custSegButtonKill.Checked = false;
                        this.custSegButtonAcs.Checked = false;
                        this.custSegButtonEPC.Checked = true;
                        this.custSegButtonTID.Checked = false;
                        this.custSegButtonUSER.Checked = false;
                        break;
                    case 3:
                        this.custSegButtonKill.Checked = false;
                        this.custSegButtonAcs.Checked = false;
                        this.custSegButtonEPC.Checked = false;
                        this.custSegButtonTID.Checked = true;
                        this.custSegButtonUSER.Checked = false;
                        break;
                    case 4:
                        this.custSegButtonKill.Checked = false;
                        this.custSegButtonAcs.Checked = false;
                        this.custSegButtonEPC.Checked = false;
                        this.custSegButtonTID.Checked = false;
                        this.custSegButtonUSER.Checked = true;
                        break;
                }
                memory = value; 
            }
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

        private void custSegButtonKill_Click(object sender, EventArgs e)
        {
            this.Memory = 0;
        }

        private void custSegButtonAcs_Click(object sender, EventArgs e)
        {
            this.Memory = 1;
        }
        
        private void custSegButtonEPC_Click(object sender, EventArgs e)
        {
            this.Memory = 2;
        }

        private void custSegButtonTID_Click(object sender, EventArgs e)
        {
            this.Memory = 3;
        }

        private void custSegButtonUSER_Click(object sender, EventArgs e)
        {
            this.Memory = 4;
        }

        private void custSegButtonUnlock_Click(object sender, EventArgs e)
        {
            this.Seed = 0;
        }

        private void custSegButtonPUnlock_Click(object sender, EventArgs e)
        {
            this.Seed = 1;
        }

        private void custSegButtonLock_Click(object sender, EventArgs e)
        {
            this.Seed = 2;
        }

        private void custSegButtonPLock_Click(object sender, EventArgs e)
        {
            this.Seed = 3;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {   
            int lockdata = 0;
            long ap = 0;

            switch(memory)
			{
			    case 0:
				lockdata = (seed << 8) | (3 << 18);
				break;
			    case 1:
				lockdata = (seed << 6) | (3 << 16);
				break;
			    case 2:
				lockdata = (seed << 4) | (3 << 14);
				break;
			    case 3:
				lockdata = (seed << 2) | (3 << 12);
				break;
			    case 4:
                lockdata = (seed << 0) | (3 << 10);
				break;
			}
			            
            try
            {            
                ap = Convert.ToUInt32(textBoxAccessPassword.Text, 16);                                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RcpApi2.Instance.lockTagMemory(ap, target.Epc, lockdata);            
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
