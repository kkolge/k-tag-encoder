//! Copyright (C) 2007 Phychips
//! 
//! Sio.cs
//!
//! Description
//! 	Serial Port Driver
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO.Ports;
using System.IO;
using Phychips.Helper;
using Microsoft.Win32;
using System.Threading;
using System.Management;

namespace Phychips.Driver
{
    public class Sio : SioBase
    {        
        // Single Tone
        readonly private static Sio m_oInstance = new Sio();
        public static Sio Instance
        {
            get
            {                
                return (Sio)m_oInstance;
            }
        }
        private Sio()
        {


// >> 20170519, HYO, for SPS
//            setType(SioType.SIO_HID);
            setType(SioType.SIO_BOTH);
// << 20170519, HYO


        }
    }
}
