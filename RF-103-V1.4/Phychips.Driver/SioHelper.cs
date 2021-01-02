using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phychips.Driver
{
    public class SioHelper
    {
        static public String compStringArray(String[] strA, String[] strB)
        {
            if (strA == null || strB == null || (strA.Length == strB.Length))
                return "";

            if (strA.Length > strB.Length)
            {
                for (int i = 0; i < strA.Length - 1; i++)
                {
                    if (i < strB.Length && strA[i] != strB[i])
                        return strA[i];
                }
                return strA[strA.Length - 1];
            }
            else
            {
                for (int i = 0; i < strB.Length - 1; i++)
                {
                    if (i < strA.Length && strA[i] != strB[i])
                        return strB[i];
                }
                return strB[strB.Length - 1];
            }
        }
    }
}
