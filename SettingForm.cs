using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using System.Windows;
//using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace CLIPSAVE
{
    public partial class SettingForm : Form
    {
        private string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        [DllImport("user32.dll", SetLastError = true)]
        private extern static void RemoveClipboardFormatListener(IntPtr hwnd);

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == 0x031D)
            {
                Console.WriteLine("onchange");
                this.OnClipboardUpdated();
            }
            else
            {
                base.WndProc(ref m);
            }        
        }

        private void OnClipboardUpdated()
        {
            if (Clipboard.ContainsImage())
            {
                Image image = Clipboard.GetImage();
                string path = this.path + "\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".jpg";
                image.Save(path);
                Clipboard.Clear();
            }
        }

        public SettingForm()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Title = "フォルダを選択してください";
            openFileDialog.IsFolderPicker = true;
            if(openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textBox1.Text = openFileDialog.FileName;
            }
            openFileDialog.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(@textBox1.Text))
            {
                this.path = textBox1.Text;
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("フォルダが存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveClipboardFormatListener(Handle);
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }
    }
}
