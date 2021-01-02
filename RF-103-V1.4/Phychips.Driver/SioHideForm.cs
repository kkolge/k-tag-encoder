using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Diagnostics;
using Phychips.Helper;
using System.Runtime.InteropServices;

namespace Phychips.Driver
{
        
    public enum WM_DEVICECHANGE_WPPARAMS : int
    {
        DBT_DEVICEARRIVAL = 0x8000,
        DBT_DEVICEQUERYREMOVE = 0x8001,
        DBT_DEVICEREMOVECOMPLETE = 0x8004,
        DBT_CONFIGCHANGECANCELED = 0x19,
        DBT_CONFIGCHANGED = 0x18,
        DBT_CUSTOMEVENT = 0x8006,
        DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
        DBT_DEVICEREMOVEPENDING = 0x8003,
        DBT_DEVICETYPESPECIFIC = 0x8005,
        DBT_DEVNODES_CHANGED = 0x7,
        DBT_QUERYCHANGECONFIG = 0x17,
        DBT_USERDEFINED = 0xFFFF
    }

    public enum DeviceType : int
    {
        OEM = 0x00000000,           //DBT_DEVTYP_OEM
        DeviceNode = 0x00000001,    //DBT_DEVTYP_DEVNODE
        Volume = 0x00000002,        //DBT_DEVTYP_VOLUME
        Port = 0x00000003,          //DBT_DEVTYP_PORT
        Net = 0x00000004,           //DBT_DEVTYP_NET
        Interface = 0x00000005      //DBT_DEVTYP_DEVICEINTERFACE
    }
        
    public enum VolumeFlags : int
    {
        Media = 0x0001,             //DBTF_MEDIA
        Net = 0x0002                //DBTF_NET
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BroadcastHeader   //_DEV_BROADCAST_HDR 
    {
        public int Size;            //dbch_size
        public DeviceType Type;     //dbch_devicetype
        private int Reserved;       //dbch_reserved
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Volume            //_DEV_BROADCAST_VOLUME 
    {
        public int Size;            //dbcv_size
        public DeviceType Type;     //dbcv_devicetype
        private int Reserved;       //dbcv_reserved
        public int Mask;            //dbcv_unitmask
        public int Flags;           //dbcv_flags
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct Port 
    {
        public int Size;            //dbcv_size
        public DeviceType Type;     //dbcv_devicetype
        private int Reserved;       //dbcv_reserved
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string Name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct Interface
    {
        public int Size;
        public DeviceType Type;     //dbcv_devicetype
        public int Reserved;
        public Guid guid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string Name;
    }
    
    public class SioHideForm : Form
    {
        private enum SioType : int
        {
            SIO_BOTH,
            SIO_VCP,
            SIO_HID            
        }

        private SioConfig mConfigData = null;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed 
        private const int WM_DEVICECHANGE = 0x0219;
        public event EventHandler<StringEventArg> SioDeviceAdded;
        public event EventHandler<StringEventArg> SioDeviceRemoved;
        string[] m_strListVcpPort;
        string[] m_strListHidPort;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        private int mSioType = (int)SioType.SIO_BOTH;
        public SioHideForm()
        {
            InitializeComponent();
            //FormClosing += SaveSioData;
        }
        
        public void setType(int type)
        {
            mSioType = type;
        }

        public SioConfig Config
        {
            get { return mConfigData; }
            set { mConfigData = value; }
        }

        private void SioHideForm_Load(object sender, EventArgs e)
        {            
            Interface data = new Interface();
            data.Type = DeviceType.Interface;            
            data.Size = Marshal.SizeOf(data);
            data.guid = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
            data.Reserved = 0;

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
        }

        ~SioHideForm()
        {
            try
            {
                UnregisterDeviceNotification(this.Handle);
            }
            catch
            {
            }
        }

        private void AddPort(string port)
        {
            if (SioDeviceAdded != null) SioDeviceAdded(this, new StringEventArg(port));
        }

        private void RemovePort(string port)
        {   
            if (SioDeviceRemoved != null) SioDeviceRemoved(this, new StringEventArg(port));
        }
        protected override void WndProc(ref Message m)
        {
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
                    case WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEARRIVAL:
                        { 
                            Interface data = (Interface) Marshal.PtrToStructure(m.LParam, typeof(Interface));
                            System.Console.WriteLine("WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEARRIVAL " + data.Name );                        
                            break;
                        }
                    case WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEREMOVECOMPLETE:
                        { 
                            Interface data = (Interface) Marshal.PtrToStructure(m.LParam, typeof(Interface));
                            System.Console.WriteLine("WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEREMOVECOMPLETE" + data.Name);
                            break;
                        }
                    case WM_DEVICECHANGE_WPPARAMS.DBT_DEVNODES_CHANGED:
                        {
                            System.Console.WriteLine("WM_DEVICECHANGE_WPPARAMS.DBT_DEVNODES_CHANGED");

                            if ((SioType)mSioType == SioType.SIO_BOTH || (SioType)mSioType == SioType.SIO_HID)
                            {
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
                            }
                               
                            if ((SioType)mSioType == SioType.SIO_BOTH || (SioType)mSioType == SioType.SIO_VCP)
                            {
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
                            }
                            break;
                        }
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SioHideForm
            // 
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SioHideForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.SioHideForm_Load);
            this.ResumeLayout(false);
        }
        
        
    }
}
