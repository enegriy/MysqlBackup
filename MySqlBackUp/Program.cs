using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace MysqlBackup
{
    static class Program
    {
        
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool isOnlyInstance;
            Mutex mutex = new Mutex(true, "MysqlBackup", out isOnlyInstance);
            if(isOnlyInstance)
                Application.Run(new MainForm());
        }
    }
}
