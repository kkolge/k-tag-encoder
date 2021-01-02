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
    public class SioHid : ISio
    {
        private bool disposed = false;
        public event EventHandler<byteEventArg> DataReceived;
        
        private Thread rxThread = null;           
        private static string m_strErrMsg;
        public bool bConnected = false;
        private SioConfig mSioConfig
            = SioSerializeHelper.LoadConfig();
        private string openedPort = "";

        // Logging
        //private StreamWriter sw;

        private void ThreadRead()
        {
            while (bConnected)
            {
                SioDataReceived(null, null);
            }
        }

        public void SioDataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            int status = HidPort.HID_UART_UNKNOWN_ERROR;

            ByteBuilder bb = new ByteBuilder();
            
            uint numBytesRead = 0;

            byte[] buf = new byte[1];
            try
            {
                status = HidPort.HidUart_Read(m_hidUart, buf, 1, ref numBytesRead);
            }
            catch
            {
                bConnected = false;
            }

            //if ((numBytesRead > 0) && (status == HidPort.HID_UART_SUCCESS))
            if (status == HidPort.HID_UART_SUCCESS)
            {
                bb.Append(buf[0]);

                if (numBytesRead > 1)
                {
                    byte[] newbuf = new byte[numBytesRead - 1];
                    if (HidPort.HidUart_Read(m_hidUart, newbuf, (uint)newbuf.Length, ref numBytesRead) == HidPort.HID_UART_SUCCESS)
                    {
                        bb.Append(newbuf);
                    }
                }

                //System.Console.Out.WriteLine("R>" + bb.ToString());
                Logger.Instance.LogWriteLine("R>", buf);

                if (null != DataReceived)
                    DataReceived(this, new byteEventArg(bb.GetByteArray()));
            }
            else if (status != HidPort.HID_UART_READ_TIMED_OUT)
            {
                bConnected = false;
            }
        }

        IntPtr m_hidUart;

        private bool Searching(SioConfig config)
        {
            String[] deviceList = HidPort.GetPortNames();

            if (deviceList == null || deviceList.Length == 0)
                return false;

            // set previous value as higher priority
            for (uint i = 0; i < deviceList.Length; i++)
            {
                if (config.Port == deviceList[i])
                {
                    // Open the device
                    m_hidUart = new IntPtr();

                    if (HidPort.HidUart_Open(ref m_hidUart, i, 0, 0)
                     == HidPort.HID_UART_SUCCESS)
                    {
                        bConnected = true;
                        openedPort = deviceList[i].Trim();
                        return true;
                    }
                }
            }            

            for (uint i = 0; i < deviceList.Length; i++)
            {
                if (deviceList[i] != null && deviceList[i].Trim() != "")
                {

                    // Open the device
                    m_hidUart = new IntPtr();

                    if (HidPort.HidUart_Open(ref m_hidUart, i, 0, 0) 
                        == HidPort.HID_UART_SUCCESS)
                    {
                        bConnected = true;
                        openedPort = deviceList[i].Trim();
                        return true;
                    }
                }
            }

            bConnected = false;                
            return false;
        }

        public bool IsOpened()
        {
            return bConnected;
        }

        public bool IsOpenable(SioConfig config)
        {
            if (!Searching(config))
                return false;

            Close();

            return true;
        }

        public bool Open(SioConfig config)
        {
            if (bConnected)
            {
                Close();
            }

            if (!Searching(config))
                return false;

            byte m_partNumber = 0;
            byte m_version = 0;
            uint baudRate = (uint)config.Baud;
            byte dataBits = (byte)(config.DataBits - 5);
            byte parity = (byte)config.Parity;
            byte stopBits;
            switch (config.StopBits)
            {                
                case StopBits.OnePointFive:
                case StopBits.Two:
                    stopBits = 1;
                    break;
                case StopBits.None:
                case StopBits.One:
                default:
                    stopBits = 0;
                    break;
            }            
            byte flowControl = 0;

            int status = HidPort.HidUart_GetPartNumber(m_hidUart, ref m_partNumber, ref m_version);
                        
            if (status == HidPort.HID_UART_SUCCESS)
            {
                status = HidPort.HidUart_SetUartConfig(m_hidUart, baudRate, dataBits, parity, stopBits, flowControl);
            }

            if (status == HidPort.HID_UART_SUCCESS)
            {
                status = HidPort.HidUart_SetTimeouts(m_hidUart, 10, 2000); // Read: 10 ms, Write 2000 ms
            }
            
            if (status != HidPort.HID_UART_SUCCESS)
            {
                HidPort.HidUart_Close(m_hidUart);
                return false;
            }

            bConnected = true;
            config.Port = openedPort;            
            mSioConfig = config;

            if (rxThread == null || rxThread.IsAlive == false)
            {
                rxThread = new Thread(new ThreadStart(ThreadRead));
                rxThread.Start();
            }
                        
            // Logging
            Logger.Instance.LogWriteLine("SioHid.Open()\r\n{");
            Logger.Instance.LogWriteLine(config.Port);
            Logger.Instance.LogWriteLine(config.Baud.ToString());
            Logger.Instance.LogWriteLine(config.DataBits.ToString());
            Logger.Instance.LogWriteLine(config.StopBits.ToString());
            Logger.Instance.LogWriteLine(config.Parity.ToString());            
            Logger.Instance.LogWriteLine("}");

            return true;
        }

        public SioConfig GetPortConfig()
        {
            return mSioConfig;
        }
       
        public bool Send(byte[] byData)
        {
            ByteBuilder bb = new ByteBuilder();

            bool opened = false;
            uint numBytesToWrite = Convert.ToUInt32(byData.Length);
            uint numBytesWritten = 0;

            if (HidPort.HidUart_IsOpened(m_hidUart, ref opened) == HidPort.HID_UART_SUCCESS && opened)
            {
                if (HidPort.HidUart_Write(m_hidUart, byData, numBytesToWrite, ref numBytesWritten) == HidPort.HID_UART_SUCCESS)
                {
                    bb.Append(byData);
                    Logger.Instance.LogWriteLine("T >" + bb.ToString());

                    return true;
                }
                else
                {
                    m_strErrMsg = "SIO : write fail";
                    bConnected = false;

                    Logger.Instance.LogWriteLine(m_strErrMsg);

                    return false;
                }
            }
            else
            {
                bConnected = false;
            }

            return false;
        }

        public bool Send(string strData)
        {
            try
            {
                byte[] bytes = new byte[strData.Length * sizeof(char)];
                System.Buffer.BlockCopy(strData.ToCharArray(), 0, bytes, 0, bytes.Length);

                return Send(bytes);
            }
            catch
            {
                m_strErrMsg = "SIO: write fail";
                bConnected = false; 

                return false;
            }
        }

        public void Close()
        {   
            bConnected = false;
            openedPort = "";
            try
            {
                if (rxThread != null && rxThread.IsAlive)
                    rxThread.Abort();
            }
            catch
            {
                
            }
            finally
            {
                if(rxThread != null)
                    rxThread.Join();
            }

            if (HidPort.HidUart_Close(m_hidUart) != HidPort.HID_UART_SUCCESS) { }
            {
                m_strErrMsg = "Failed to disconnect";
                Logger.Instance.LogWriteLine(m_strErrMsg);
            }

            // Logging
            Logger.Instance.LogWriteLine("SioHid.Close()");       
        }

        public void Flush()
        {
            //int byteToRead = Sio.Instance.oSp.BytesToRead;
            //if (byteToRead != 0) Sio.Instance.oSp.ReadExisting();
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
