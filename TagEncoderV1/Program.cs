using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagEncoderV1
{
    static class Program
    {
        static WizardParameters wp = new WizardParameters();

        //Logging
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            log.Info("Application started.");

            
            Application.Run(new Form1());
            //Application.Run(new frmMain());
            log.Info("Application Closed");
        }
    }
}
