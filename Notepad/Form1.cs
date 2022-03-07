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

namespace Notepad
{
    public partial class Form1 : Form
    {
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
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message, this.Text); }
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
                        { MessageBox.Show(ex.Message, this.Text); }
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
                { MessageBox.Show(ex.Message, this.Text); }
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
                    { MessageBox.Show(ex.Message, this.Text); }
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
            MessageBox.Show("", Text);
        }
    }
    public class Global
    {
        public static string savefilename;
    }
}
