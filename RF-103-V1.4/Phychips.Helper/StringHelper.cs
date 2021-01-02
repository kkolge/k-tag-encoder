//! Copyright (C) 2007 Phychips
//! 
//! StringHelper.cs
//!
//! Description
//! 	StringHelper
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release
using System;
using System.Collections.Generic;
using System.Text;

namespace Phychips.Helper
{
    public enum PadType
    {
        PAD_LEFT,
        PAD_RIGHT
    };

    public class StringHelper
    {

        static public string ArgByteToStringByte(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte abyte in byteArray)
            {
                sb.Append(abyte.ToString("X02") + " ");
            }

            return sb.ToString();
        }

        static public string ArgByteToStringByte2(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte abyte in byteArray)
            {
                sb.Append(abyte.ToString("X02"));
            }

            return sb.ToString();
        }
        
        static public byte[] ArgStringHexToByte(string val)
        {
            ByteBuilder bb = new ByteBuilder();
            char[] delimStr = { ' ' };

            val = val.Trim();
            val = val.Replace("0x", "");
            val = val.Replace(" ", "");

            try
            {
                for (int i = 0; i < val.Length / 2; i++)
                {
                    bb.Append(Convert.ToByte(val.Substring(i * 2, 2), 16));
                }
            }
            catch
            {
            }

            return bb.GetByteArray();
        }



        static public byte[] ArgStringBinaryToByte(string val, PadType pad)
        {
            ByteBuilder bb = new ByteBuilder();
            char[] delimStr = { ' ' };

            val = val.Trim();
            val = val.Replace(" ", "");
            if (pad == PadType.PAD_LEFT)
            {
                val = val.PadLeft(((val.Length + 7) >> 3) * 8, '0');
            }
            else
            {
                val = val.PadRight(((val.Length + 7) >> 3) * 8, '0');
            }

            try
            {
                for (int i = 0; i < (val.Length + 7) >> 3; i++)
                {
                    if (i == 0) bb.Append(Convert.ToByte(val.Substring(i * 8, 8 - val.Length % 8), 2));
                    else bb.Append(Convert.ToByte(val.Substring(i * 8 - val.Length % 8, 8), 2));
                }
            }
            catch
            {

            }

            return bb.GetByteArray();
        }


        static public string ArgByteToStringBinary(byte[] val, int BitLength)
        {
            int len;
            StringBuilder sb = new StringBuilder();

            if (BitLength > (val.Length + 7 / 8)) len = (val.Length + 7 / 8);
            else len = BitLength;

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (((val[i] >> (7 - j)) & 0x01) == 0x01) sb.Append("1");
                    else sb.Append("0");
                }
                sb.Append(" ");
            }

            return sb.ToString();
        }



    }
}
