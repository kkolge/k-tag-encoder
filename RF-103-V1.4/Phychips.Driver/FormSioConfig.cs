//! Copyright (C) 2007 Phychips
//! 
//! Phychips.Driver
//!
//! Description
//! 	Phychips.Driver
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using Phychips.Helper;
using System.Runtime.InteropServices;

namespace Phychips.Driver
{            
    public partial class FormSioConfig : Form
    {
        private SioConfig mConfigData = null;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed 
        private const int WM_DEVICECHANGE = 0x0219;
        //public event EventHandler<StringEventArg> SioDeviceAdded;
        //public event EventHandler<StringEventArg> SioDeviceRemoved;
        string[] m_strListVcpPort;
        string[] m_strListHidPort;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);
        public FormSioConfig()
        {
            InitializeComponent();            
        }
        
        public SioConfig Config
        {
            get { return mConfigData; }
            set { mConfigData = value; }
        }
                        
        private void FormSioConfig_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            ///////////////
            BroadcastHeader data = new BroadcastHeader();
            data.Type = DeviceType.Port;
            data.Size = Marshal.SizeOf(data);

            try
            {
                IntPtr buffer = Marshal.AllocHGlobal(data.Size);
                Marshal.StructureToPtr(data, buffer, true);
                RegisterDeviceNotification(this.Handle, buffer, 0);
            }
            catch
            {

            }

            m_strListHidPort = HidPort.GetPortNames();
            m_strListVcpPort = SerialPort.GetPortNames();
            ////////////////

            bool selected = false;
            if (mConfigData == null)
            {
                mConfigData = SioSerializeHelper.LoadConfig();
            }

            foreach (string port in m_strListHidPort)            
            {
                if (port == null || port.Trim().Length == 0)
                    continue;
                
                if(this.comboBox_UARTConfiguration_Port.Items.Count == 0)
                    this.comboBox_UARTConfiguration_Port.Items.Add(port);
                else if((string) this.comboBox_UARTConfiguration_Port.Items[this.comboBox_UARTConfiguration_Port.Items.Count-1] != port)
                    this.comboBox_UARTConfiguration_Port.Items.Add(port);

                if (port == mConfigData.Port)
                {
                    selected = true;
                    this.comboBox_UARTConfiguration_Port.SelectedText = mConfigData.Port;
                    //break;
                }

                Debug.Write(port+" ");
            }
            Debug.Write("\r\n");

            foreach (string port in m_strListVcpPort)
            {
                if (port == null || port.Trim().Length == 0)
                    continue;

                if (this.comboBox_UARTConfiguration_Port.Items.Count == 0)
                    this.comboBox_UARTConfiguration_Port.Items.Add(port);
                else if ((string)this.comboBox_UARTConfiguration_Port.Items[this.comboBox_UARTConfiguration_Port.Items.Count - 1] != port)
                    this.comboBox_UARTConfiguration_Port.Items.Add(port);

                if (port == mConfigData.Port && !selected)
                {
                    this.comboBox_UARTConfiguration_Port.SelectedText = mConfigData.Port;
                    //break;
                }

                Debug.Write(port + " ");
            }
            Debug.Write("\r\n");

            if (!selected && this.comboBox_UARTConfiguration_Port.Items.Count != 0)
            {
                this.comboBox_UARTConfiguration_Port.SelectedIndex = 0;
            }

            //UpdateDeviceList();

            this.comboBox_UARTConfiguration_Baudrate.Text = mConfigData.Baud.ToString();
            this.comboBox_UARTConfiguration_DataBits.Text = mConfigData.DataBits.ToString();
            this.comboBox_UARTConfiguration_Parity.Text = mConfigData.Parity.ToString();

            switch (mConfigData.StopBits)
            {
                case StopBits.One:                    
                    comboBox_UARTConfiguration_StopBits.Text = "1";
                    break;
                case StopBits.OnePointFive:
                    comboBox_UARTConfiguration_StopBits.Text = "1.5";                    
                    break;
                case StopBits.Two:                    
                    comboBox_UARTConfiguration_StopBits.Text = "2";                    
                    break;
            }
        }

        private void SaveSioData(object sender, EventArgs e)
        {
            SioSerializeHelper.SaveConfig(mConfigData);         
        }

        private void comboBox_UARTConfiguration_Port_SelectedIndexChanged(object sender, EventArgs e)
        {
            mConfigData.Port = comboBox_UARTConfiguration_Port.Text;
        }
        
        private void comboBox_UARTConfiguration_Baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mConfigData.Baud = int.Parse(comboBox_UARTConfiguration_Baudrate.Text);
            }
            catch
            {

            }
        }

        private void comboBox_UARTConfiguration_DataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                mConfigData.DataBits = int.Parse(comboBox_UARTConfiguration_DataBits.Text);
            }
            catch
            {

            }
        }

        private void comboBox_UARTConfiguration_Parity_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox_UARTConfiguration_Parity.Text)
            {
                case "None":
                    mConfigData.Parity = System.IO.Ports.Parity.None;
                    break;
                case "Odd":
                    mConfigData.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "Even":
                    mConfigData.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "Mark":
                    mConfigData.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case "Space":
                    mConfigData.Parity = System.IO.Ports.Parity.Space;
                    break;
            }
        }

        private void comboBox_UARTConfiguration_StopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_UARTConfiguration_StopBits.Text)
            {
                case "1":
                    mConfigData.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    mConfigData.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    mConfigData.StopBits = System.IO.Ports.StopBits.Two;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SioSerializeHelper.SaveConfig(mConfigData);            
        }


        private void AddPort(string port)
        {
            string selectedPort
                = this.comboBox_UARTConfiguration_Port.Text;

            if (port == null)
                return;

            this.comboBox_UARTConfiguration_Port.Items.Add(port);
            if (selectedPort.Length == 0)
            {
                this.comboBox_UARTConfiguration_Port.SelectedText = port;
            }

        }

        private void RemovePort(string port)
        {   
            string selectedPort
                = this.comboBox_UARTConfiguration_Port.Text;

            this.comboBox_UARTConfiguration_Port.Items.Remove(port);

            if (selectedPort == port)
            {
                if (this.comboBox_UARTConfiguration_Port.Items.Count == 0)
                {
                    this.comboBox_UARTConfiguration_Port.SelectedText = "";
                }
                else
                {
                    this.comboBox_UARTConfiguration_Port.SelectedIndex = 0;
                }
            }
        }
                
        protected override void WndProc(ref Message m)
        {
            if (this.DesignMode) return;

            //BroadcastHeader lBroadcastHeader;
            //Port IPort;
            //DeviceEvent lEvent;
            WM_DEVICECHANGE_WPPARAMS lEvent;

            base.WndProc(ref m);
            if (m.Msg == WM_DEVICECHANGE)
            {
                Debug.WriteLine(m);

                //lEvent = (DeviceEvent)m.WParam.ToInt32();
                lEvent = (WM_DEVICECHANGE_WPPARAMS)m.WParam.ToInt32();
                //if (lEvent == DeviceEvent.RemoveComplete)
                switch (lEvent)
                {
                    /*
                    case WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEREMOVECOMPLETE:
                        {
                            System.Console.WriteLine("WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEREMOVECOMPLETE");
                            lBroadcastHeader = (BroadcastHeader)Marshal.PtrToStructure(m.LParam, typeof(BroadcastHeader));
                            if (lBroadcastHeader.Type == DeviceType.Port)
                            {
                                IPort = (Port)Marshal.PtrToStructure(m.LParam, typeof(Port));

                                if (SioDeviceRemoved != null) SioDeviceRemoved(this, new StringEventArg(IPort.Name));
                            }
                            break;
                        }
                    */
                    case WM_DEVICECHANGE_WPPARAMS.DBT_DEVNODES_CHANGED:
                        {
                            System.Console.WriteLine("WM_DEVICECHANGE_WPPARAMS.DBT_DEVNODES_CHANGED");

                            

                            string[] listHidPort = HidPort.GetPortNames();
                            if (m_strListHidPort.Length != listHidPort.Length)
                            {
                                string evtPort = SioHelper.compStringArray(m_strListHidPort, listHidPort);
                                if (m_strListHidPort.Length < listHidPort.Length)
                                {
                                    System.Console.WriteLine("HID port [{0}] added", evtPort);
                                    AddPort(evtPort);        
                                }
                                else
                                {
                                    System.Console.WriteLine("HID port [{0}] removed", evtPort);
                                    RemovePort(evtPort);
                                }
                                m_strListHidPort = listHidPort;
                                break;
                            }

                            string[] listVcpPort = SerialPort.GetPortNames();
                            if (m_strListVcpPort.Length != listVcpPort.Length)
                            {
                                string evtPort = SioHelper.compStringArray(m_strListVcpPort, listVcpPort);

                                if (m_strListVcpPort.Length < listVcpPort.Length)
                                {
                                    System.Console.WriteLine("VCP port [{0}] added", evtPort);
                                    AddPort(evtPort);
                                }
                                else
                                {
                                    System.Console.WriteLine("VCP port [{0}] removed", evtPort);
                                    RemovePort(evtPort);
                                }
                                m_strListVcpPort = listVcpPort;
                            }

                            break;
                        }
                }
            }
        }
                
    }
}
