using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Phychips.Helper;
using Phychips.Rcp;
using Phychips.Red;

namespace TagEncoderV1
{
    public partial class frmMain : Form, IRcpEvent2 
    {
        //Logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string iFileName = "";
        string oFileName = "";
        bool encodingStarted = false;

        int rowCounter = 1;

        public frmMain()
        {
            InitializeComponent();
        }

        private void rbInputFile_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            //rb.Checked = true;
            //rbCopyTidToEpc.Checked = false;
            if (rb.Checked)
            {
                gbpInputFile.Enabled = true;
            }
            else
            {
                gbpInputFile.Enabled = false;
            }

            //resetting input file details
            iFileName = "";
            lblInputFile.Text = "";
            lblOutputFile.Text = "";
            lblPath.Text = "";
        }

        private void rbCopyTidToEpc_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            //rbFromFile.Checked = false;
            //rbCopyTidToEpc.Checked = true;
            if (rb.Checked)
            {
                gbpInputFile.Enabled = false;
            }

            //resetting input file details
            iFileName = "";
            lblInputFile.Text = "";
            lblOutputFile.Text = DateTime.Now.ToString("yyyyMMddHHmmss") + "_out.csv";
            lblPath.Text = "";
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            rbInputFile.Checked = false;
            rbCopyTidToEpc.Checked = false;
            gbpOptions.Enabled = false;
            gbpSetPassword.Enabled = false;
            btnStopEncoding.Enabled = false;
            cbPermaLock.Enabled = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fileName = "";
            iFileName = "";
            oFileName = "";

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Select CSV file for encoding tags",
                Filter = "csv files(*.csv)|*.csv|All Files(*.*)|*.*",
                DefaultExt = "csv",
                FilterIndex = 2,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                lblPath.Text = Path.GetDirectoryName(fileName);
                lblInputFile.Text = Path.GetFileName(fileName);
                iFileName = fileName;
                oFileName = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileName(fileName).Substring(0, Path.GetFileName(fileName).Length - 4) + "_out.csv";
                lblOutputFile.Text = Path.GetFileName(oFileName);
            }
            else
            {
                MessageBox.Show("Please select file to process", "Select File...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!string.IsNullOrWhiteSpace(iFileName))
            {
                log.Info("Input file: " + iFileName);
            }
        }

        private void cbLockTags_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.CheckState == CheckState.Checked)
            {
                gbpOptions.Enabled = true;
                rbFromFile.Checked = true;
                cbPermaLock.Enabled = true;
            }
            else
            {
                gbpOptions.Enabled = false;
                gbpSetPassword.Enabled = false;
                cbPermaLock.Checked = false;
                cbPermaLock.Enabled = false;
            }
        }

        private void rbFromFile_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if(rb.Checked)
            {
                gbpSetPassword.Enabled = false;
            }
        }

        private void rbSpecify_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if(rb.Checked)
            {
                gbpSetPassword.Enabled = true;
            }
        }

        private Int64 kp, ap;
        private Int64 LockTags = 0;
        private Int64 permaLock = 0; //TODO: CHANGE THIS TO ACTUAL CODE
        private bool processFile = false;
        FileStream fsInput, fsOutput;
        CsvFileReader fileReaderInput;
        CsvFileWriter fileWriterOutput;
        private void btnStartEncoding_Click(object sender, EventArgs e)
        {
            string killPwd = "";
            string accessPwd = "";
            

            if (encodingStarted == false)
            {
                //check all parameters if encoding can be started 

                //getting password details 
                if (gbpOptions.Enabled)
                {
                    if (rbFromFile.Checked) // Take the passwords from the file
                    {
                        //Both these need to be picked up from the input file
                        kp = 0;
                        ap = 0;
                    }
                    else
                    {
                        //get the details from the text input
                        killPwd = txtKillPassword.Text.Trim().Replace(" ", "");
                        accessPwd = txtAccessPassword.Text.Trim().Replace(" ", "");
                        try
                        {
                            kp = Convert.ToInt64(killPwd, 16);
                            ap = Convert.ToInt64(accessPwd, 16);

                            if (ap == 0)
                            {
                                MessageBox.Show("To lock tags, Access password must be non zero", "Invalid Access Password...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            LockTags = 0;
                            

                            if (cbPermaLock.Checked)
                            {
                                LockTags = LockTags | permaLock; // adding perma lock
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Invalid Kill and/or Access password.", "Invalid passwords... ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if (rbInputFile.Checked) // source is the file
                {
                    //Check for Input file
                    if (!string.IsNullOrWhiteSpace(lblInputFile.Text.Trim()) && !string.IsNullOrWhiteSpace(lblOutputFile.Text.Trim()))
                    {
                        if (File.Exists(Path.Combine(lblPath.Text.Trim(), lblOutputFile.Text.Trim())))
                        {
                            DialogResult resp = MessageBox.Show("Output file already exists. Delete it?", "Delete...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            log.Info("Output file deleted as per user request. ");
                            if (resp == DialogResult.Yes)
                            {
                                try
                                {
                                    File.Delete(Path.Combine(lblPath.Text.Trim(), lblOutputFile.Text.Trim()));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error \n Cannot proceed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    log.Debug("Cannot proceed as output file already exists and user wants to keep it");
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }

                            try
                            {

                                fsInput = new FileStream(iFileName, FileMode.Open, FileAccess.Read);
                                fsOutput = new FileStream(oFileName, FileMode.OpenOrCreate, FileAccess.Write);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Cannot Open Input file. \n Error: " + ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                log.Error("Cannot open Input file");
                                log.Error("Errror: " + ex.Message);
                                log.Error("Exception details:" + ex.InnerException);
                                log.Error("Stack Trace: \n" + ex.StackTrace);
                                return;
                            }

                            fileReaderInput = new CsvFileReader(fsInput);
                            fileWriterOutput = new CsvFileWriter(fsOutput);
                        }
                        processFile = true;
                       
                    }
                    else
                    {
                        log.Error("Cannot start encoding. No input output files are set");
                        MessageBox.Show("Please select Input file first", "Select file...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //End check for input file
                }
                else // copy TID to EPC
                {
                    try
                    {
                        fsOutput = new FileStream(lblOutputFile.Text.Trim(), FileMode.OpenOrCreate, FileAccess.Write);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot create output file. \n Error: " + ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Cannot create output file");
                        log.Error("Errror: " + ex.Message);
                        log.Error("Exception details:" + ex.InnerException);
                        log.Error("Stack Trace: \n" + ex.StackTrace);
                        return;
                    }

                    //fileReaderInput = new CsvFileReader(fsInput);
                    fileWriterOutput = new CsvFileWriter(fsOutput);
                    processFile = false;
                }


                #region
                /*
                 bool okToStartSource = false;
                //1. Check the input source radio buttons 
                if (rbFromFile.Checked == true)
                {
                    //Encode from file
                    //2. Check if input and output files are set 
                    if (!string.IsNullOrWhiteSpace(iFileName) && !string.IsNullOrWhiteSpace(oFileName))
                    {
                        okToStartSource = true;
                    }
                }
                else if(rbCopyTidToEpc.Checked)
                {
                    //this is to copy TID to EPC so do that check 
                    okToStartSource = false; //Ths source is tag itself
                }

                bool okToLock = false;
                //Password section 
                if(cbLockTags.Checked == true)
                {
                    okToLock = true;
                }

                bool okToPermaLock = false;
                if(cbPermaLock.Checked == true)
                {
                    okToPermaLock = true;
                }

                bool okPwd = false;
                bool pwdSrcFile = false;
                if (okToLock)
                {
                    //check if from file or specified 
                    if(rbFromFile.Checked == true)
                    {
                        okPwd = true;
                        pwdSrcFile = true;
                    }
                    else
                    {
                        //password specified selected 
                        killPwd = txtKillPassword.Text.Trim().Replace(" ", "");
                        accessPwd = txtAccessPassword.Text.Trim().Replace(" ", "");
                        try
                        {
                            kp = Convert.ToInt64(killPwd, 16);
                            ap = Convert.ToInt64(accessPwd, 16);

                            if(ap > 0)
                            {
                                okPwd = true;
                                pwdSrcFile = false;
                            }
                            else
                            {
                                MessageBox.Show("To lock tags, Access password must be non zero", "Invalid Access Password...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Invalid Kill and/or Access password.", "Invalid passwords... ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }


                if(okToStartSource == true)
                {
                    if(okToLock == true)
                    {
                        if (okPwd == true)
                        {
                            //TODO - check where to read the passwords from
                            if (okToPermaLock == true)
                            {
                                //change LOCK CODE
                            }
                            if (pwdSrcFile)
                            {
                                //passwords are in the file so read them and use for locking 
                            }
                            else
                            {
                                //Passwords to be read from the window
                                //use kp and ap

                            }
                        }
                    }
                    else
                    {
                        //No Locking
                        //CHANGE LOCK CODE TO 0
                    }
                    //TODO: Start Reading the tags
                    //WriteTagsFromFile(LOCK_CODE);
                }
                else
                {
                    //Copy TID to EPC
                    if (okToLock == false)
                    {
                        if (okPwd == true)
                        {
                            if (okToPermaLock == true)
                            {
                                //change LOCK CODE
                            }
                            if (pwdSrcFile)
                            {
                                //passwords in the file 
                            }
                            else
                            {
                                //read password from screen
                                //use kp and ap
                            }
                        }
                    }
                    else
                    {
                        //No Locking
                        //CHANGE LOCK CODE TO 0
                    }
                    //TODO: Start Reading the tags
                    //WriteTagsTIDtoEPC(LOCK_CODE);
                }
                */
                #endregion

                
                if (!RcpApi2.Instance.isOpened())
                {
                    if (RcpApi2.Instance.isOpenable())
                    {
                        if (!RcpApi2.Instance.open())
                        {
                            MessageBox.Show("Cannot open the reader", "Reader Error...",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            log.Error("Cannot Open Reader");
                            if (rbInputFile.Checked)
                            {
                                fsInput.Close();
                            }
                            fsOutput.Close();
                            return;
                        }
                        else
                        {
                            encodingStarted = true;
                            btnStopEncoding.Enabled = true;
                            btnStartEncoding.Enabled = false;
                            RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
                        }
                    }
                    else
                    {
                        log.Error("Cannot Open Reader");
                        if (rbInputFile.Checked)
                        {
                            fsInput.Close();
                        }
                        fsOutput.Close();
                        MessageBox.Show("Cannot open reader. Not openable.", "Reader Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    encodingStarted = true;
                    btnStopEncoding.Enabled = true;
                    btnStartEncoding.Enabled = false;
                    RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);

                }
            }
        }

        private void btnStopEncoding_Click(object sender, EventArgs e)
        {
            if (encodingStarted == true)
            {
                try
                {
                    //fileReaderInput.Close();
                    //fileWriterOutput.Close();
                    fsInput.Close();
                    fsOutput.Close();
                }
                catch
                {
                    //nothing to be done
                }
                //TODO: Stop Reading the tags
                encodingStarted = false;
                btnStartEncoding.Enabled = true;
                btnStopEncoding.Enabled = false;
                try
                {
                    RcpApi2.Instance.close();
                }
                catch
                {
                    //nothing to be done
                }
            }
        }

        int max_tag, max_time, repeat_cycle, encoding_type;
        bool speakerBeep, readAfterPlugging, displayRssi, plugged;
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

        public void onSuccessReceived(byte[] data, int cmdCode)
        {
            throw new NotImplementedException();
        }

        public void onFailureReceived(byte[] errCode)
        {
            //throw new NotImplementedException();
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    onFailureReceived(errCode);
                }));
                return;
            }

            log.Error("Error Code: " + Utils.ByteArrayToString(errCode));
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

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        public void onTagWithRssiReceived(byte[] pcEpc, int rssi)
        {
            throw new NotImplementedException();
        }

        private List<byte> lastTid = new List<byte>();
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

            if (lastTid.SequenceEqual(tid.AsEnumerable<byte>()))
            {
                //Console.WriteLine("Same as last tag. Ignoring...");
                log.Debug("Same as Last Tag. Ignoring...");

                System.Threading.Thread.Sleep(500);

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

                return;
            }

            int epcStart = this.getEpcStartPos(pcEpc);

            StringBuilder hsb = new StringBuilder();

            hsb.Append("PC: ");

            for (int i = 0; i < epcStart; i++)
            {
                try
                {
                    hsb.Append(pcEpc[i].ToString("X02"));
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error in processing PC bits");
                    log.Error("Error processing PC bits for TID :" + Utils.ByteArrayToString(tid));
                    log.Error("Errror: " + ex.Message);
                    log.Error("Exception details:" + ex.InnerException);
                    log.Error("Stack Trace: \n" + ex.StackTrace);
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
                catch (Exception ex)
                {
                    //Console.WriteLine("Error in capturing EPC");
                    log.Error("Error processing EPC bits for TID :" + Utils.ByteArrayToString(tid));
                    log.Error("Errror: " + ex.Message);
                    log.Error("Exception details:" + ex.InnerException);
                    log.Error("Stack Trace: \n" + ex.StackTrace);
                    break;
                }
            }
            hsb.Append("  TID:");
            for (int i = 0; i < tid.Length; i++)
            {
                try
                {
                    hsb.Append(tid[i].ToString("X02"));
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error in capturing TID");
                    log.Error("Error processing TID bits for TID :" + Utils.ByteArrayToString(tid));
                    log.Error("Errror: " + ex.Message);
                    log.Error("Exception details:" + ex.InnerException);
                    log.Error("Stack Trace: \n" + ex.StackTrace);
                    break;
                }
            }

            string timeStamp = DateTime.Now.ToString("G");
            //Console.WriteLine("Tag: " + hsb.ToString() + " read at: " + timeStamp);

            lblProcessing.Text = "Processing: " + hsb.ToString() + " at: " + timeStamp + "\n";
            log.Info("Processing: " + hsb.ToString());

            byte[] pc = new byte[epcStart];
            byte[] epc = new byte[pcEpc.Length - epcStart];
            {
                Array.Copy(pcEpc, 0, pc, 0, epcStart);
                Array.Copy(pcEpc, epcStart, epc, 0, epc.Length);
            }
            //DONE WITH NORMAL WORK

            //check how the data has to be processed - from file or just copy TID to EPC
            CsvRow fInput = new CsvRow();
            CsvRow fOutput = new CsvRow();
            CsvDataFormat cdf = new CsvDataFormat();
            byte[] accessPwd;
            byte[] killPwd;
            byte[] pwdMem = new byte[8];

            if (rbInputFile.Checked)
            {
                if (fileReaderInput.EndOfStream)
                {
                    //TODO: Add total number of records processed along with errors and success. 
                    log.Info("End of Input File. Cannot proceed.");
                    log.Info("Input file name: " + fileReaderInput);

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

                try
                {
                    cdf.srNo = rowData[0];
                    cdf.setAccessPwd(rowData[1]);
                    cdf.setKillPwd(rowData[2]);
                    cdf.setEpcMem(rowData[3]);
                    cdf.setUserMem(rowData[4]);
                    cdf.setLockMem(rowData[5]);
                    cdf.tidMem = tid;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error reading from input file. \n Error:" + ex.Message);
                    log.Error("Error reading from input file. Filename: " + fileReaderInput);
                    log.Error("Errror: " + ex.Message);
                    log.Error("Exception details:" + ex.InnerException);
                    log.Error("Stack Trace: \n" + ex.StackTrace);
                }

                //check how the password is set
                if (rbFromFile.Checked)
                {
                    //passwords are set in the file so need to read from the file
                    accessPwd = cdf.getAccessPwd();  //{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };
                    killPwd = cdf.getKillPwd();      //{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };

                    Array.Copy(killPwd, 4, pwdMem, 0, 4);
                    Array.Copy(accessPwd, 4, pwdMem, 4, 4);
                }
            }
            else
            {
                //Copy TID to EPC
                accessPwd = BitConverter.GetBytes(ap);
                killPwd = BitConverter.GetBytes(kp);
                Array.Copy(killPwd, 4, pwdMem, 0, 4);
                Array.Copy(accessPwd, 4, pwdMem, 4, 4);
            }

            //Writing EPC using TID 
            RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x01, 0x02, cdf.getEpcMem());
            log.Info("EPC: " + Utils.ByteArrayToString(cdf.getEpcMem()) + " written to TID: " + Utils.ByteArrayToString(tid));

            //Writing USER if USER is longer than 1 byte else skipping
            if (cdf.getUserMem().Length > 1)
            {
                RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x03, 0x00, cdf.getUserMem());
                log.Info("USER: " + Utils.ByteArrayToString(cdf.getUserMem()) + " written to TID: " + Utils.ByteArrayToString(tid));
            }
            else
            {
                log.Info("No USER memory written to TID: " + Utils.ByteArrayToString(tid));
            }

            //Writing the ACCESS MEMORY 
            RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x00, 0, pwdMem);
            log.Info("PASSWORD: " + Utils.ByteArrayToString(cdf.getKillPwd()) + Utils.ByteArrayToString(cdf.getAccessPwd()) + "to TID: " + Utils.ByteArrayToString(tid));

            //Finally locking the tag
            Int64 apw = Convert.ToInt64(Utils.ByteArrayToString(cdf.getAccessPwd()), 16);
            Int32 lData = Convert.ToInt32(Utils.ByteArrayToString(LOCK_DATA), 16);
            RcpApi2.Instance.lockTagMemory_MaskTID(apw, tid, lData);
            log.Info("Tag data locked for TID: " + Utils.ByteArrayToString(tid) + " using lock code: " + Utils.ByteArrayToString(LOCK_DATA));

            log.Info("Process completed for TID: " + Utils.ByteArrayToString(tid));

            //DONE WRITING TAGS

            //Updating last tid field 
            lastTid.Clear();
            lastTid.AddRange(tid.AsEnumerable<byte>());

            //Now writing data to output file
            CsvRow rowToWrite = new CsvRow();
            rowToWrite.Add(rbFromFile.Checked == true ?  cdf.srNo : (rowCounter++).ToString());
            rowToWrite.Add(Utils.ByteArrayToString(BitConverter.GetBytes(ap)));
            rowToWrite.Add(Utils.ByteArrayToString(BitConverter.GetBytes(kp)));
            rowToWrite.Add(Utils.ByteArrayToString(epc));
            rowToWrite.Add(rbFromFile.Checked == true ? (cdf.getUserMem().Length > 1 ? Utils.ByteArrayToString(cdf.getUserMem()) : ""): "");
            rowToWrite.Add(cbLockTags.Checked == true ? "YES" : "NO");
            rowToWrite.Add(Utils.ByteArrayToString(tid));

            fileWriterOutput.WriteRow(rowToWrite);
            log.Debug("Done writing record to Output file: " + fileWriterOutput);

            System.Threading.Thread.Sleep(1000);

            if (!RcpApi2.Instance.isOpened())
            {
                if (RcpApi2.Instance.isOpenable())
                {
                    if (!RcpApi2.Instance.open())
                    {
                        MessageBox.Show("Cannot open the reader");
                        log.Error("Cannot Open Reader");
                    }
                }
            }
            else
            {
                RcpApi2.Instance.startReadTagsWithTid(1, 0, 0);
            }

        }

        byte[] LOCK_DATA = { 0x0A, 0xAA, 0xAA };
        private bool processInputFile(byte[] pc, byte[] epc, byte[] tid)
        {
            //defining rows for input and output files 
            

            

            
            
            

            
            return true;
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

        public void onFHModeReceived(int mode)
        {
            throw new NotImplementedException();
        }

        public void onFHModeRefLevelReceived(int refLevel)
        {
            throw new NotImplementedException();
        }

        public void onNativeReceived(byte[] packet)
        {
            throw new NotImplementedException();
        }

        public void onGetReaderSerial(byte[] packet)
        {
            throw new NotImplementedException();
        }

        //Implementation of IRcpEvent2 Interface



    }
}