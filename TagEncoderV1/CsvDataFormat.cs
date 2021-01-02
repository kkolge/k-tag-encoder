using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagEncoderV1
{
    class CsvDataFormat
    {
        //Serial number for the row that is read from the file
        public string srNo { get; set; }

        //Access Password. Converting in byte array and storing 
        private byte[] iAccessPwd;
        public void setAccessPwd (string ap)
        {
            iAccessPwd = new byte[8];
            iAccessPwd = Utils.StringToByteArray(ap);
            //byte[] hexArr = Utils.StringToByteArray(ap);
            //hexArr =  hexArr.Reverse<byte>().ToArray();
            //iAccessPwd = BitConverter.ToInt64(hexArr, 0);
        }
        public byte[] getAccessPwd()
        {
            return iAccessPwd;
        }

        //Kill Password
        private byte[] iKillPwd;
        public void setKillPwd(string kp)
        {
            iKillPwd = new byte[8];
            iKillPwd = Utils.StringToByteArray(kp);
            //iKillPwd = Convert.ToInt64(kp, 16);
        }
        public byte[] getKillPwd()
        {
            return iKillPwd;
        }

        //EPC
        private byte[] iEpc;
        public void setEpcMem(string epc)
        {
            iEpc = new byte[8];
            iEpc = Utils.StringToByteArray(epc);
        } 
        public byte[] getEpcMem()
        {
            return iEpc;
        }

        //TID
        public byte[] tidMem { get; set; }

        //USER Memory
        private byte[] iUserMem;
        public void setUserMem(string userMem)
        {
            if (userMem.Length != 0)
            {
                iUserMem = new byte[8];
                iUserMem = Utils.StringToByteArray(userMem);
            }
            else
            {
                iUserMem = new byte[1];
                iUserMem[0] = 0x00;
            }
        }
        public byte[] getUserMem()
        {
            return iUserMem;
        }

        //Memory Locking
        private bool iLockMem = false;
        public void setLockMem(string lockMem)
        {
            if(lockMem.ToUpper() == "YES")
            {
                iLockMem = true;
            }
        }
        public bool getLockMem()
        {
            return iLockMem;
        }

        //Overriding To String method to get all data from the the object
        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(srNo);
            sb.Append(", ");
            sb.Append(getAccessPwd());
            sb.Append(", ");
            sb.Append(getKillPwd());
            sb.Append(", ");
            sb.Append(Utils.ByteArrayToString(getEpcMem()));
            sb.Append(", ");
            sb.Append(Utils.ByteArrayToString(tidMem));
            sb.Append(", ");
            sb.Append(Utils.ByteArrayToString(getUserMem()));
            sb.Append(", ");
            sb.Append(getLockMem() == true ? "YES" : "NO");

            return sb.ToString();
        }
    }
}
