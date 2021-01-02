//! Copyright (C) 2007 Phychips
//! 
//! StringEventArg.cs
//!
//! Description
//! 	StringEventArg
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.Text;

namespace Phychips.Helper
{
    public class StringEventArg : EventArgs
    {
        private readonly string m_oData;

        public StringEventArg(string str)
        {
            m_oData = str;
        }
          
        public string Data
        {
            get { return m_oData; }
        }

    }

    public class byteEventArg : EventArgs
    {
        private readonly byte[] m_oData;

        public byteEventArg(byte[] Rsp)
        {
            m_oData = Rsp;
        }

        public byte[] Data
        {
            get { return m_oData; }
        }

    }
}
