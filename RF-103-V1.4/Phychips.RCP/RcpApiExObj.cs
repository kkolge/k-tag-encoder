using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phychips.Rcp;

namespace Phychips.Rcp
{
    public class RcpApiExObj : RcpApiEx
    {
        private readonly static RcpApiExObj m_oInstance = new RcpApiExObj();
        private RcpApiExObj()
        {            
        
        }
        public static RcpApiExObj Instance
        {
            get
            {                
                return m_oInstance;
            }
        }
    }
}
