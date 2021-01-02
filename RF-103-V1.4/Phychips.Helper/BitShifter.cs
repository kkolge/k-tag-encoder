using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phychips.Helper
{
    public class BitShifter
    {
        private static byte bit_align(byte source_byte, int bit_length, int lsb_offset)
        {
            if (lsb_offset > 0)
            {
                return (byte)(((source_byte & ((1 << bit_length) - 1)) << lsb_offset) & 0xff);
            }
            else
            {
                return (byte)(((source_byte >> (0 - lsb_offset)) & ((1 << bit_length) - 1)) & 0xff);
            }
        }

        public static byte[] bit_array_shift_l(byte[] src, int BitLength, int BitOffset)
        {
            if (BitOffset == 0 || BitOffset > 7)
                return src;

            byte[] dst = new byte[((BitLength - BitOffset) + 7) >> 3];

            //for(int i = 0; i < (((BitLength + BitOffset) + 7) >> 3) ; i++)
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = bit_align(src[i], 8 - BitOffset, BitOffset);

                if (((BitLength >> 3) - i) > 0)
                {
                    dst[i] |= bit_align(src[i + 1], BitOffset, -8 + BitOffset);
                }

                //System.out.printf("[%02X]  [%02X]\r\n", src[i], dst[i]);
            }

            return dst;
        }
    }
}
