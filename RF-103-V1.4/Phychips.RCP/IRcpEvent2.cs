using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phychips.Rcp
{
    public interface IRcpEvent2
    {
        void onPlugged(bool plug, string port);

        void onSuccessReceived(byte[] data, int cmdCode);

        void onFailureReceived(byte[] errCode);

        void onReaderInfoReceived(byte[] info);

        void onRegionReceived(int region);

        void onSelectParamReceived(int target, int action, int memBank, long pointer, int length, int truncate, byte[] mask);

        void onQueryParamReceived(int dr, int m, int trext, int sel, int session, int target, int q);

        void onChannelReceived(int ch, int chOffset);

        void onFhLbtParamReceived(int rTime, int iTime, int csTime, int rfLevel, int fh, int lbt, int cw);

        void onTxPowerLevelReceived(int currPower, int minPower, int maxPower);

        void onTagMemoryReceived(int wordCnt, byte[] data);

        void onTagMemoryLongReceived(int rspType, int startAddr, int wordCnt, byte[] data);

        void onSessionReceived(int session);

        void onFHTableReceived(int tblSize, byte[] table);

        void onModulationReceived(int blf, int rxMod, int dr);

        void onAntiColModeReceived(int mode, int qStart, int qMax, int qMin);

        void onTagReceived(byte[] pcEpc);

        void onTagWithRssiReceived(byte[] pcEpc, int rssi);

        void onTagWithTidReceived(byte[] pcEpc, byte[] tid);

        void onFHModeReceived(int mode);

        void onFHModeRefLevelReceived(int refLevel);


// >> 20180611, HYO
        void onNativeReceived(byte[] packet);
        // << 20180611, HYO

        // >> 20200121, KK
        void onGetReaderSerial(byte[] packet);
// << 20200121, KK
    
    
    }
}
