using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phychips.Helper;
using Phychips.Rcp;
using Phychips.Red;

namespace TagEncoderV1
{
    public partial class Form1 : Form, IRcpEvent2 
    {

        private int nTagCnt = 0;
        private bool plugged = false;

        public int max_tag = 0;
        public int max_time = 0;
        public int repeat_cycle = 0;
        public bool speakerBeep = false;
        public bool readAfterPlugging = false;
        public bool displayRssi = false;
        public int encoding_type = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void onResume()
        {
            RcpApi2.Instance.setOnRcpEventListener(this);

            max_tag = Phychips.Red.Properties.Settings.Default.max_tag;
            max_time = Phychips.Red.Properties.Settings.Default.max_time;
            repeat_cycle = Phychips.Red.Properties.Settings.Default.repeat_cycle;
            speakerBeep = Phychips.Red.Properties.Settings.Default.speakerBeep;
            readAfterPlugging = Phychips.Red.Properties.Settings.Default.readAfterPlugging;

            bool newDisplayRssi = Phychips.Red.Properties.Settings.Default.displayRSSI;
            int new_encoding_type = Phychips.Red.Properties.Settings.Default.encodingType;

            if (this.displayRssi != newDisplayRssi || this.encoding_type != new_encoding_type)
            {
                this.displayRssi = newDisplayRssi;
                this.encoding_type = new_encoding_type;
                resetUI();
            }

            if (RcpApi2.Instance.isOpened())
            {
                plugged = true;
            }
            else if (RcpApi2.Instance.isOpenable())
            {
                plugged = true;
                if (RcpApi2.Instance.open())
                {
                    this.lblConnectionStatus.Text = "CONNECTED";
                    this.lblConnectionStatus.ForeColor = Color.Green;
                }
            }
            else
            {
                plugged = false;
            }

            diplayPlug();
        }

        private void diplayPlug()
        {
            if (plugged)
            {
                this.lblConnectionStatus.Text = "CONNECTED";
                this.lblConnectionStatus.ForeColor = Color.Green;
            }
            else
            {
                this.lblConnectionStatus.Text = "NOT CONNECTED";
                this.lblConnectionStatus.ForeColor = Color.Red;
            }
        }

        public void onAntiColModeReceived(int mode, int qStart, int qMax, int qMin)
        {
            //Not Implemented
        }
        public void onChannelReceived(int ch, int chOffset)
        {
            //Not Implemented
        }
        public void onFailureReceived(byte[] errCode)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    onFailureReceived(errCode);
                }));
                return;
            }

            MessageBox.Show("Error code: " + new ByteBuilder(errCode).ToString());
        }
        public void onFhLbtParamReceived(int rTime, int iTime, int csTime, int rfLevel, int fh, int lbt, int cw)
        {
            //Not Implemented
        }
        public void onFHModeReceived(int mode)
        {
            //Not Implemented
        }
        public void onFHModeRefLevelReceived(int refLevel)
        {

        }
        public void onFHTableReceived(int tblSize, byte[] table)
        {
            //Not Implemented
        }
        public void onGetReaderSerial(byte[] packet)
        {
            //Not Implemented
        }
        public void onModulationReceived(int blf, int rxMod, int dr)
        {
            //Not Implemented
        }
        public void onNativeReceived(byte[] packet)
        {
            //Not Implemented
        }
        public void onPlugged(bool plug, string port)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    onPlugged(plug, port);
                }));
                return;
            }

            //System.Console.WriteLine("Plug = " + plug + " Port = " + port);

            if (this.readAfterPlugging && plug)
            {
                System.Threading.Thread.Sleep(1000);

                if (RcpApi2.Instance.open())
                {
                    plugged = true;
                    //this.toggleButton.Checked = true;

                    System.Threading.Thread.Sleep(100);

                    if (!displayRssi)
                        RcpApi2.Instance.startReadTags(max_tag, max_time, repeat_cycle);
                    else
                        RcpApi2.Instance.startReadTagsWithRssi(max_tag, max_time, repeat_cycle);
                }
            }

            if (!plug)
            {
                if (RcpApi2.Instance.isOpened())
                {
                    RcpApi2.Instance.close();
                }
                //this.toggleButton.Checked = false;
                plugged = false;
            }

            plugged = plug;
            diplayPlug();
        }
        public void onQueryParamReceived(int dr, int m, int trext, int sel, int session, int target, int q)
        {
            //Not Implemented
        }
        public void onReaderInfoReceived(byte[] info)
        {
            //Not Implemented
        }
        public void onRegionReceived(int region)
        {
            //Not Implemented
        }
        public void onSelectParamReceived(int target, int action, int memBank, long pointer, int length, int truncate, byte[] mask)
        {
            //Not Implemented
        }
        public void onSessionReceived(int session)
        {
            //Not Implemented
        }
        public void onSuccessReceived(byte[] data, int cmdCode)
        {
            //Not Implemented
        }
        public void onTagMemoryLongReceived(int rspType, int startAddr, int wordCnt, byte[] data)
        {
            //Not Implemented
        }
        public void onTagMemoryReceived(int wordCnt, byte[] data)
        {
            //Not Implemented
        }
        public void onTagReceived(byte[] pcEpc)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    onTagReceived(pcEpc);
                }));
                return;
            }

            
            int epcStart = this.getEpcStartPos(pcEpc);
            
            StringBuilder hsb = new StringBuilder();

            for (int i = 0; i < pcEpc.Length; i++)
            {
                try
                {
                    //hsb.Append(pcEpc[i].ToString("X2") + " ");
                    hsb.Append(pcEpc[i].ToString("X02"));
                }
                catch
                {
                    break;
                }
            }

            string timeStamp = DateTime.Now.ToString("G");

            string str = "";

            if (encoding_type == EpcConverter.HEX_STRING)
            {
                str = hsb.ToString().Substring(0, epcStart * 2);
            }
            else
            {
                 str = EpcConverter.toString(encoding_type, pcEpc);
            }

            
            
            {
                byte[] epc = new byte[pcEpc.Length - epcStart];
                Array.Copy(pcEpc, epcStart, epc, 0, epc.Length);
                str += EpcConverter.toString(encoding_type, epc);
            }
            //Console.WriteLine("Tag: " + str + " read at: " + timeStamp);


        }
        public void onTagWithRssiReceived(byte[] pcEpc, int rssi)
        {
            //Not Implemented
        }
        public void onTagWithTidReceived(byte[] pcEpc, byte[] tid)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    onTagWithTidReceived(pcEpc, tid);
                }));
                return;
            }
            //Read the tag. Now stop reading and process the tag data
            //RcpApi2.Instance.stopReadTags();

            int epcStart = this.getEpcStartPos(pcEpc);

            StringBuilder hsb = new StringBuilder();

            hsb.Append("PC: ");
            
            for (int i=0; i < epcStart; i++)
            {
                try
                {
                    hsb.Append(pcEpc[i].ToString("X02"));
                }
                catch
                {
                    Console.WriteLine("Error in processing PC bits");
                    break;
                }
            }

            hsb.Append("  EPC: ");
            for (int i = epcStart; i < pcEpc.Length; i++)
            {
                try
                {
                    hsb.Append(pcEpc[i].ToString("X02"));
                }
                catch
                {
                    Console.WriteLine("Error in capturing EPC");
                    break;
                }
            }
            hsb.Append("  TID:");
            for(int i=0; i<tid.Length; i++)
            {
                try
                {
                    hsb.Append(tid[i].ToString("X02"));
                }
                catch
                {
                    Console.WriteLine("Error in capturing TID");
                    break;
                }
            }

            string timeStamp = DateTime.Now.ToString("G");
            Console.WriteLine("Tag: " + hsb.ToString() + " read at: " + timeStamp);
            
            {
                byte[] pc = new byte[epcStart];
                byte[] epc = new byte[pcEpc.Length - epcStart];
                Array.Copy(pcEpc, 0, pc, 0, epcStart);
                Array.Copy(pcEpc, epcStart, epc, 0, epc.Length);
            }

            //manipulating the EPC ID by adding 0x01 to the last hex
            byte[] newEpc = { 0xE2, 0x00, 0x00, 0x19, 0x59, 0x03, 0x02, 0x42, 0x23, 0x80, 0x27, 0x15 };//new byte[pcEpc.Length - epcStart];
            //changing the last part of the hex
            try
            {
                //newEpc[pcEpc.Length - epcStart - 1] += 0x01;
                //RcpApi2.Instance.
                RcpApi2.Instance.blockwriteToTagMemory_MaskTID(0x00000000, tid, 1, 2, newEpc);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception type: " + ex.GetType() + " Message: " + ex.Message);
            }


            //RcpApi2.Instance.lockTagMemory_MaskTID();
            //RcpApi2.Instance.blockwriteToTagMemory_MaskTID()

            


        }
        public void onTxPowerLevelReceived(int currPower, int minPower, int maxPower)
        {
            //Not Implemented
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //nothing to be done
        }

        private void resetUI()
        {
            nTagCnt = 0;
            //setTagCount();
            //listTagVO.Clear();
            //this.listViewEPC.Items.Clear();
        }

        private int getEpcStartPos(byte[] pcEpc)
        {
            int epcStart = 2;

            if ((pcEpc[0] & 0x02) == 0x02)
            {
                if ((pcEpc[0] & 0x08) == 0x08)
                {
                    epcStart = 6; // PC + XPC_W1 + XPC_W2
                }
                else
                {

                    epcStart = 4; // PC + XPC_W1
                }
            }

            return epcStart;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            onResume();
        }

        private void btnStartRead_Click(object sender, EventArgs e)
        {
            if (!RcpApi2.Instance.isOpened())
            {
                if (RcpApi2.Instance.isOpenable())
                {
                    if (!RcpApi2.Instance.open())
                    {
                        MessageBox.Show("Cannot open the reader");
                    }
                }
            }
            else
            {
                RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
            }
        }

        private void btnStopRead_Click(object sender, EventArgs e)
        {
            RcpApi2.Instance.stopReadTags();
        }
    }
}
