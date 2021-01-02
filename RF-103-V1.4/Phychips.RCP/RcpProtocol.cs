//! Copyright (C) 2007 Phychips
//! 
//! RcpProtocol.cs
//!
//! Description
//! 	RCP protocol
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.Text;
using Phychips.Helper;
using Phychips.Driver;
using System.IO.Ports;

using System.Threading;


namespace Phychips.Rcp
{
    public class RcpProtocol
    {
        // Single Tone
        private readonly static RcpProtocol m_oInstance = new RcpProtocol();
        public static RcpProtocol Instance
        {
            get
            {   
                return m_oInstance;
            }
        }

        private object lockObj = new object();
        public const byte PREAMBLE = 0xBB;
        public const byte ENDMARK = 0x7E;
        public const byte HEADER_CMD = 0x00;

        public const byte RCP_MSG_CMD = 00;  // Command Type
        public const byte RCP_MSG_RSP = 01;  // Response Type
        public const byte RCP_MSG_NOTI = 02; // Notification Type
        public const byte RCP_MSG_TEST = 03; // Test Type
        public const byte RCP_MSG_CAP = 04;  // Test Type
                
        public const byte ERR_CRC = 1;
        public const byte ERR_NACK = 2;
        public const byte ERR_TIME_OUT = 4;

        public enum InfoType : byte
        {
            RDR_MODEL = 0x00,
            RDR_SN = 0x01,
            RDR_MANFACT = 0x02,
            RDR_FREQ = 0x03,
            RDR_TAG_TYPE = 0x04,
            RDR_FW_TIME = 0xA0,
            RDR_CHIP_VER = 0xA1
        }
        
        private ByteBuilder m_oByteRxPkt = new ByteBuilder();

        private volatile int nErrorCode = 0;
        private volatile bool m_bRcpReceivedPacket = false;
        private volatile bool m_bRcpReceivedPacketCrcError = false;
        
        private byte[] dump_data = new byte[16];
        
        public event EventHandler<StringEventArg> SioEventMsg;        
        public event EventHandler<byteEventArg> RxRspParsed;
        public event EventHandler<byteEventArg> RxCaptureMsg;
        public event EventHandler<StringEventArg> EventPortClosed;
        public event EventHandler<StringEventArg> EventPortOpened;
        private event EventHandler<StringEventArg> mRcpLogEventReceived;

        private static Mutex mut = new Mutex();

        public event EventHandler<StringEventArg> RcpLogEventReceived
        {
            add
            {
                lock (lockObj)
                {
                    if (mRcpLogEventReceived == ConsoleLogger)                    
                        mRcpLogEventReceived -= ConsoleLogger;
                                        
                    if (mRcpLogEventReceived == null)                    
                        mRcpLogEventReceived += value;                    
                }
            }
            remove
            {
                lock (lockObj)
                {
                    if (mRcpLogEventReceived != null)
                        mRcpLogEventReceived -= value;
                }
            }
        }

        private void ConsoleLogger(object sender, StringEventArg e)
        {
            System.Console.WriteLine(e.Data);
        }

        private RcpProtocol()
        {
            System.Console.WriteLine("RcpProtocol()");
            Sio.Instance.DataReceived += SioDataReceived;
            Sio.Instance.DeviceRemoved += SioDeviceRemoved;
            Sio.Instance.DeviceAdded += SioDeviceAdded;
            mRcpLogEventReceived += ConsoleLogger;
        }

        private void SioDeviceAdded(object sender, StringEventArg e)
        {
            if(EventPortOpened != null)
                EventPortOpened(sender, e);
        }

        private void SioDeviceRemoved(object sender, StringEventArg e)
        {
            System.Console.WriteLine(this.ToString() + "::" + "SioDeviceRemoved");
            if (EventPortClosed != null)
                EventPortClosed(sender, e);
        }

        public int GetErrCode()
        {
            if (m_bRcpReceivedPacketCrcError)
                nErrorCode |= ERR_CRC;
                        
            if (!m_bRcpReceivedPacket)
                nErrorCode |= ERR_TIME_OUT;

            return nErrorCode;
        }

        public bool Open()
        {
            m_oByteRxPkt.Clear();
            return Sio.Instance.Open();
        }

        public bool IsOpenable()
        {
            return Sio.Instance.IsOpenable();
        }

        public void Close()
        {
            Sio.Instance.Close();
        }

        public bool IsOpened()
        {
            return Sio.Instance.IsOpened();
        }


// >> 20191203, HYO
        public void ShowFormConfig()
        {
            Sio.Instance.ShowFormConfig();
        }
// << 20191203, HYO


        public byte[] BuildCmdPacketByte(byte nMsg, byte nCmd, byte[] ArgByte)
        {
            ByteBuilder bb = new ByteBuilder();
            UInt16 ArgLen;
           

            if (ArgByte != null)
            {
                ArgLen = (UInt16)ArgByte.Length;
            }
            else
            {
                ArgLen = 0;
            }

            // Byte Packet Build
            bb.Append(PREAMBLE);
            bb.Append(nMsg);
            bb.Append(nCmd);
            bb.Append(ArgLen);
            bb.Append(ArgByte);
            bb.Append(ENDMARK);
            
            return bb.GetByteArray();
        }

        public bool SendBytePkt(byte[] pkt)
        {
            if (!SendBytePktNoMsg(pkt))
            {
                if (mRcpLogEventReceived != null) 
                    mRcpLogEventReceived(this, new StringEventArg("ERROR > Failure to receive response message ..."));

                return false;
            }

            if (SioEventMsg != null) SioEventMsg(this, new StringEventArg("CMD > Send data to SIO\r\n"));

            return true;
        }

        public byte[] GetDump()
        {
            return dump_data;
        }

        public bool SendBytePktRaw(byte[] pkt)
        {   
            if (!Sio.Instance.Send(pkt))
            {
                if (SioEventMsg != null)
                    SioEventMsg(this, new StringEventArg("ERR > Failure to send data to SIO...\r\n"));
            }

            ByteBuilder bb = new ByteBuilder();
            bb.Append(pkt);
            RcpLog(bb);

            return true;
        }

        public bool SendBytePktNoMsg(byte[] pkt)
        {   
            ByteBuilder bb = new ByteBuilder();

            bb.Append(pkt);
            bb.Append(CRCCalculator.Cal_CRC16(pkt));

            mut.WaitOne();
                        
            m_bRcpReceivedPacket = false;
            m_bRcpReceivedPacketCrcError = false;
            nErrorCode = 0;

            if(!Sio.Instance.Send(bb.GetByteArray()))                    
            {
                if (SioEventMsg != null) 
                    SioEventMsg(this, new StringEventArg("ERR > Failure to send data to SIO...\r\n"));
            }
                        
            RcpLog(bb);
            
            int count = 0;
            while (!m_bRcpReceivedPacket && !m_bRcpReceivedPacketCrcError)
            {
                if (count > 100)                
                {
                    mut.ReleaseMutex();
                    return false;
                }

                count++;
                System.Threading.Thread.Sleep(10);
            }

            if (m_bRcpReceivedPacketCrcError)
            {
                mut.ReleaseMutex();
                return false;
            }

            mut.ReleaseMutex();

            return true;
        }
                
        private void SioDataReceived(object sender, byteEventArg e)
        {
            int length = e.Data.Length;
            if (length == 0) return;

            m_oByteRxPkt.Append(e.Data);
                        
            int i = 0;
            for (i = 0; i < m_oByteRxPkt.Length; i++)
            {
                if (m_oByteRxPkt.GetAt(i) == RcpProtocol.PREAMBLE) break;
            }

            if (0 != i)
            {
                ByteBuilder bb = new ByteBuilder();
                bb.Append(m_oByteRxPkt.GetByteArray(i, m_oByteRxPkt.Length - i));
                m_oByteRxPkt.Clear();
                m_oByteRxPkt.Append(bb.GetByteArray());
            }
                        
            while(m_oByteRxPkt.Length > 8)
            {
                //int len = (m_oByteRxPkt.GetByte(3) << 8) + m_oByteRxPkt.GetByte(4);
                int len = m_oByteRxPkt.GetAt(4);
                if (m_oByteRxPkt.Length >= len + 8)
                {
                    if (m_oByteRxPkt.GetAt(len + 5) == RcpProtocol.ENDMARK)
                    {
                        ByteBuilder bb = new ByteBuilder(m_oByteRxPkt.GetByteArray(0, len + 8));

                        //Check CRC                        
                        if (CRCCalculator.Cal_CRC16(bb.GetByteArray()) != 0x0000)
                        {
                            m_bRcpReceivedPacketCrcError = true;
                        }
                        
                        if (!RcpProtocol.Instance.ParseRxData(bb))
                        {
                            m_oByteRxPkt.Clear();
                            return;
                        }

                        RcpLog(bb);

                        bb.Clear();

                        if ((m_oByteRxPkt.Length - (len + 8)) > 0)
                            bb.Append(m_oByteRxPkt.GetByteArray(len + 8, m_oByteRxPkt.Length - (len + 8)));

                        m_oByteRxPkt.Clear();
                        m_oByteRxPkt.Append(bb.GetByteArray());
                    }
                    else
                    {
                        m_oByteRxPkt.Clear();
                    }
                }
                else
                {
                    break;
                }                
            }
        }

        public bool ParseRxData(ByteBuilder bb)
        {
            switch (bb.GetAt(1)) //msg_type
            {
                case RCP_MSG_CMD:
                    break;
                case RCP_MSG_RSP:
                    m_bRcpReceivedPacket = true;
                    ParseMsgRspType(bb);
                    if (RxCaptureMsg != null) RxCaptureMsg.BeginInvoke(this, new byteEventArg(bb.GetByteArray()), null, null);                    
                    break;
                case RCP_MSG_NOTI:
                case RCP_MSG_TEST:
                case RCP_MSG_CAP:
                    ParseMsgRspType(bb);                    
                    if (RxCaptureMsg != null) RxCaptureMsg.BeginInvoke(this, new byteEventArg(bb.GetByteArray()), null, null);
                    break;
            }   

            return true; // packet payload 에 0x7E가 포함되어 있을 경우 false를 retrun 한다. TBD
        }

        System.Threading.Mutex m = new System.Threading.Mutex();

        public void RcpLog(ByteBuilder bb)
        {
            byte[] buf = bb.GetByteArray();
            int len = buf[4];

            if (buf[1] == 'W' || buf[1] == 'X') // cmd_type
            {
                StringBuilder byteToString = new StringBuilder();

                for (int i = 0; i < bb.Length; i++)
                {
                    byteToString.Append(string.Format("{0:X2} ", buf[i]));
                }

                string strData = "RCP CMD> " + byteToString.ToString() + " \r\n";

                m.WaitOne();        
                if (mRcpLogEventReceived != null) mRcpLogEventReceived.BeginInvoke(this, new StringEventArg(strData), null, null);                                                
                m.ReleaseMutex();
                
                return;
            }

            StringBuilder sb = new StringBuilder();
            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            for (int i = 5; i < len + 5; i++)
            {
                if (buf[i] >= 0x20 && buf[i] < 0x7F)
                {
                    char[] chars = new char[1];
                    d.GetChars(buf, i, 1, chars, 0);
                    sb.Append(new System.String(chars));
                }
                else
                {
                    sb.Append(" ");
                }                
            }

            if (buf[2] == 0xDF) // cmd_type
            {
                string strData = "DEBUG> " + sb.ToString() + " \r\n";
                m.WaitOne();        
                if (mRcpLogEventReceived != null) mRcpLogEventReceived.BeginInvoke(this, new StringEventArg(strData), null, null);
                if (SioEventMsg != null) SioEventMsg(this, new StringEventArg(strData));
                m.ReleaseMutex();
            }            
            else 
            {
                StringBuilder byteToString = new StringBuilder();

                for (int i = 0; i < len + 8; i++)
                {                    
                    byteToString.Append(string.Format("{0:X2} ", buf[i]));                    
                }
                string strData;

                if (buf[1] != 00)
                    strData = "RCP RSP> ";
                else
                    strData = "RCP CMD> ";

                strData +=  byteToString.ToString() + "(" + sb.ToString() + ")" + " \r\n";

                m.WaitOne(); 
                if (mRcpLogEventReceived != null) mRcpLogEventReceived.BeginInvoke(this, new StringEventArg(strData), null, null);
                m.ReleaseMutex();
            }            
        }

        public void ParseMsgRspType(ByteBuilder bb)
        {
            byte[] buf = bb.GetByteArray();

            int cmd_type;
            int len;
                       
            cmd_type = (int)buf[2];
            len = (buf[3] << 8) + buf[4];
                                   
            if (SioEventMsg != null)
            {
                switch (cmd_type)
                {
                    case (int)RcpConst.RCP_CMD_SET_RF_REG:                        
                        if (SioEventMsg != null) SioEventMsg(this, new StringEventArg("SIO>RF Register SET OK...\r\n"));
                        break;
                    case (int)RcpConst.RCP_CMD_SET_MODEM_REG:                        
                        if (SioEventMsg != null) SioEventMsg(this, new StringEventArg("SIO>MODEM Register SET OK...\r\n"));
                        break;
                    default:
                        break;
                }
                return;
            }
            else
            {
                switch (cmd_type)
                {
                    case (int)RcpConst.RCP_CMD_SET_DOWNLOAD:
                        if (RxRspParsed != null) RxRspParsed(this, new byteEventArg(buf));                        
                        return;
                    case (int)RcpConst.RCP_CMD_SET_DUMP:
                        if (RxRspParsed != null) RxRspParsed(this, new byteEventArg(buf));
                        //if (m_bCmdError)
                        //     if (RxRspParsed != null) RxMsgParsed.BeginInvoke(this, new StringEventArg(sb.ToString()), null, null);

                        for (int i = 0; i < len; i++)
                            dump_data[i] = buf[i + 5];                        
                        return;
                }
            }                                                
            if (RxRspParsed != null) RxRspParsed(this, new byteEventArg(buf));
        }
    }
}
