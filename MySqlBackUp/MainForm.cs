using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MysqlBackup
{
    public partial class MainForm : Form
    {
        Settings settings;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        private void Form_Load(object sender, EventArgs e)
        {
            ShowInTaskbar = false;
            settings = new Settings();
            settings.LoadFromFile();

            if (settings.CountDoBackUp != null)
            {
                DoBackUp();
                int interval = (24 / settings.CountDoBackUp) * 3600000;
                timer.Interval = interval;
                timer.Start();
            }
        }

        /// <summary>
        /// Закрыть
        /// </summary>
        private void MiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miSettings_Click(object sender, EventArgs e)
        {
            var storeAutorun = settings.IsAutorun;

            SettingsForm form = new SettingsForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                settings.LoadFromFile();

                if (settings.CountDoBackUp != null)
                {
                    int interval = (24 / settings.CountDoBackUp) * 3600 * 1000;
                    timer.Interval = interval;
                    timer.Start();
                }

                if (storeAutorun != settings.IsAutorun)
                {
                    if (settings.IsAutorun)
                        AddAutorun();
                    else
                        DeleteAutorun();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DoBackUp();
        }

        private void miDoBackUp_Click(object sender, EventArgs e)
        {
            DoBackUp();
        }

        /// <summary>
        /// Сделать бэкап
        /// </summary>
        private void DoBackUp()
        {
            if (!string.IsNullOrEmpty(settings.Path))
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd__HH");

                System.Diagnostics.ProcessStartInfo psi =
                    new System.Diagnostics.ProcessStartInfo();

                psi.FileName = "cmd";
                psi.Arguments = string.Format(
                    "/k mysqldump -u root --password=  --default-character-set=cp1251 fitness > {0}\\{1}.bak && exit",
                    settings.Path,
                    fileName);

                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                System.Diagnostics.Process.Start(psi);

                if (settings.IsDeleteOldFiles)
                {
                    DeleteOldFiles(settings.Path);
                }
            }
        }

        /// <summary>
        /// Удалить старые файлы
        /// </summary>
        /// <param name="path"></param>
        private void DeleteOldFiles(string path)
        {
            var files = Directory.GetFiles(path,"*.bak");
            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((DateTime.Now - fi.CreationTime).Days > 30)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Добавить автозапус
        /// </summary>
        private void AddAutorun()
        {
            Microsoft.Win32.RegistryKey myKey =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\\", true);
            myKey.SetValue("MysqlBackup", Application.ExecutablePath);
            myKey.Close();
        }

        /// <summary>
        /// Удалить автозапус
        /// </summary>
        private void DeleteAutorun()
        {
            Microsoft.Win32.RegistryKey myKey =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\\", true);
            myKey.DeleteValue("MysqlBackup",false);
            myKey.Close();
        }

    }
}
