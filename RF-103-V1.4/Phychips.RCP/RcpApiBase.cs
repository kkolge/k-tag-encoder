using System;
using System.Collections.Generic;
using System.Text;
using Phychips.Rcp;
using Phychips.Helper;
using System.Threading;


namespace Phychips.Rcp
{
    public class RcpApiBase
    {
        private IRcpEvent2 mRcpEvent2;
        public event EventHandler<byteEventArg> RcpPacketReceived;

        protected RcpApiBase()
        {
            RcpProtocol.Instance.RxRspParsed += RxRspEventReceived;
            RcpProtocol.Instance.EventPortClosed += DeviceRemoved;
            RcpProtocol.Instance.EventPortOpened += DeviceAdded;
        }

        protected virtual void ParseRspEx(byte[] Data)
        {
            mRcpEvent2.onSuccessReceived(Data, Data[2]);
        }

        private void RxRspEventReceived(object sender, byteEventArg e)
        {
            ParseRsp(e.Data);

            if (RcpPacketReceived != null)
                RcpPacketReceived(sender, e);
        }

        public void DeviceAdded(object sender, StringEventArg e)
        {
            if (mRcpEvent2 != null)
                mRcpEvent2.onPlugged(true, e.Data);
        }

        public void DeviceRemoved(object sender, StringEventArg e)
        {
            if (mRcpEvent2 != null)
                mRcpEvent2.onPlugged(false, e.Data);
        }

        public void setOnRcpEventListener(IRcpEvent2 e)
        {
            mRcpEvent2 = e;
        }

        public bool isOpenable()
        {
            return RcpProtocol.Instance.IsOpenable();
        }

        public bool open()
        {
            return RcpProtocol.Instance.Open();
        }

        public bool isOpened()
        {
            return RcpProtocol.Instance.IsOpened();
        }

        public void close()
        {
            RcpProtocol.Instance.Close();
        }


// >> 20191203, HYO
        public void showFormConfig()
        {
            RcpProtocol.Instance.ShowFormConfig();
        }
// << 20191203, HYO


        private void ParseRsp(byte[] Data)
        {
            byte[] buf = Data;
            int len = buf[4];
            if (len > buf.Length - 5) len = buf.Length;
            char[] chars = new char[len];
            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            d.GetChars(buf, 5, len, chars, 0);

            if (Data.Length >= 7)
            {
                byte cmd = buf[2];

                if (mRcpEvent2 != null)
                {
                    byte[] dest = new byte[len];
                    Array.Copy(buf, 5, dest, 0, dest.Length);


// >> 20180611, HYO
                    mRcpEvent2.onNativeReceived(dest);
// << 20180611, HYO


                    switch (cmd)
                    {
                        case RcpConst.RCP_CMD_GET_RD_INF:
                            mRcpEvent2.onReaderInfoReceived(dest);
                            break;
                        case RcpConst.RCP_CMD_GET_REGION:
                            mRcpEvent2.onRegionReceived(dest[0]);
                            break;
                        case RcpConst.RCP_CMD_GET_C_SEL_PARM:
                            {
                                int target = (dest[0] >> 5) & 0x07;
                                int action = (dest[0] >> 2) & 0x07;
                                int memBank = dest[0] & 0x03;
                                long pointer = dest[1];
                                pointer = (pointer << 8) + dest[2];
                                pointer = (pointer << 8) + dest[3];
                                pointer = (pointer << 8) + dest[4];
                                int length = dest[5];
                                int truncate = dest[6] >> 7;
                                byte[] mask = new byte[dest.Length - 6];
                                Array.Copy(dest, 6, mask, 0, mask.Length);
                            
                                mRcpEvent2.onSelectParamReceived(target, action, memBank, pointer, length, truncate, mask);
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_C_QRY_PARM:
                            {
                                int dr = (dest[0] >> 7) & 0x01;
                                int m = (dest[0] >> 5) & 0x03;
                                int trext = (dest[0] >> 4) & 0x01;
                                int sel = (dest[0] >> 2) & 0x03;
                                int session = dest[0] & 0x03;
                                int target = (dest[1] >> 7) & 0x01;
                                int q = (dest[1] >> 3) & 0x0f;
                                
                                mRcpEvent2.onQueryParamReceived(dr, m, trext, sel, session, target, q);
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_CH:
                            mRcpEvent2.onChannelReceived(dest[0], dest[1]);
                            break;
                        case RcpConst.RCP_CMD_GET_FH_LBT:
                            {
                                int rTime = dest[0];
                                rTime = (rTime << 8) + dest[1];
                                int iTime = dest[2];
                                iTime = (iTime << 8) + dest[3];
                                int csTime = dest[4];
                                csTime = (csTime << 8) + dest[5];
                                int rfLevel = dest[6];
                                rfLevel = (rfLevel << 8) + dest[7];
                                int fh = dest[8];
                                int lbt = dest[9];
                                int cw = dest[10];

                                mRcpEvent2.onFhLbtParamReceived(rTime, iTime, csTime, rfLevel, fh, lbt, cw);
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_TX_PWR:
                            {
                                int currPower = dest[0];
                                currPower = (currPower << 8) + dest[1];
                                int minPower = dest[2];
                                minPower = (minPower << 8) + dest[3];
                                int maxPower = dest[4];
                                maxPower = (maxPower << 8) + dest[5];


// >> 20180612, HYO
                                if ((minPower == 130) && (maxPower == 200))
                                {
                                    maxPower = 270;
                                }

                                if ((minPower == 200) && (maxPower == 270))
                                {
                                    minPower = 130;
                                }
// << 20180612, HYO


                                mRcpEvent2.onTxPowerLevelReceived(currPower, minPower, maxPower);
                            }
                            break;
                        case RcpConst.RCP_CMD_READ_C_DT:
                            mRcpEvent2.onTagMemoryReceived(dest.Length>>1, dest);
                            break;
                        case RcpConst.RCP_CMD_READ_C_DT_EX:
                            {
                                if (dest.Length > 1)
                                {
                                    int rspType = 1;
                                    int startAddr = dest[0];
                                    startAddr = (startAddr << 8) + dest[1];
                                    int wordCnt = dest[2];
                                    byte[] data = new byte[dest.Length - 3];
                                    Array.Copy(dest, 3, data, 0, data.Length);

                                    mRcpEvent2.onTagMemoryLongReceived(rspType, startAddr, wordCnt, data);
                                }
                                else
                                {
                                    int rspType = 0;
                                    int startAddr = 0;
                                    int wordCnt = 0;
                                    byte[] data = dest;

                                    mRcpEvent2.onTagMemoryLongReceived(rspType, startAddr, wordCnt, data);
                                }
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_SESSION:
                            mRcpEvent2.onSessionReceived(dest[0]);
                            break;
                        case RcpConst.RCP_CMD_GET_HOPPING_TBL:
                            {
                                int tblSize = dest[0];
                                byte[] table = new byte[dest.Length - 1];
                                    Array.Copy(dest, 1, table, 0, table.Length);
 
                                mRcpEvent2.onFHTableReceived(tblSize, table);
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_MODULATION:
                            {
                                int blf = dest[0];
                                blf = (blf << 8) + dest[1];
                                int rxMod = dest[2];
                                int dr = dest[3];

                                mRcpEvent2.onModulationReceived(blf, rxMod, dr);
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_ANTICOL_MODE:
                            mRcpEvent2.onAntiColModeReceived(dest[0], dest[1], dest[2], dest[3]);
                            break;
                        case RcpConst.RCP_CMD_READ_C_UII:
                            mRcpEvent2.onTagReceived(dest);
                            break;
                        case RcpConst.RCP_CMD_READ_C_UII_RSSI:
                            {
                                int dataLen = dest.Length;
                                int rssi_i = dest[dataLen - 4] & 0xff;
                                int rssi_q = dest[dataLen - 3] & 0xff;
                                int gain_i = dest[dataLen - 2] & 0xff;
                                int gain_q = dest[dataLen - 1] & 0xff;

                                double rfin_i = (20 * Math.Log10(rssi_i) - gain_i - 33 - 30);
                                double rfin_q = (20 * Math.Log10(rssi_q) - gain_q - 33 - 30);

                                rfin_i = Math.Pow(10, (rfin_i / 20));
                                rfin_q = Math.Pow(10, (rfin_q / 20));

                                int rssi = (int)(20 * Math.Log10(Math.Sqrt(
                                    Math.Pow(rfin_i, 2) + Math.Pow(rfin_q, 2))));

                                byte[] newDest = new byte[dest.Length - 4];
                                Array.Copy(dest, 0, newDest, 0, dest.Length - 4);

                                mRcpEvent2.onTagWithRssiReceived(newDest, rssi);
                            }
                            break;
                        case RcpConst.RCP_CMD_READ_C_UII_TID:
                            {
                                if (dest.Length == 1) break;

                                int pcEpcLength = getPcEpcLength(dest[0]);

                                byte[] pcEpc = new byte[pcEpcLength];
                                byte[] tid = new byte[dest.Length - pcEpcLength];

                                Array.Copy(dest, 0, pcEpc, 0, pcEpc.Length);
                                Array.Copy(dest, pcEpc.Length, tid, 0, tid.Length);

                                mRcpEvent2.onTagWithTidReceived(pcEpc, tid);
                            }
                            break;
                        case RcpConst.RCP_CMD_GET_FH_MODE:
                            mRcpEvent2.onFHModeReceived(dest[0]);
                            break;
                        case RcpConst.RCP_CMD_GET_FH_MODE_REF_LEVEL:
                            mRcpEvent2.onFHModeRefLevelReceived(dest[0]);
                            break;
                        case RcpConst.RCP_CMD_SET_REGION:
                        case RcpConst.RCP_CMD_SET_C_SEL_PARM:
                        case RcpConst.RCP_CMD_SET_C_QRY_PARM:
                        case RcpConst.RCP_CMD_SET_CH:
                        case RcpConst.RCP_CMD_SET_FH_LBT:
                        case RcpConst.RCP_CMD_SET_TX_PWR:
                        case RcpConst.RCP_CMD_SET_CW:
                        case RcpConst.RCP_CMD_SET_HOPPING_TBL:
                        case RcpConst.RCP_CMD_SET_MODULATION:
                        case RcpConst.RCP_CMD_SET_ANTICOL_MODE:
                        case RcpConst.RCP_CMD_STOP_AUTO_READ_EX:
                        case RcpConst.RCP_CMD_WRITE_C_DT:


// >> 20200106, HYO                        
                        case RcpConst.RCP_CMD_WRITE_C_DT_MASK_TID:
                        case RcpConst.RCP_CMD_BLOCKWRITE_C_DT_MASK_TID:
                        case RcpConst.RCP_CMD_LOCK_C_MASK_TID:
// << 20200106, HYO                        
                        
                        
                        case RcpConst.RCP_CMD_KILL_RECOM_C:
                        case RcpConst.RCP_CMD_LOCK_C:
                        case RcpConst.RCP_UPDATE_REGISTRY:
                        case RcpConst.RCP_CMD_SET_OPT_FH_TABLE:
                        case RcpConst.RCP_CMD_SET_FH_MODE:
                        case RcpConst.RCP_CMD_SET_FH_MODE_REF_LEVEL:


// >> 20180611, HYO
                        case RcpConst.RCP_CMD_SET_GAIN_MODE:
// << 20180611, HYO
                        
                        
                            mRcpEvent2.onSuccessReceived(dest, cmd);
                            break;
// >> 20200121, KK
                        case RcpConst.RCP_GET_REGISTRY_ITEM:
                            mRcpEvent2.onGetReaderSerial(dest);
                            break;
// << 20200121, KK
                        case RcpConst.RCP_FAIL:
                            mRcpEvent2.onFailureReceived(dest);
                            break;
                        default:
                            ParseRspEx(Data);
                            break;
                    }
                }
            }
        }

        private int getPcEpcLength(int pcMsb)
        {
            int length = ((pcMsb >> 2) & 0xFE) + 2;
            return length;
        }

        public bool getReaderInfo(int type)
        {
            byte[] param = new byte[1];
            param[0] = (byte)type;

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_RD_INF, param));
        }

        public bool getRegion()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_REGION, null));
        }

        public bool setRegion(int region)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_REGION, new byte[] { (byte)region }));
        }

        public bool getSelectParam()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_C_SEL_PARM, null));
        }

        public bool setSelectParam(int target, int action, int memBank, long pointer, int length, int truncate, byte[] mask)
        {
            ByteBuilder bb = new ByteBuilder();

            byte[] param = new byte[7];

            param[0] = (byte)((target << 5) + (action << 2) + (memBank));
            param[1] = 0; // pointer
            param[2] = 0; // pointer
            param[3] = (byte)((pointer & 0xFF00) >> 8);
            param[4] = (byte)((pointer & 0x00FF));
            param[5] = (byte)length;
            param[6] = (byte)truncate;

            if (mask != null && mask.Length > 0)
            {
                bb.Append(param);
                bb.Append(mask);
            }
            else
            {
                return false;
            }

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_C_SEL_PARM, param));
        }

        public bool getQueryParam()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_C_QRY_PARM, null));
        }

        public bool setQueryParam(int dr, int m, int trext, int sel, int session, int target, int q)
        {
            byte[] param = new byte[2];

            param[0] = (byte) ((dr << 7) + (m << 5) + (trext << 4) + (sel << 2) + (session));
            param[1] = (byte) ((target << 7) + (q << 3));

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_C_QRY_PARM, param));
        }

        public bool getChannel()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_CH, null));
        }

        public bool setChannel(int ch, int chOffset)
        {
            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)ch);
            bb.Append((byte)chOffset);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_CH, bb.GetByteArray()));
        }

        public bool getFhLbtParam()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_FH_LBT, null));
        }

        public bool setFhLbtParam(int rTime, int iTime, int csTime, int rfLevel, int fh, int lbt, int cw)
        {
            ByteBuilder bb = new ByteBuilder();

            bb.Append((UInt16)rTime);
            bb.Append((UInt16)iTime);
            bb.Append((UInt16)csTime);
            bb.Append((UInt16)rfLevel);
            bb.Append((byte)fh);
            bb.Append((byte)lbt);
            bb.Append((byte)cw);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_FH_LBT, bb.GetByteArray()));
        }

        public bool getOutputPowerLevel()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_TX_PWR, null));
        }


// >> 20180612, HYO
        public bool setOutputPowerLevel(int power)
        {
            byte[] arrayPower = new byte[2];

            arrayPower[0] = (byte)((power & 0xFF00) >> 8);
            arrayPower[1] = (byte)(power & 0xFF);

            if(power >= 200)
                RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_GAIN_MODE, new byte[] { 0x00 }));    // High Gain Mode
            else
                RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_GAIN_MODE, new byte[] { 0x01 }));    // Low Gain Mode

            System.Threading.Thread.Sleep(100);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_TX_PWR, arrayPower));
        }

        public bool setGainMode(int mode)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_GAIN_MODE, new byte[] { (byte)mode }));
        }
// << 20180612, HYO


        public bool setRfCw(int control)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_CW, new byte[] { (byte)control }));
        }
        public bool readFromTagMemory(long accessPw, byte[] epc, int memBank, int startAddr, int dataLen)
        {
            if (epc == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((epc.Length >> 8) & 0xff));
            bb.Append((byte)epc.Length);

            for (int i = 0; i < epc.Length; i++)
            {
                bb.Append(epc[i]);
            }

            bb.Append((byte)memBank); // mem_bank = RFU            
            bb.Append((UInt16)startAddr); // Start Addresss            
            bb.Append((UInt16)dataLen); // Length

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_READ_C_DT, bb.GetByteArray()));
        }

        public bool readFromTagMemoryLong(long accessPw, byte[] epc, int memBank, int startAddr, int dataLen)
        {
            if (epc == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((epc.Length >> 8) & 0xff));
            bb.Append((byte)epc.Length);

            for (int i = 0; i < epc.Length; i++)
            {
                bb.Append(epc[i]);
            }

            bb.Append((byte)memBank); // mem_bank = RFU            
            bb.Append((UInt16)startAddr); // Start Addresss            
            bb.Append((UInt16)dataLen); // Length

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_READ_C_DT_EX, bb.GetByteArray()));
        }

        public bool getSession()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_SESSION, null));
        }

        public bool setSession(int session)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_SESSION, new byte[] { (byte)session }));
        }

        public bool getFHTable()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_HOPPING_TBL, null));
        }

        public bool setFHTable(byte[] table)
        {
            ByteBuilder bb = new ByteBuilder();
            bb.Append(table);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_HOPPING_TBL, bb.GetByteArray()));
        }

        public bool getModulation()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_MODULATION, null));
        }

        public bool setModulation(int blf, int rxMod, int dr)
        {
            ByteBuilder bb = new ByteBuilder();

            bb.Append((UInt16)blf);
            bb.Append((byte)rxMod);
            bb.Append((byte)dr);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_MODULATION, bb.GetByteArray()));
        }

        public bool getAntiColMode()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_ANTICOL_MODE, null));
        }

        public bool setAntiColMode(int mode)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_ANTICOL_MODE, new byte[] { (byte)mode }));
        }

        public bool startReadTags(int maxTags, int maxTime, int reCycle)
        {
            ByteBuilder bb = new ByteBuilder();

            bb.Append(0x02);
            bb.Append((byte)maxTags);
            bb.Append((byte)maxTime);
            bb.Append((UInt16)reCycle);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_STRT_AUTO_READ_EX, bb.GetByteArray()));
        }

        public bool stopReadTags()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_STOP_AUTO_READ, null));
        }

        public bool startReadTagsWithRssi(int maxTags, int maxTime, int reCycle)
        {
            ByteBuilder bb = new ByteBuilder();

            bb.Append(0x02);
            bb.Append((byte)maxTags);
            bb.Append((byte)maxTime);
            bb.Append((UInt16)reCycle);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_STRT_AUTO_READ_RSSI, bb.GetByteArray()));
        }

 // Ketan - Get Serial number - 21Jan2020

            public bool getReaderSerial()
        {
            ByteBuilder bb = new ByteBuilder();
            bb.Append(0x00);
            bb.Append(0x0B);
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_GET_REGISTRY_ITEM, bb.GetByteArray()));

        }
 //Ketan Add end


// >> 20180611, HYO
        public bool startReadTagsWithTid(int maxTags, int maxTime, int reCycle)
        {
            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)maxTags);
            bb.Append((byte)maxTime);
            bb.Append((UInt16)reCycle);

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_READ_C_UII_TID, bb.GetByteArray()));
        }
// << 20180611, HYO


        public bool writeToTagMemory(long accessPw, byte[] epc, int memBank, int startAddr, byte[] data)
        {
            if (epc == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((epc.Length >> 8) & 0xff));
            bb.Append((byte)epc.Length);

            for (int i = 0; i < epc.Length; i++)
            {
                bb.Append(epc[i]);
            }

            bb.Append((byte)memBank); // mem_bank = RFU            
            bb.Append((UInt16)startAddr); // Start Addresss            
            bb.Append((UInt16)(data.Length / 2)); // Start Addresss            
            bb.Append(data); // Length

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_WRITE_C_DT, bb.GetByteArray()));
        }


// >> 20200106, HYO
        public bool writeToTagMemory_MaskTID(long accessPw, byte[] tid, int memBank, int startAddr, byte[] data)
        {
            if (tid == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((tid.Length >> 8) & 0xff));
            bb.Append((byte)(tid.Length & 0xff));

            for (int i = 0; i < tid.Length; i++)
            {
                bb.Append(tid[i]);
            }

            bb.Append((byte)memBank); // mem_bank = RFU            
            bb.Append((UInt16)startAddr); // Start Addresss            
            bb.Append((UInt16)(data.Length / 2)); // Start Addresss            
            bb.Append(data); // Length

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_WRITE_C_DT_MASK_TID, bb.GetByteArray()));
        }

        public bool blockwriteToTagMemory_MaskTID(long accessPw, byte[] tid, int memBank, int startAddr, byte[] data)
        {
            if (tid == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((tid.Length >> 8) & 0xff));
            bb.Append((byte)(tid.Length & 0xff));

            for (int i = 0; i < tid.Length; i++)
            {
                bb.Append(tid[i]);
            }

            bb.Append((byte)memBank); // mem_bank = RFU            
            bb.Append((UInt16)startAddr); // Start Addresss            
            bb.Append((UInt16)(data.Length / 2)); // Start Addresss            
            bb.Append(data); // Length

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_BLOCKWRITE_C_DT_MASK_TID, bb.GetByteArray()));
        }
// << 20200106, HYO


        public bool killTag(long killPw, byte[] epc, int recom)
        {
            if (epc == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((killPw >> 24) & 0xff));
            bb.Append((byte)((killPw >> 16) & 0xff));
            bb.Append((byte)((killPw >> 8) & 0xff));
            bb.Append((byte)((killPw >> 0) & 0xff));

            bb.Append((byte)((epc.Length >> 8) & 0xff));
            bb.Append((byte)epc.Length);

            for (int i = 0; i < epc.Length; i++)
            {
                bb.Append(epc[i]);
            }

            bb.Append((byte)recom);     // RFU

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_KILL_RECOM_C, bb.GetByteArray()));
        }

        public bool lockTagMemory(long accessPw, byte[] epc, int lockData)
        {
            if (epc == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((epc.Length >> 8) & 0xff));
            bb.Append((byte)epc.Length);

            for (int i = 0; i < epc.Length; i++)
            {
                bb.Append(epc[i]);
            }
            
            bb.Append((byte)((lockData >> 16) & 0xff));
            bb.Append((byte)((lockData >> 8) & 0xff));
            bb.Append((byte)((lockData >> 0) & 0xff));

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_LOCK_C, bb.GetByteArray()));
        }

        
// >> 20200106, HYO
        public bool lockTagMemory_MaskTID(long accessPw, byte[] tid, int lockData)
        {
            if (tid == null) return false;

            ByteBuilder bb = new ByteBuilder();

            bb.Append((byte)((accessPw >> 24) & 0xff));
            bb.Append((byte)((accessPw >> 16) & 0xff));
            bb.Append((byte)((accessPw >> 8) & 0xff));
            bb.Append((byte)((accessPw >> 0) & 0xff));

            bb.Append((byte)((tid.Length >> 8) & 0xff));
            bb.Append((byte)tid.Length);

            for (int i = 0; i < tid.Length; i++)
            {
                bb.Append(tid[i]);
            }

            bb.Append((byte)((lockData >> 16) & 0xff));
            bb.Append((byte)((lockData >> 8) & 0xff));
            bb.Append((byte)((lockData >> 0) & 0xff));

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_LOCK_C_MASK_TID, bb.GetByteArray()));
        }
// << 20200106, HYO


        public bool updateRegistry(int action)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_UPDATE_REGISTRY, null));
        }

        public bool setOptimumFHTable()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_OPT_FH_TABLE, null));
        }

        public bool getFHMode()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_FH_MODE, null));
        }

        public bool setFHMode(int mode)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_FH_MODE, new byte[] { (byte)mode }));
        }

        public bool getFHModeRefLevel()
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_GET_FH_MODE_REF_LEVEL, null));
        }

        public bool setFHModeRefLevel(int level)
        {
            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(RcpProtocol.RCP_MSG_CMD, RcpConst.RCP_CMD_SET_FH_MODE_REF_LEVEL, new byte[] { (byte)level }));
        }


// >> 20180612, HYO        
        public bool nativeRcp(byte[] rcp)
        {
            if (rcp == null) return false;
            if (rcp.Length < 6) return false;    // Minimum Packet Length
            if (rcp[0] != RcpProtocol.PREAMBLE) return false;    // Preamble = 0xBB
            if (rcp[1] != RcpProtocol.RCP_MSG_CMD) return false;    // (Message Type)Command = 0x00
            if (rcp[rcp.Length - 1] != RcpProtocol.ENDMARK) return false;  // End Mark = 0x7E

            ByteBuilder bb = new ByteBuilder();

            for (int i = 5; i < rcp.Length - 1; i++)   // Payload
            {
                bb.Append(rcp[i]);
            }

            return RcpProtocol.Instance.SendBytePkt(RcpProtocol.Instance.BuildCmdPacketByte(rcp[1], rcp[2], bb.GetByteArray()));
        }
// << 20180612, HYO


    }
}
