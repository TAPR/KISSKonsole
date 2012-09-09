using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KISS_Konsole
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // allow a user to pass command line arguments to do things like set a non-default application data directory
            // so that multiple discrete instances of KISS Konsole could be run on the system.  Note that the installer only allows
            // one instance to be 'installed'.  But others could be created by copying the installed instance...
            string appDataDir = "";
            // NOTE: these args do NOT include the program name!
            if (args != null)
            {
                if (args.Length >= 2)
                {
                    if (args[0].ToUpper().Equals("APPDATADIR"))
                    {
                        appDataDir = args[1];
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(appDataDir));
        }
    }
}