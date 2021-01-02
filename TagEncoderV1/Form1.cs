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

        private int nTagCnt = 0;
        private bool plugged = false;

        public int max_tag = 0;
        public int max_time = 0;
        public int repeat_cycle = 0;
        public bool speakerBeep = false;
        public bool readAfterPlugging = false;
        public bool displayRssi = false;
        public int encoding_type = 0;

        private List<byte> lastTid = new List<byte>();

        //Files
        bool filesOK = false;
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
                Console.WriteLine("Same as last tag. Ignoring...");

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

            lblProcessing.Text = "Processing: " + hsb.ToString() + " at: " + timeStamp;

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
                Console.WriteLine("End of Input File. Cannot proceed further.");
                MessageBox.Show("End of input file. \n Cannot proceed further", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    fileReaderInput.Close();
                    fileWriterOutput.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot close files. \n Errror: " + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
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
                Console.WriteLine("Error reading from input file. \n Error:" + ex.Message);
            }

            try
            {
                byte[] accessPwd = cdf.getAccessPwd();  //{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };
                byte[] killPwd = cdf.getKillPwd();      //{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 };

                /*
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(accessPwd, 0, accessPwd.Length);
                    Array.Reverse(killPwd, 0, killPwd.Length);
                    Array.Reverse(LOCK_DATA, 0, LOCK_DATA.Length);
                }
                */
                byte[] pwdMem = new byte[8];
                Array.Copy(killPwd, 4 , pwdMem, 0, 4);
                Array.Copy(accessPwd, 4, pwdMem, 4, 4);

                //Writing EPC using TID 
                RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x01, 0x02, cdf.getEpcMem());

                //Writing USER if USER is longer than 1 byte else skipping
                if(cdf.getUserMem().Length > 1)
                {
                    RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x03, 0x00, cdf.getUserMem());
                    //As not writing user we should not lock it 
                    LOCK_DATA[0] = 0x0A;
                    LOCK_DATA[1] = 0xAA;
                    LOCK_DATA[2] = 0xAA;
                }

                //Writing the ACCESS MEMORY 
                RcpApi2.Instance.writeToTagMemory_MaskTID(0, tid, 0x00, 0, pwdMem);

                //Finally locking the tag
                Int64 apw = Convert.ToInt64(Utils.ByteArrayToString(cdf.getAccessPwd()), 16);
                Int32 lData = Convert.ToInt32(Utils.ByteArrayToString(LOCK_DATA),16);
                RcpApi2.Instance.lockTagMemory_MaskTID(apw, tid, lData) ;  

                //DONE WRITING TAGS
/*
                //Writing Passwords in the memory
                RcpApi2.Instance.writeToTagMemory(0x00000000, epc, MEM_RFU, 0, pwdMem);
                //RcpApi2.Instance.writeToTagMemory(0x11010000, epc, MEM_RFU, 0, pwdMem);
                Console.WriteLine("Done Writing Access and Kill Password");

                System.Threading.Thread.Sleep(150);

                //Locking the Tag Memory
                RcpApi2.Instance.lockTagMemory(BitConverter.ToInt64(accessPwd, 0), epc, BitConverter.ToInt32(LOCK_DATA, 0));
                //RcpApi2.Instance.lockTagMemory(0x00000000, epc, BitConverter.ToInt32(LOCK_DATA, 0));
                Console.WriteLine("Done Locking the tag memory");

                System.Threading.Thread.Sleep(150);

                //Check if user data is provided 
                if (cdf.getUserMem().Length > 2)
                {
                    byte[] userData = cdf.getUserMem();
                    RcpApi2.Instance.writeToTagMemory(BitConverter.ToInt64(accessPwd, 0), epc, MEM_USER, 0, userData);
                    Console.WriteLine("Done Writing USER to the Tag");
                    System.Threading.Thread.Sleep(150);
                }
                else
                {
                    Console.WriteLine("No data to be writtien to USER memory");
                }

                
                //Start writing EPC memory
                //byte[] newEpc = { 0x30, 0x08, 0x33, 0xB2, 0xDD, 0xD9, 0x01, 0x40, 0x00, 0x00, 0x00, 0x16 };//new byte[pcEpc.Length - epcStart];
                RcpApi2.Instance.writeToTagMemory(BitConverter.ToInt64(accessPwd, 0), epc, MEM_EPC, 2, cdf.getEpcMem());
                Console.WriteLine("Done Writing EPC to the Tag");
                System.Threading.Thread.Sleep(150);
*/
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception type: " + ex.GetType() + " Message:" + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }

            System.Threading.Thread.Sleep(1000);

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
            //MessageBox.Show(BitConverter.IsLittleEndian.ToString());
            lblPath.Text = "";
            lblSelectedFile.Text = "";
            lblOutFile.Text = "";
            lblProcessing.Text = "";
            lblProcessing.Text = "";
            lblError.Text = "";
            lblProcessingError.Text = "";
            lblProcessingSuccess.Text = "";

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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fileName = "";

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

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                lblPath.Text =  Path.GetDirectoryName(fileName);
                lblSelectedFile.Text = Path.GetFileName(fileName);
            }
            else
            {
                MessageBox.Show("Please select file to process", "Select File...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //setting up filename for output file 
            string outFileName = fileName.Substring(0, fileName.Length - 4) + "_out.csv";
            //Setting label for out file name
            lblOutFile.Text = Path.GetFileName(outFileName);

            if (File.Exists(outFileName))
            {
                DialogResult resp = MessageBox.Show("Output file already exists. Delete it?", "Delete out file",  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(resp == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(outFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error \n Cannot proceed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            //Open the file as a read only stream and check if it can be opened 
            try
            {
                fsInput = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                fsOutput = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                filesOK = true;
            }
            catch( Exception ex)
            {
                MessageBox.Show("Cannot Open Input file. \n Error: " + ex.Message, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filesOK = false;
            }

            //creating CSV File Reader and Writer
            fileReaderInput = new CsvFileReader(fsInput);
            fileWriterOutput = new CsvFileWriter(fsOutput);

            CsvRow crHeader = new CsvRow();

            //lets try to get the header of the file
            fileReaderInput.ReadRow(crHeader);

            Console.WriteLine("File header:");
            foreach (string s in crHeader)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine("Done writing header");


            //TODO::Checking for header ROW 
            //CsvRow crHeader = new CsvRow();
            //crHeader.Add("SRNO"); crHeader.Add("ACCESS_PWD"); crHeader.Add("KILL_PWD"); crHeader.Add("EPC USER"); crHeader.Add("LOCK");
            //if (Enumerable.SequenceEqual(crHeader, crInput))
            //{
            //    Console.WriteLine("HEADER OK. Processing the file.");
            //}
            //else
            //{
            //    Console.WriteLine("WRONG HEADER. Please correct and process.");
            //}
            /*
            try
            {
                using (FileStream fsInput = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    
                    using ( CsvFileReader fileReaderInput = new CsvFileReader(fsInput))
                    {
                        CsvRow crHeader = new CsvRow();
                        crHeader.Add("SRNO"); crHeader.Add("ACCESS_PWD"); crHeader.Add("KILL_PWD"); crHeader.Add("EPC USER"); crHeader.Add("LOCK");
                        CsvRow crInput = new CsvRow();
                        
                        //lets try to get the header of the file
                        fileReaderInput.ReadRow(crInput);

                        Console.WriteLine("File Header:");
                        foreach (string s in crInput)
                        {
                            Console.Write(s + " ");
                        }
                        Console.WriteLine("");


                        //TODO::Checking for header ROW 
                        //if (Enumerable.SequenceEqual(crHeader, crInput))
                        //{
                        //    Console.WriteLine("HEADER OK. Processing the file.");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("WRONG HEADER. Please correct and process.");
                        //}

                        //processing rest of the file 
                        using (FileStream fsOutput = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            Console.WriteLine("inside 1");
                            using (CsvFileWriter fileWriterOutput = new CsvFileWriter(fsOutput))
                            {
                                Console.WriteLine("inside 2");
                                while (fileReaderInput.ReadRow(crInput))
                                {
                                    CsvDataFormat cdf = new CsvDataFormat();
                                    string[] rowData = crInput.ToArray();
                                    try
                                    {
                                        cdf.srNo = rowData[0];
                                        cdf.setAccessPwd(rowData[1]);
                                        cdf.setKillPwd(rowData[2]);
                                        cdf.epcMem = rowData[3];
                                        cdf.userMem = rowData[4];
                                        cdf.lockMem = rowData[5];

                                        //Console.Write("From object: ");
                                        //Console.WriteLine(cdf.ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        cdf = null;
                                        return;
                                    }

                                    //start calling the write part 

                                    //write back the data to the output file
                                    cdf.tidMem = "2002000000";
                                    CsvRow rowToWrite = new CsvRow();
                                    for(int i=0; i< rowData.Length; i++)
                                    {
                                        rowToWrite.Add(rowData[i]);
                                    }
                                    rowToWrite.Add(cdf.tidMem);
                                    //writing to file 
                                    fileWriterOutput.WriteRow(rowToWrite);
                                }
                            }
                        }
                    }
                }
            }
            catch( Exception ex)
            {
                Console.WriteLine("Error opening selected file. \n" + ex.GetType().ToString() + "\n Message: " + ex.Message);
                MessageBox.Show("Cannot open Input file. " + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            */

            //Console.WriteLine("End of operation");

            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
