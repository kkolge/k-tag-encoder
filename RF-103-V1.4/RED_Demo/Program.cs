//!
//! Description
//! 	RED DEMO
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 0.1	2015/07/30	Yeongo Hwang		Initial Release

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Phychips.Red
{
    static class Program
    {        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "MainThread";
            }

            Application.Run(new FormReadTagID());
        }
    }
}