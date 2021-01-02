using Phychips.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Phychips.Driver
{
    public class Logger
    {
        // Single Tone
        private static Logger m_oInstance;
        public static Logger Instance
        {
            get
            {
                if (m_oInstance == null) m_oInstance = new Logger();
                return m_oInstance;
            }
        }

        private Logger()
        {
        }

        private StreamWriter sw;

        public void LogOpen(string file_name)
        {
            if (sw != null)
            {
                sw.Flush();
                sw.Close();
                sw = null;
            }

            try
            {
                sw = new StreamWriter(file_name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LogWriteLine(string msg)
        {
            if (sw != null)
            {
                sw.WriteLine(msg); sw.Flush();
            }
        }
        
        public void LogWriteLine(byte[] a)
        {
            if (sw != null)
            {
                sw.WriteLine((new ByteBuilder(a)).ToString()); sw.Flush();
            }
        }

        public void LogWriteLine(string msg, byte[] a)
        {
            if (sw != null)
            {
                sw.WriteLine(msg + (new ByteBuilder(a)).ToString()); sw.Flush();
            }
        }
        
        public void LogClose()
        {
            if (sw == null)
                return;

            sw.Flush();
            sw.Close();
            sw = null;
        }
    }
}
