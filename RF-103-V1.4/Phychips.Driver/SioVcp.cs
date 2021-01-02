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

namespace Phychips.Driver
{
    public class SioVcp : ISio
    {
        private bool disposed = false;
        public event EventHandler<byteEventArg> DataReceived;                
        public SerialPort mVcp = new SerialPort();        
        private static string m_strErrMsg;
        public bool bConnected = false;
        public SioConfig mSioConfig;

        public SioVcp()
        {   
            mVcp.DataReceived += SioDataReceived;            
        }

        public bool IsOpened()
        {
            return bConnected;
        }

        public void SioDataReceived(object sender, SerialDataReceivedEventArgs e) 
        {            
            int length = mVcp.BytesToRead;

            if (length == 0) return;

            byte[] buf = new byte[length];
            
            try
            {
                mVcp.Read(buf, 0, length);
            }
            catch
            {
                m_strErrMsg = "SIO: read fail";                
                bConnected = false;
                return;
            }

            Logger.Instance.LogWriteLine("R>",buf);
            
            if (null != DataReceived)
                DataReceived(this, new byteEventArg(buf));         
        }

        public bool IsOpenable(SioConfig config)
        {
            if (Searching(config))
            {
                mVcp.PortName = config.Port;
                try
                {                    
                    mVcp.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[mVcp.TryOpen()]" + ex.ToString());
                    return false;
                }
            }

            if (!mVcp.IsOpen)
            {
                string[] ports = SerialPort.GetPortNames();

                foreach (string aport in ports)
                {
                    mVcp.PortName = aport;

                    try
                    {
                        mVcp.Open();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("2. mVcp.Open()" + ex.ToString());
                        continue;
                    }

                    if (mVcp.IsOpen)
                    {
                        config.Port = aport;
                        break;
                    }
                }
            }

            if (mVcp.IsOpen)
            {
                mVcp.Close();
                return true;
            }
            return false;
        }

        public bool Open(SioConfig config)
        {
            if (bConnected)
                return true;

            /*
            if(oSp == null) oSp = new SerialPort();

            if (oSp.IsOpen) return true;

            string[] cur_ports = SerialPort.GetPortNames();
            bool findPort = false;
            
            foreach(string cur_port in cur_ports)
            {
                if (cur_port.IndexOf(oSioData.Port) != -1) findPort = true;
            }

            if (!findPort)
            {
                //throw new Exception(string.Format("Failure to fine serial port {0}", oSioData.Port));
                return false;
            }
            */

            mVcp.PortName = config.Port; 
            mVcp.BaudRate = config.Baud;
            mVcp.DataBits = config.DataBits;
            mVcp.StopBits = config.StopBits;
            mVcp.Parity = config.Parity;
            mVcp.ReadTimeout = System.IO.Ports.SerialPort.InfiniteTimeout;            
            mVcp.Handshake = Handshake.None;
            mVcp.DiscardNull = false;

            if (Searching(config))
            {
                mVcp.PortName = config.Port;
                try
                {                    
                    mVcp.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("1. mVcp.Open()" + ex.ToString());
                }
            }

            if (!mVcp.IsOpen)
            {
                string[] ports = SerialPort.GetPortNames();

                foreach (string aport in ports)
                {
                    mVcp.PortName = aport;

                    try
                    {
                        mVcp.Open();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("2. mVcp.Open()" + ex.ToString());
                        continue;
                    }

                    if (mVcp.IsOpen)
                    {
                        config.Port = aport;
                        break;
                    }
                }
            }

            if(!mVcp.IsOpen)
            {
                bConnected = false;
                return false;
            }

            mSioConfig = config;
            bConnected = true;
            
            // Logging
            Logger.Instance.LogWriteLine("SioVcp.Open()\r\n{");
            Logger.Instance.LogWriteLine(mVcp.PortName);
            Logger.Instance.LogWriteLine(mVcp.BaudRate.ToString());
            Logger.Instance.LogWriteLine(mVcp.DataBits.ToString());
            Logger.Instance.LogWriteLine(mVcp.StopBits.ToString());
            Logger.Instance.LogWriteLine(mVcp.Parity.ToString());
            Logger.Instance.LogWriteLine(mVcp.ReadTimeout.ToString());
            Logger.Instance.LogWriteLine(mVcp.DiscardNull.ToString());
            Logger.Instance.LogWriteLine(mVcp.Handshake.ToString());
            Logger.Instance.LogWriteLine("}");              

            return true;
        }

        public SioConfig GetPortConfig()
        {
            return mSioConfig;
        }
        
        public bool Send(byte[] byData)
        {    
            try
            {  
                mVcp.Write(byData, 0, byData.Length);
                // Logging                
                Logger.Instance.LogWriteLine("T>", byData);
            }
            catch
            {
                m_strErrMsg = "SIO: write fail";                
                bConnected = false;                
                Logger.Instance.LogWriteLine(m_strErrMsg);
                return false;
            }

            return true;
        }

        public void Close()
        {
            bConnected = false;
            if (mVcp == null) return;

            if (mVcp.IsOpen) mVcp.Close();
            
            // Logging
            Logger.Instance.LogWriteLine("oSp.Close()");
        }
        
        public bool SearchSilabVcp(string port)
        {
            /*
             * Ketan Commented for CH340G
             * 
            //HKEY_LOCAL_MACHINE,"HARDWARE\\DEVICEMAP\\SERIALCOMM
            string device1;
            string device2;

            RegistryKey reg;

            device1 = @"\Device\Silabser";
            device2 = @"\Device\Slabser";
            reg = Registry.LocalMachine.OpenSubKey("HARDWARE").OpenSubKey("DEVICEMAP").OpenSubKey("SERIALCOMM");

            foreach (String regName in reg.GetValueNames())
            {
                try
                {
                    if ((string)reg.GetValue(regName) == port)
                    {
                        if ((string.Compare(regName, 0, device1, 0, device1.Length, true) == 0)
                            || (string.Compare(regName, 0, device2, 0, device2.Length, true) == 0))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return false;
            *
            * //Ketan Comment end. always returning true
            */
            return true;
        }

        private bool Searching(SioConfig config)
        {
            string[] ports = SerialPort.GetPortNames();
            
            foreach (string aport in ports)
            {
                if (SearchSilabVcp(aport) && config.Port == aport)
                    return true;               
            }

            return false;
        }
        
        public void Flush()
        {
            int byteToRead = mVcp.BytesToRead;
            if (byteToRead != 0) mVcp.ReadExisting();
        }

        #region IDisposable ¸â¹ö
        public void Dispose()
        {   
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                mVcp.DataReceived -= SioDataReceived;
                this.Close();
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        #endregion
    }
}
