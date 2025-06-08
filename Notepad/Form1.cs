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
using System.Drawing.Printing;
using System.Web;

namespace Notepad
{
    public partial class Form1 : Form
    {
        
        // Update strings
        private readonly string _releaseURL = "http://git.vichingo455.freeddns.org/Vichingo455/Notepad/raw/branch/master/latest.txt";
        public Version CurrentVersion = new Version(Application.ProductVersion);
        public Version LatestVersion;

        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text_area.Text = "";
            Global.savefilename = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a file";
            ofd.Multiselect = false;
            ofd.Filter = "Text file|*.txt|All files|*.*";
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
                sfd.Filter = "Text file|*.txt|All files|*.*";
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
            MessageBox.Show("Notepad\nVersion " + Program.GetCurrentVersionTostring() + "" + "\nMade by Vichingo455\nCopyright (C) 2025 Vichingo455. All rights reserved.\n\n\nReleased under GPL 3.0 License: you are free to\n- Modify\n- Use for business\n- Distribute\n- Use at home\n\nBut you have:\n- No liability\n- No warranty\n\nAt these conditions:\n- License and copyright notice\n- State changes\n- Disclose source\n- Same license", Text);
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
                try
                {
                    var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        Program.SendCrashReport(ex);
                    }
                }
                catch
                {

                }
            }
            try
            {
                var equals = LatestVersion.CompareTo(CurrentVersion);

                if (equals == 0)
                {
                    // Up-to-date
                    MessageBox.Show("The program is up-to-date", Text, MessageBoxButtons.OK);
                }
                else if (equals < 0)
                {
                    MessageBox.Show("You are using an unofficial release of Notepad", Text, MessageBoxButtons.OK); // Unofficial
                }
                else // New release available!
                {
                    if (MessageBox.Show("This new version is available:" + " " + LatestVersion + ". You are using the version" + " " + CurrentVersion + ". Do you want to open the download website?", Text, MessageBoxButtons.YesNo) == DialogResult.Yes) // New release available!
                    {
                        Process.Start("https://git.vichingo455.freeddns.org/Vichingo455/Notepad/releases/tag/" + LatestVersion);
                    }
                }
            }
            catch
            {
                
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
            if (File.Exists(Global.savefilename))
            {
                text_area.Text = File.ReadAllText(Global.savefilename);
            }
            if (Global.confidential == "true")
            {
                var dialog = MessageBox.Show("This version is for testing purpouses only!!!\nYou should use it only for debugging.\nPublic publication of this version is fobidden and severally punished!! Do you agree?",Text,MessageBoxButtons.YesNo,MessageBoxIcon.None,MessageBoxDefaultButton.Button2);
                if (dialog == DialogResult.Yes)
                {

                }
                else
                {
                    Environment.Exit(436);
                }
            }
            else
            {

            }
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

        private void characterStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void characterStyleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                FontDialog stl = new FontDialog();
                stl.ShowColor = true;
                stl.AllowVectorFonts = true;
                stl.Font = text_area.Font;
                stl.Color = text_area.ForeColor;
                if (stl.ShowDialog() == DialogResult.OK)
                {
                    text_area.Font = stl.Font;
                    text_area.ForeColor = stl.Color;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        Program.SendCrashReport(ex);
                    }
                }
                catch
                {

                }
            }
        }

        private void printStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
        }

        private void searchTextInTheWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var txt = HttpUtility.UrlEncode(text_area.Text);
                Process.Start("https://google.com/search?q=" + txt);
            }
            catch (Exception ex)
            {
                try
                {
                    var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        Program.SendCrashReport(ex);
                    }
                }
                catch
                {

                }
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog clr = new ColorDialog();
                clr.Color = text_area.BackColor;
                if (clr.ShowDialog() == DialogResult.OK)
                {
                    text_area.BackColor = clr.Color;
                    BackColor = clr.Color;
                    menuStrip1.BackColor = clr.Color;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var dialog = MessageBox.Show("The following exception occured: " + ex.Message + "\nDo you want to send a crash report", this.Text + " - Critical error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        Program.SendCrashReport(ex);
                    }
                }
                catch
                {

                }
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.Checked == true)
            {
                text_area.WordWrap = false;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                text_area.WordWrap = true;
                wordWrapToolStripMenuItem.Checked = true;
            }
        }
    }
    public class Global
    {
        public static string savefilename;
        public static string confidential;
    }
}
