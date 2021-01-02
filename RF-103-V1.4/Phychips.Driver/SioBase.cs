//! Copyright (C) 2007 Phychips
//! 
//! Sio.cs
//!
//! Description
//! 	Serial Port Driver
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO.Ports;
using System.IO;
using Phychips.Helper;
using Microsoft.Win32;
using System.Threading;
using System.Management;

namespace Phychips.Driver
{
    public class SioBase : IDisposable
    {
        protected enum SioType
        {
            SIO_BOTH,
            SIO_VCP,
            SIO_HID            
        }

        public event EventHandler<byteEventArg> DataReceived;
        public event EventHandler<StringEventArg> DeviceRemoved;
        public event EventHandler<StringEventArg> DeviceAdded;

        //private Thread rxThread = null;
        //public SerialPort oSp = new SerialPort();
        private SioHideForm mSioEvent = new SioHideForm();        
        private static string m_strErrMsg = null;
        //public bool bConnected = false;
        private SioConfig mSioConfig = new SioConfig();
        private SioType mSioType = SioType.SIO_BOTH;
        
        public SioConfig SioConfig
        {
            get { return mSioConfig; }
            //set { mSioConfig = value; }
        }
        private ISio mSio;
        
        protected SioBase()
        {            
            //EmbeddedDll.Load("Phychips.Driver.SLABHIDDevice.dll", "SLABHIDDevice.dll");
            //EmbeddedDll.Load("Phychips.Driver.SLABHIDtoUART.dll", "SLABHIDtoUART.dll");
            mSioConfig = SioSerializeHelper.LoadConfig();
            mSioEvent.Show();
            mSioEvent.Hide(); //mFormConfig.Visible = false;                                    
            mSioEvent.SioDeviceRemoved += new EventHandler<StringEventArg>(SioDeviceRemoved);
            mSioEvent.SioDeviceAdded += new EventHandler<StringEventArg>(SioDeviceAdded);
        }

        protected void setType(SioType type)
        {
            mSioType = type;
            mSioEvent.setType((int)type);
        }
                
        private void SioDeviceAdded(object sender, StringEventArg e)
        {
            if(this.DeviceAdded != null && !IsOpened())
            {
                this.DeviceAdded(this, e);
            }
        }

        private void SioDeviceRemoved(object sender, StringEventArg e)
        {
            //System.Console.WriteLine(this.ToString() + "::" + "DeviceRemoved");

            if (mSio != null)            
            {                
                if(mSio.GetPortConfig() != null && mSio.GetPortConfig().Port == e.Data)
                {
                    Close();                    
                }
                
                if (this.DeviceRemoved != null) this.DeviceRemoved(this, e);
                // Logging
                Logger.Instance.LogWriteLine("Sio.Instance.Close();"); 
            }
        }

        //private int test = 0;
        private void SioDataReceived(object sender, byteEventArg e)
        {
            //Console.Out.WriteLine("OnDataReceived" + (test++).ToString() );
            if (DataReceived != null)
                DataReceived(sender, e);
        }

        public bool IsOpened()
        {
            if (mSio == null)
                return false;

            return mSio.IsOpened();
        }

        public string StrErrMsg
        {
            get { return m_strErrMsg; }
        }

        public string strConnectionStatus
        {
            get
            {
                if (mSio == null) return "CLOSE";
                if (!mSio.IsOpened()) return "CLOSE";
                else return "OPEN";                
            }
        }
        
        public void ShowFormConfig()
        {
            FormSioConfig form = new FormSioConfig();

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SioSerializeHelper.SaveConfig(form.Config);

                // Logging
                Logger.Instance.LogWriteLine(form.Config.ToString());
            }   
        }

        public bool IsOpenable()
        {
            bool ret = false;

            if (mSio != null && mSio.IsOpened())
            {
                mSio.Close();
            }

            if (mSio != null)
                mSio.Dispose();

            if (mSioType == SioType.SIO_BOTH || mSioType == SioType.SIO_HID)
            {
                mSio = new SioHid();
                ret = mSio.IsOpenable(mSioConfig);
            }

            if (!ret
                && (mSioType == SioType.SIO_BOTH || mSioType == SioType.SIO_VCP) )
            {
                if (mSio != null)
                    mSio.Dispose();

                mSio = new SioVcp();
                ret = mSio.IsOpenable(mSioConfig);
            }

            if (mSio != null)
                mSio.Dispose();
            
            return ret;
        }

        public bool Open()
        {
            bool ret = false;

            if (mSio != null && mSio.IsOpened())
                return true;
            
            System.Console.WriteLine("Sio.Open()");


// >> 20191203, HYO
            mSioConfig = SioSerializeHelper.LoadConfig();
// << 20191203, HYO


            if(mSioConfig.Port.Contains("COM")
                && (mSioType == SioType.SIO_BOTH || mSioType == SioType.SIO_VCP)) // saved Vcp port only
            {
                if (mSio != null)                
                    mSio.Dispose();
                
                mSio = new SioVcp();                
                ret = mSio.Open(mSioConfig);               
            }
            
            if (!ret
                && (mSioType == SioType.SIO_BOTH || mSioType == SioType.SIO_HID)) // saved Hid port, then any Hid port
            {
                if (mSio != null)
                    mSio.Dispose();
                                
                mSio = new SioHid();                
                ret = mSio.Open(mSioConfig);                                
            }
            
            if (!ret
                && (mSioType == SioType.SIO_BOTH || mSioType == SioType.SIO_VCP)) // any Vcp port
            {
                if (mSio != null)
                    mSio.Dispose();

                mSio = new SioVcp();
                ret = mSio.Open(mSioConfig);                
            }

            if (ret)
            {
                SioSerializeHelper.SaveConfig(mSio.GetPortConfig());
                mSio.DataReceived += SioDataReceived;                
            }

            return ret;
        }

        public bool Send(byte[] byData)
        {
            bool ret = mSio.Send(byData);

            if (!ret)
                Close();

            return ret;
        }

        public bool Send(string strData)
        {
            //try
            {
                byte[] bytes = new byte[strData.Length * sizeof(char)];
                System.Buffer.BlockCopy(strData.ToCharArray(), 0, bytes, 0, bytes.Length);
                return Send(bytes);
            }
            /*
            catch
            {
                //mSio.Close();
                m_strErrMsg = "SIO: write fail";
                return false;
            }
            */
        }

        public void Close()
        {
            System.Console.WriteLine("Sio.Close()");
            if (mSio.IsOpened())
            {
                mSio.Close();
                mSio.DataReceived -= SioDataReceived;                
            }
        }

        public void Flush()
        {
            mSio.Flush();
        }
        
        #region IDisposable 
        public void Dispose()
        {
            if (mSio != null)
                mSio.Dispose();

            Logger.Instance.LogClose();
            
            //oSp.DataReceived -= SioDataReceived;            
            mSioEvent.SioDeviceRemoved -= new EventHandler<StringEventArg>(SioDeviceRemoved);
            mSioEvent.SioDeviceAdded -= new EventHandler<StringEventArg>(SioDeviceAdded);      
            this.Close();
            mSioEvent.Dispose();
            
            GC.SuppressFinalize(this);
        }
        #endregion        
    }
}
