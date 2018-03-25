using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLW.Lib.SqliteHelper;

namespace BLW.Modules.WindowsService.LicenceApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Initializing Sqlite db helper
            SqliteDbHelper.Initialize(); 
           Application.Run(new frmManageLicences());                                          
        }
    }
}
