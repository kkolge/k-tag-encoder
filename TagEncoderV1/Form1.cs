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
using System.Collections;
using System.IO;

namespace TagEncoderV1
{
    public partial class Form1 : Form, IRcpEvent2
    {
        //For Logging
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int lvItemCount = 0;
        private int nTagCnt = 0;
        private int totalProcessed = 0;
        private int totalError = 0;
        private bool plugged = false;

        public int max_tag = 0;
        public int max_time = 0;
        public int repeat_cycle = 0;
        public bool speakerBeep = false;
        public bool readAfterPlugging = false;
        public bool displayRssi = false;
        public int encoding_type = 0;
        public int power = 0;

        //private long killPasswordT = 0;
        //private long accessPasswordT = 0;
        //private bool PasswordInFile = true;

        private volatile bool writeError = false;

        private List<byte> lastTid = new List<byte>();

        //Files
        private bool filesOK;
        string iFileName = ""; //input file name
        string oFileName = ""; //output file name
        FileStream fsInput;
        FileStream fsOutput;
        CsvFileReader fileReaderInput;
        CsvFileWriter fileWriterOutput;

        //Standard commands and Constants
        //Constants
        readonly int MEM_RFU = 0x00;
        readonly int MEM_EPC = 0x01;
        readonly int MEM_TID = 0x02;
        readonly int MEM_USER = 0x03;

        byte[] LOCK_DATA = { 0x0A, 0xAA, 0xAA };

        //Commands

        //End of Standard commands and constants 

        public Form1()
        {
            InitializeComponent();
        }

        private void onResume()
        {
            RcpApi2.Instance.setOnRcpEventListener(this);
            max_tag = Properties.Settings.Default.max_tag;
            max_time = Properties.Settings.Default.max_time;
            repeat_cycle = Properties.Settings.Default.repeat_cycle;
            speakerBeep = Properties.Settings.Default.speakerBeep;
            readAfterPlugging = Properties.Settings.Default.readAfterPlugging;
            power = Properties.Settings.Default.power;

            bool newDisplayRssi = Properties.Settings.Default.displayRSSI;
            int new_encoding_type = Properties.Settings.Default.encodingType;

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
                    writeMessage("Reader Connected.");
                }
                else
                {
                    writeMessage("Cannot connect to Reader");
                }
            }
            else
            {
                plugged = false;
            }

            diplayPlug();

            if (RcpApi2.Instance.open())
            {
                RcpApi2.Instance.setOutputPowerLevel(power);
                log.InfoFormat("Power set to {1}", power);
                System.Threading.Thread.Sleep(100);
                RcpApi2.Instance.getOutputPowerLevel();
                writeMessage("Power level set to:" + power);
            }
            else
            {
                log.InfoFormat("Reader not connected. Cannot set the power");
                writeMessage("Error setting power level");
            }
        }

        private void writeMessage(string message)
        {
           if(lvMessage.Items.Count > 500)
            {
                for(int i=0; i<100; i++)
                {
                    lvMessage.Items.RemoveAt(0);
                }
            }
            ListViewItem lvi = new ListViewItem((lvItemCount++).ToString());
            //lvi.SubItems.Add();
            lvi.SubItems.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            lvi.SubItems.Add(message);
            lvMessage.Items.Add(lvi);
            //lvMessage.Items.Add((lvItemCount++).ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message);
            //lvMessage.Refresh();
            AutoSizeColumnList(lvMessage);


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

            log.Error("Error Code: " + Utils.ByteArrayToString(errCode));
            //MessageBox.Show("Error code: " + new ByteBuilder(errCode).ToString());
            writeMessage("Reader Error: " + new ByteBuilder(errCode).ToString());
            writeError = true;
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
            //Console.WriteLine("NATIVE: " + BitConverter.ToString(packet));
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
            log.Info("Reader Plugged: " + port);

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
            
            Console.WriteLine("Tag: " + hsb.ToString() + " read at: " + timeStamp);

            byte[] pc = new byte[epcStart];
            byte[] epc = new byte[pcEpc.Length - epcStart];
            {
                Array.Copy(pcEpc, 0, pc, 0, epcStart);
                Array.Copy(pcEpc, epcStart, epc, 0, epc.Length);
            }

            try
            {
                byte[] accessPwd = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };
                byte[] killPwd = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(accessPwd, 0, accessPwd.Length);
                    Array.Reverse(killPwd, 0, killPwd.Length);
                    Array.Reverse(LOCK_DATA, 0, LOCK_DATA.Length);
                }

                byte[] pwdMem = new byte[8];
                Array.Copy(killPwd,4, pwdMem, 0, 4);
                Array.Copy(accessPwd, 4, pwdMem, 4, 4);
                RcpApi2.Instance.writeToTagMemory(0x00000000, epc, MEM_RFU, 0, pwdMem);
                Console.WriteLine("Done Writing Access and Kill Password");

                System.Threading.Thread.Sleep(50);


                    RcpApi2.Instance.lockTagMemory(BitConverter.ToInt64(accessPwd, 0), epc, BitConverter.ToInt32(LOCK_DATA, 0));

                System.Threading.Thread.Sleep(50);

                byte[] userData = { 0x00, 0x01, 0x02, 0x03 };
                Console.WriteLine("Done creating user data");

                RcpApi2.Instance.writeToTagMemory(BitConverter.ToInt64(accessPwd,0), epc, MEM_USER, 0, userData);
                Console.WriteLine("Done Writing USER to the Tag");
                System.Threading.Thread.Sleep(50);


                byte[] newEpc = { 0x30, 0x08, 0x33, 0xB2, 0xDD, 0xD9, 0x01, 0x40, 0x00, 0x00, 0x00, 0x16 };//new byte[pcEpc.Length - epcStart];
                RcpApi2.Instance.writeToTagMemory(BitConverter.ToInt64(accessPwd,0), epc, MEM_EPC, 2, newEpc);
                Console.WriteLine("Done Writing EPC to the Tag");
                System.Threading.Thread.Sleep(50);




            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception type: " + ex.GetType() + " Message:" + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }


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
            
            //Check if the TID is same as last tag

            if (lastTid.SequenceEqual(tid.AsEnumerable<byte>()))
            {
                log.Debug("Same as Last TID/Tag. Ignoring...");
                System.Threading.Thread.Sleep(500);

                RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
                return;
            }

            int epcStart = this.getEpcStartPos(pcEpc);

            StringBuilder hsb = new StringBuilder();
            hsb.Append("PC: ");
            for (int i=0; i < epcStart; i++)
            {
                    hsb.Append(pcEpc[i].ToString("X02"));
            }

            hsb.Append("  EPC: ");
            for (int i = epcStart; i < pcEpc.Length; i++)
            {
                hsb.Append(pcEpc[i].ToString("X02"));
            }
            hsb.Append("  TID:");
            for(int i=0; i<tid.Length; i++)
            {
                hsb.Append(tid[i].ToString("X02"));
            }

            string timeStamp = DateTime.Now.ToString("G");
            //Console.WriteLine("Tag: " + hsb.ToString() + " read at: " + timeStamp);

            lblProcessing.Text = "Processing: " + hsb.ToString() + " at: " + timeStamp;
            log.Info("Processing: " + hsb.ToString());

            byte[] pc = new byte[epcStart];
            byte[] epc = new byte[pcEpc.Length - epcStart];
            {
                Array.Copy(pcEpc, 0, pc, 0, epcStart);
                Array.Copy(pcEpc, epcStart, epc, 0, epc.Length);
            }
            //DONE WITH NORMAL WORK

            //defining rows for input and output files 
            CsvRow fInput = new CsvRow();
            CsvRow fOutput = new CsvRow();

            //getting the Row from the input file
            if (fileReaderInput.EndOfStream)
            {
                //Console.WriteLine("End of Input File. Cannot proceed further.");
                //TODO: Add total number of records processed along with errors and success. 
                log.Info("End of Input File. Cannot proceed.");
                log.Info("Input file name: " + fileReaderInput);
                writeMessage("End of input file");

                MessageBox.Show("End of input file. \n Cannot proceed further", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    fileReaderInput.Close();
                    fileWriterOutput.Close();
                }
                catch (Exception ex)
                {
                    log.Error("Cannot close files.");
                    log.Error("Errror: " + ex.Message);
                    log.Error("Exception details:" + ex.InnerException); 
                    log.Error("Stack Trace: \n" + ex.StackTrace);
                    //Console.WriteLine("Cannot close files. \n Errror: " + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                }
                return;
            }
            
            fileReaderInput.ReadRow(fInput);

            //creting object for Tag
            string[] rowData = fInput.ToArray();
            CsvDataFormat cdf = new CsvDataFormat();
            try
            {
                cdf.srNo = rowData[0];
                cdf.setAccessPwd(rowData[1]);
                cdf.setKillPwd(rowData[2]);
                cdf.setEpcMem(rowData[3]);
                cdf.setUserMem(rowData[4]);
                cdf.setLockMem (rowData[5]);
                cdf.tidMem = tid;
            }
            catch (Exception ex)
            {
                log.Error("Error processing record " + hsb.ToString());
                log.Error("Errror: " + ex.Message);
                log.Error("Exception details:" + ex.InnerException);
                log.Error("Stack Trace: \n" + ex.StackTrace);
                RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
                totalError++;
                totalProcessed++;
                writeMessage("Error processing input line - " + cdf.ToString());
                return;
            }

            try
            {
                byte[] accessPwd;
                byte[] killPwd;

                accessPwd = cdf.getAccessPwd();  //{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };
                killPwd = cdf.getKillPwd();      //{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };

                byte[] pwdMem = new byte[8];
                Array.Copy(killPwd, 4 , pwdMem, 0, 4);
                Array.Copy(accessPwd, 4, pwdMem, 4, 4);

                //Writing EPC using TID 
                RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x01, 0x02, cdf.getEpcMem());
                log.Info("EPC: " + Utils.ByteArrayToString(cdf.getEpcMem()) + " written to TID: " + Utils.ByteArrayToString(tid));


                    //Writing USER if USER is longer than 1 byte else skipping
                    if (cdf.getUserMem().Length > 1)
                    {
                        RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x03, 0x00, cdf.getUserMem());
                        //As not writing user we should not lock it 
                        LOCK_DATA[0] = 0x0A;
                        LOCK_DATA[1] = 0xAA;
                        LOCK_DATA[2] = 0xAA;
                        log.Info("USER: " + Utils.ByteArrayToString(cdf.getUserMem()) + " written to TID: " + Utils.ByteArrayToString(tid));
                    }
                    else
                    {
                        log.Info("No USER memory written to TID: " + Utils.ByteArrayToString(tid));
                    }


                //System.Threading.Thread.Sleep(100);

                    //Writing the ACCESS MEMORY 
                    RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x00, 0, pwdMem);
                    log.Info("PASSWORD: " + Utils.ByteArrayToString(cdf.getKillPwd()) + Utils.ByteArrayToString(cdf.getAccessPwd()) + "to TID: " + Utils.ByteArrayToString(tid));

  
                //System.Threading.Thread.Sleep(100);
                //Finally locking the tag
                Int64 apw = Convert.ToInt64(Utils.ByteArrayToString(cdf.getAccessPwd()), 16);
                Int32 lData = Convert.ToInt32(Utils.ByteArrayToString(LOCK_DATA), 16);
                RcpApi2.Instance.lockTagMemory_MaskTID(apw, tid, lData);
                log.Info("Tag data locked for TID: " + Utils.ByteArrayToString(tid) + " using lock code: " + Utils.ByteArrayToString(LOCK_DATA));

                log.Info("Process completed for TID: " + Utils.ByteArrayToString(tid));
                writeMessage("Writing completed for TID: " + Utils.ByteArrayToString(tid) + " EPC:" + Utils.ByteArrayToString(cdf.getEpcMem()) + " USER:" + Utils.ByteArrayToString(cdf.getUserMem()));

                //System.Threading.Thread.Sleep(100);
                if (writeError)
                {
                    writeError = false;
                    throw new Exception("Tag Memory write error");
                    
                }

                //DONE WRITING TAGS

                //Updating last tid field 
                lastTid.Clear();
                lastTid.AddRange(tid.AsEnumerable<byte>());

                //Now writing data to output file
                CsvRow rowToWrite = new CsvRow();
                rowToWrite.Add(cdf.srNo);
                rowToWrite.Add(Utils.ByteArrayToString(cdf.getAccessPwd()));
                rowToWrite.Add(Utils.ByteArrayToString(cdf.getKillPwd()));
                rowToWrite.Add(Utils.ByteArrayToString(cdf.getEpcMem()));
                rowToWrite.Add(cdf.getUserMem().Length > 1 ? Utils.ByteArrayToString(cdf.getUserMem()) : "");
                rowToWrite.Add(cdf.getLockMem() == true ? "YES" : "NO");
                rowToWrite.Add(Utils.ByteArrayToString(cdf.tidMem));

                fileWriterOutput.WriteRow(rowToWrite);
                log.Debug("Done writing record to Output file: " + fileWriterOutput);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception type: " + ex.GetType() + " Message:" + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
                writeMessage("Error processing record - " + cdf.ToString());
                totalError++;
                totalProcessed++;
                return;
            }

            System.Threading.Thread.Sleep(200);

            totalProcessed++;
            lblProcessingError.Text = totalError.ToString();
            lblProcessingSuccess.Text = totalProcessed.ToString();
            RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
        }
        public void onTxPowerLevelReceived(int currPower, int minPower, int maxPower)
        {
            log.InfoFormat("Power set successful. Curr Power: {1}", currPower);       
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
            //lvMessage.Columns.Add("Sr. No.");
            lvMessage.View = View.Details;
            lvMessage.Columns.Add("Sr. No.", -2, HorizontalAlignment.Left);
            lvMessage.Columns.Add("Timestamp", -2, HorizontalAlignment.Left);
            lvMessage.Columns.Add("Message", -2, HorizontalAlignment.Left);
            lvMessage.Refresh();

            //MessageBox.Show(BitConverter.IsLittleEndian.ToString());
            lblPath.Text = "";
            lblSelectedFile.Text = "";
            lblOutFile.Text = "";
            lblProcessing.Text = "";
            lblProcessing.Text = "";
            lvMessage.Items.Clear();
            lblProcessingError.Text = "";
            lblProcessingSuccess.Text = "";

            btnStartRead.Enabled = false;
            btnStopRead.Enabled = false;

            
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
                        log.Error("Cannot open reader");
                        writeMessage("Cannot connect to Reader");
                    }
                    else
                    {
                        RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
                        writeMessage("Write process started. Waiting for Tag");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot open the reader");
                    log.Error("Cannot open Reader");
                    writeMessage("Cannot connect to Reader");
                }
            }
            else
            {
                RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
            }
            btnStartRead.Enabled = false;
            btnStopRead.Enabled = true;
            btnBrowse.Enabled = false;
            //lvMessage.Items.Clear();
        }

        private void btnStopRead_Click(object sender, EventArgs e)
        {
            fsOutput.Flush();
            RcpApi2.Instance.stopReadTags();
            btnStartRead.Enabled = true;
            btnStopRead.Enabled = false;
            btnBrowse.Enabled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fileName = "";
            try
            {
                if(fsInput != null)
                    fsInput.Close();
                if(fsOutput != null)
                    fsOutput.Close();
                log.Debug("closed input and output files");
            }
            catch
            {
                log.Debug("closed input and output files in error");
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Select CSV file for encoding tags",
                Filter = "csv files(*.csv)|*.csv",
                DefaultExt = "csv",
                FilterIndex = 2,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                RestoreDirectory = true
            };

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                lblPath.Text =  Path.GetDirectoryName(fileName);
                lblSelectedFile.Text = Path.GetFileName(fileName);
                iFileName = fileName;
            }
            else
            {
                MessageBox.Show("Please select file to process", "Select File...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            //setting up filename for output file 
            string outFileName = fileName.Substring(0, fileName.Length - 4) + "_out.csv";
            writeMessage("Processing file. Input: " + Path.GetFileName(fileName) + ", Output file: " + Path.GetFileName(outFileName));

            //Setting label for out file name
            lblOutFile.Text = Path.GetFileName(outFileName);

            if (File.Exists(outFileName))
            {
                DialogResult resp = MessageBox.Show("Output file already exists. Delete it?", "Delete out file",  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                log.Info("Output file deleted as per user request. ");
                if(resp == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(outFileName);
                        writeMessage("Old log file deleted.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error \n Cannot proceed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Debug("Cannot proceed as output file already exists and user wants to keep it");
                        writeMessage("Error deleting old log file");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            oFileName = outFileName;

            filesOK = true;

            //Open the file as a read only stream and check if it can be opened 
            try
            {
                fsInput = new FileStream(iFileName, FileMode.Open, FileAccess.Read);
                
            }
            catch( Exception ex)
            {
                MessageBox.Show("Cannot Open Input file. \n Error: " + ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Cannot open Input file");
                log.Error("Errror: " + ex.Message);
                log.Error("Exception details:" + ex.InnerException);
                log.Error("Stack Trace: \n" + ex.StackTrace);
                writeMessage("Cannot open input file for reading");
                filesOK = false;
            }

            //Open the output file for write. 
            try
            {
                fsOutput = new FileStream(oFileName, FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot Open Output file. \n Error: " + ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Cannot open Output file");
                log.Error("Errror: " + ex.Message);
                log.Error("Exception details:" + ex.InnerException);
                log.Error("Stack Trace: \n" + ex.StackTrace);
                writeMessage("Cannot open output file for writing");
                filesOK = false;
            }

            //creating CSV File Reader and Writer
            fileReaderInput = new CsvFileReader(fsInput);
            fileWriterOutput = new CsvFileWriter(fsOutput);

            CsvRow crHeader = new CsvRow();

            //lets try to get the header of the file
            fileReaderInput.ReadRow(crHeader);
            log.Info("File Header");
            
            StringBuilder sa = new StringBuilder();
            foreach (string s in crHeader)
            {
                sa.Append(s);
            }
            log.Info(sa.ToString());
            sa.Clear();

            //TODO::Checking for header ROW 
            CsvRow crStdHeader = new CsvRow();
            crStdHeader.Add("SRNO"); crStdHeader.Add("ACCESS_PWD"); crStdHeader.Add("KILL_PWD"); crStdHeader.Add("EPC"); crStdHeader.Add("USER"); crStdHeader.Add("LOCK");
            if (Enumerable.SequenceEqual(crHeader, crStdHeader))
            {
                log.Debug("Headers OK.");
                writeMessage("Input file Headers OK. Processing.");
            }
            else
            {
                log.Error("WRONG HEADER.");
                MessageBox.Show("Cannot proceed. Header Mismatch.", "Header Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                writeMessage("Wrong Input file format. Headers Mismatch");
                return;
            }
            btnStartRead.Enabled = true;
        }
        #region Commented code
        /*
         private void cbLock_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbl = (CheckBox)sender;
            if (cbl.CheckState == CheckState.Checked)
            {
                //check if then check if Access and Kill Passwords are non zero 
                string strKillPwd = txtKillPwd.Text.Trim().Replace(" ", "");
                string strAccessPwd = txtAccessPwd.Text.Trim().Replace(" ", "");
                Int64 kp =0, ap=0;

                bool kaPasswordOk = true;
                try
                {
                    kp = Convert.ToInt64(strKillPwd, 16);
                    if(kp == 0)
                    {
                        MessageBox.Show("Please enter the Kill Password!", "Kill Password Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        kaPasswordOk = kaPasswordOk && false;
                        return;
                    }
                    else
                    {

                    }

                    ap = Convert.ToInt64(strKillPwd, 16);
                    if (ap == 0)
                    {
                        MessageBox.Show("Please enter the Access Password!", "Access Password Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        kaPasswordOk = kaPasswordOk && false;
                    }
                }
                catch(Exception ex)
                {
                    log.Debug("Error concerting Kill / Access Passwords.");
                    log.Debug("Kill Password: " + txtKillPwd.Text.Trim());
                    log.Debug("Access Password: " + txtAccessPwd.Text.Trim());
                    log.Debug(ex.Message + "\n" + ex.InnerException);
                }

                if(kaPasswordOk != true)
                {
                    cbLockTags.Checked = false;
                }
                else
                {
                    if (MessageBox.Show("Use password as entered instead of from input file", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        PasswordInFile = false;
                        killPasswordT = kp;
                        accessPasswordT = ap;
                        log.Info("Password provided in UI. Kill: " + txtKillPwd.Text.Trim() + " Access: " + txtAccessPwd.Text.Trim());
                    }
                    else
                    {
                        PasswordInFile = true;
                        cbLockTags.Checked = false;
                        MessageBox.Show("Using password from input file", "Information..", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        */

        /*
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbo = (CheckBox)sender;
            if (cbo.CheckState == CheckState.Checked)
            {

                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    Description = "Select folder to generate output file",
                    SelectedPath = @"C:\",
                    ShowNewFolderButton = true
                };

                DialogResult folderName = fbd.ShowDialog();
                if (folderName == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string fileName = fbd.SelectedPath.Trim() + "\\"+ "KEncoderV1" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".csv";
                    lblFolderPath.Text = fileName;
                    MessageBox.Show("Output file will be stored at : \n" + fileName, "Output File...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    oFileName = fileName;
                    log.Info("Output file for copy TID to EPC" + fileName);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(lblFolderPath.Text))
                {
                    DialogResult dialogResult =  MessageBox.Show("Output file already set. Do you want to reset or cancel", "Reset / Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                        lblFolderPath.Text = "";
                        oFileName = "";
                        log.Info("Removed Copy TID to EPC output file name");
                    }
                }
            }

        }
        */
        #endregion Commented Code

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(RcpApi2.Instance.isOpened())
            {
                RcpApi2.Instance.close();
                this.Close();
            }
        }

        private void setPowerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox abt = new AboutBox();
            abt.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.rfts.co.in");
        }

        private void AutoSizeColumnList(ListView listView)
        {
            //Prevents flickering
            listView.BeginUpdate();

            Dictionary<int, int> columnSize = new Dictionary<int, int>();

            //Auto size using header
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //Grab column size based on header
            foreach (ColumnHeader colHeader in listView.Columns)
                columnSize.Add(colHeader.Index, colHeader.Width);

            //Auto size using data
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            //Grab comumn size based on data and set max width
            foreach (ColumnHeader colHeader in listView.Columns)
            {
                int nColWidth;
                if (columnSize.TryGetValue(colHeader.Index, out nColWidth))
                    colHeader.Width = Math.Max(nColWidth, colHeader.Width);
                else
                    //Default to 50
                    colHeader.Width = Math.Max(50, colHeader.Width);
            }

            listView.EndUpdate();
        }
    }
}
