using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySqlBackUp
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
                int interval = (24 / settings.CountDoBackUp) * 120 * 1000;
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
            SettingsForm form = new SettingsForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                settings.LoadFromFile();

                if (settings.CountDoBackUp != null)
                {
                    int interval = (24 / settings.CountDoBackUp) * 120 * 1000;
                    timer.Interval = interval;
                    timer.Start();
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

        private void DoBackUp()
        {
            if (!string.IsNullOrEmpty(settings.Path))
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd__hh");

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
            }
        }

    }
}
