using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NoteApp
{
    public partial class PreferenceDlg : Form
    {
        NoteAppPreference objPreference;
        public PreferenceDlg()
        {
            InitializeComponent();
        }

        private void buttonBrowseLocal_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxLocalCache.Text = folderBrowserDialog1.SelectedPath;
                objPreference.SetLocalCacheDir(textBoxLocalCache.Text);
            }
        }

        private void buttonServerBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxServerCache.Text = folderBrowserDialog1.SelectedPath;
                objPreference.SetServerInfo(textBoxServerCache.Text);
            }

        }
        public void SetPreference(NoteAppPreference obj)
        {
            objPreference = obj;
            string strCurDir = Directory.GetCurrentDirectory();
            if (String.IsNullOrEmpty ( objPreference.GetLocalServer()))
            {
                textBoxLocalCache.Text = strCurDir + "\\Local";
                obj.SetLocalCacheDir(textBoxLocalCache.Text);
            }
            else
            {
                textBoxLocalCache.Text = objPreference.GetLocalServer();
            }
            if(String.IsNullOrEmpty(objPreference.GetServerInfo()))
            {
                textBoxServerCache.Text = strCurDir + "\\Destination";
                obj.SetServerInfo(textBoxServerCache.Text);
            }
            else
            {
                textBoxServerCache.Text = objPreference.GetServerInfo();
            }
        }
    }
}
