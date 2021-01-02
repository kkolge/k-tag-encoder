using Phychips.Helper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Phychips.Driver
{
    public interface ISio : IDisposable
    {        
        event EventHandler<byteEventArg> DataReceived;
        bool IsOpenable(SioConfig config);
        bool Open(SioConfig config);
        SioConfig GetPortConfig();
        void SioDataReceived(object sender, SerialDataReceivedEventArgs e);
        bool Send(byte[] byData);
        void Close();
        void Flush();
        bool IsOpened();        
    }
}
