using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Phychips.Driver
{
    [SerializableAttribute]
    public class SioConfig
    {
        public String Port;
        public int Baud;
        public int DataBits;
        public Parity Parity;
        public StopBits StopBits;
        public SioConfig()
        {
            Port = "COM6";
            Baud = 115200;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
        }        
    }
}
