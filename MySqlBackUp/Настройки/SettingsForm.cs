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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        Settings settings;

        private void Form_Load(object sender, EventArgs e)
        {
            settings = new Settings();
            settings.LoadFromFile();

            tbPath.Text = settings.Path;
            numCount.Value = settings.CountDoBackUp;
            cbDeleteFile.Checked = settings.IsDeleteOldFiles;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                settings.CountDoBackUp = (int)numCount.Value;
                settings.Path = tbPath.Text;
                settings.IsDeleteOldFiles = cbDeleteFile.Checked;

                settings.SaveToFile();
            }
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            folderBrowser.Description = "Выберите каталог в который будут копироваться резервные копии БД";
            if(folderBrowser.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = folderBrowser.SelectedPath;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
