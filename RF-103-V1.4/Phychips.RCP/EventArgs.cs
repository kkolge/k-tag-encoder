using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phychips.Rcp
{
    public class onSuccessReceivedEventArgs : EventArgs
    {
        public int[] data { get; set; }
        public int cmdCode { get; set; }        
    }

    public class onFailureReceivedEventArgs : EventArgs
    {
        public int[] errCode { get; set; }        
    }

    public class onReaderInfoReceivedEventArgs : EventArgs
    {
        public int[] info { get; set; }
    }

    public class onRegionReceivedEventArgs : EventArgs
    {
        public int region { get; set; }
    }

    public class onSelectParamReceivedEventArgs : EventArgs
    {
        public int target { get; set; }
        public int action { get; set; }
        public int memBank { get; set; }
        public long pointer { get; set; }
        public int length { get; set; }
        public int truncate { get; set; }
        public int[] mask { get; set; }
    }

    public class onQueryParamReceivedEventArgs : EventArgs
    {
        public int dr { get; set; }
        public int m { get; set; }
        public int trext { get; set; }
        public int sel { get; set; }
        public int session { get; set; }
        public int target { get; set; }
        public int q { get; set; }
    }

    public class onChannelReceivedEventArgs : EventArgs
    {
        public int ch { get; set; }
        public int chOffset { get; set; }
    }

    public class onFhLbtParamReceivedEventArgs : EventArgs
    {
        public int rTime { get; set; }
        public int iTime { get; set; }
        public int csTime { get; set; }
        public int rfLevel { get; set; }
        public int fh { get; set; }
        public int lbt { get; set; }
        public int cw { get; set; }
    }

    public class onTxPowerLevelReceivedEventArgs : EventArgs
    {
        public int currPower { get; set; }
        public int minPower { get; set; }
        public int maxPower { get; set; }
    }

    public class onTagMemoryReceivedEventArgs : EventArgs
    {
        public int wordCnt { get; set; }
        public int[] data { get; set; }
    }

    public class onTagMemoryLongReceivedEventArgs : EventArgs
    {
        public int rspType { get; set; }
        public int startAddr { get; set; }
        public int wordCnt { get; set; }
        public int[] data { get; set; }
    }

    public class onSessionReceivedEventArgs : EventArgs
    {
        public int session { get; set; }
    }

    public class onFHTableReceivedEventArgs : EventArgs
    {
        public int tblSize { get; set; }
        public int[] table { get; set; }
    }

    public class onModulationReceivedEventArgs : EventArgs
    {
        public int blf { get; set; }
        public int rxMod { get; set; }
        public int dr { get; set; }
    }

    public class onAntiColModeReceivedEventArgs : EventArgs
    {
        public int mode { get; set; }
        public int qStart { get; set; }
        public int qMax { get; set; }
        public int qMin { get; set; }
    }

    public class onTagReceivedEventArgs : EventArgs
    {
        public int[] pcEpc { get; set; }
    }

    public class onTagWithTidReceivedEventArgs : EventArgs
    {
        public int[] pcEpc { get; set; }
        public int[] tid { get; set; }
    }
        
    public class onTagWithRssiReceivedEventArgs : EventArgs
    {
        public int[] pcEpc { get; set; }
        public int rssi { get; set; }
    }

    public class onFHModeReceivedEventArgs : EventArgs
    {
        public int mode { get; set; }
    }

    public class onFHModeRefLevelReceivedEventArgs : EventArgs
    {
        public int refLevel { get; set; }
    }


// >> 20180611, HYO
    public class onNativeReceivedEventArgs : EventArgs
    {
        public int mode { get; set; }
    }
// << 20180611, HYO


}
