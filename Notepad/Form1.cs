using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Net;

namespace Notepad
{
    public partial class Form1 : Form
    {
        // Update strings
        private readonly string _releaseURL = "https://raw.githubusercontent.com/Vichingo455/Notepad/master/latest.txt";
        public Version CurrentVersion = new Version(Application.ProductVersion);
        public Version LatestVersion;

        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text_area.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a file";
            ofd.Multiselect = false;
            ofd.Filter = "*.txt|*.txt|*.*|*.*";
            ofd.DefaultExt = "*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    try
                    {
                        text_area.Text = File.ReadAllText(fileName);
                        Global.savefilename = fileName;
                    }
                    catch (Exception ex)
                    {
                        var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialog == DialogResult.Yes)
                        {
                            Program.SendCrashReport(ex);
                        }
                    }
                }
            }
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(file);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Global.savefilename) || Global.savefilename == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save the file";
                sfd.Filter = "*.txt|*.txt|*.*|*.*";
                sfd.DefaultExt = "*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in sfd.FileNames)
                    {
                        try
                        {
                            File.WriteAllText(fileName, text_area.Text);
                            Global.savefilename = fileName;
                        }
                        catch (Exception ex)
                        {
                            var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialog == DialogResult.Yes)
                            {
                                Program.SendCrashReport(ex);
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    File.WriteAllText(Global.savefilename, text_area.Text);
                }
                catch (Exception ex)
                {
                    var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        Program.SendCrashReport(ex);
                    }
                }
            }
            
        }

        private void saveFileWithNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save the file";
            sfd.Filter = "*.txt|*.txt|*.*|*.*";
            sfd.DefaultExt = "*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in sfd.FileNames)
                {
                    try
                    {
                        File.WriteAllText(fileName, text_area.Text);
                        Global.savefilename = fileName;
                    }
                    catch (Exception ex)
                    { 
                        var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error",MessageBoxButtons.YesNo,MessageBoxIcon.Warning); 
                        if (dialog == DialogResult.Yes)
                        {
                            Program.SendCrashReport(ex);
                        }
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Notepad\nVersion " + Program.GetCurrentVersionTostring() + "\nMaded by Vichingo455\nCopyright (C) 2022 Vichingo455. All rights reserved.\n\n\nReleased under GPL 3.0 License: you are free to\n- Modify\n- Use for business\n- Distribute\n- Use at home\n\nBut you have:\n- No liability\n- No warranty", Text);
        }
        /// <summary>
        /// Check for updates
        /// </summary>
        public void CheckUpdates()
        {
            try
            {
                WebRequest hreq = WebRequest.Create(_releaseURL);
                hreq.Timeout = 10000;
                hreq.Headers.Set("Cache-Control", "no-cache, no-store, must-revalidate");

                WebResponse hres = hreq.GetResponse();
                StreamReader sr = new StreamReader(hres.GetResponseStream());

                LatestVersion = new Version(sr.ReadToEnd().Trim());

                // Done and dispose!
                sr.Dispose();
                hres.Dispose();
            }
            catch (Exception ex)
            {
                var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes)
                {
                    Program.SendCrashReport(ex);
                }
            }

            var equals = LatestVersion.CompareTo(CurrentVersion);

            if (equals == 0)
            {
                // Up-to-date
                MessageBox.Show("The program is up-to-date",Text,MessageBoxButtons.OK);
            }
            else if (equals < 0)
            {
                MessageBox.Show("You are using an unofficial release of Notepad", Text, MessageBoxButtons.OK); // Unofficial
            }
            else // New release available!
            {
                if (MessageBox.Show("This new version is available:" + " " + LatestVersion + ". You are using the version" + " " + CurrentVersion + ". Do you want to open the download website?", Text, MessageBoxButtons.YesNo) == DialogResult.Yes) // New release available!
                {
                    Process.Start("https://github.com/Vichingo455/Notepad/releases/tag/" + LatestVersion);
                }
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckUpdates();
        }

        private void viewToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (alwaysOnTopToolStripMenuItem.Checked == true)
            {
                TopMost = false;
                alwaysOnTopToolStripMenuItem.Checked = false;
            }
            else
            {
                TopMost = true;
                alwaysOnTopToolStripMenuItem.Checked = true;
            }
        }

        private void giveAFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://forms.gle/crZF4YJ6eiAzQ8ic8");
        }

        private void resetTransparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Opacity = 100;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void applyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aboutWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                if (File.Exists(windir + @"\System32\winver.exe"))
                {
                    Process.Start(windir + @"\System32\winver.exe");
                }
            }
            catch (Exception ex)
            {
                var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes)
                {
                    Program.SendCrashReport(ex);
                }
            }
        }
    }
    public class Global
    {
        public static string savefilename;
        public static string transparency;
    }
}
