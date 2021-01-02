using System;
using System.Collections.Generic;
using System.Text;
using Phychips.Rcp;
using Phychips.Helper;
using System.Threading;


namespace Phychips.Rcp
{    
    public class RcpApi2 : RcpApiBase
    {
        private readonly static RcpApi2 m_oInstance = new RcpApi2();
        private RcpApi2()
        {            
        
        }
        public static RcpApi2 Instance
        {
            get
            {                
                return m_oInstance;
            }
        }
    }
}
