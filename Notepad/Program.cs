using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics;

namespace Notepad
{
    internal static class Program
    {
        /// <summary>
        /// Get app version
        /// </summary>
        /// <returns></returns>
        internal static string GetCurrentVersionTostring() => new Version(Application.ProductVersion).ToString(4);
        /// <summary>
        /// Sends Crash report to GitHub with exception details
        /// </summary>
        public static void SendCrashReport(Exception ex)
        {
            string header = HttpUtility.UrlEncode(ex.Message);
            string body = $"***Notepad version {GetCurrentVersionTostring()}***\n\n" +
                          $"**Message**\n{ex.Message}\n\n" +
                          $"**Source**\n{ex.Source}\n\n" +
                          $"**Stack Trace**\n```{ex.StackTrace}```\n\n";

            body = HttpUtility.UrlEncode(body);

            string uri = $"https://github.com/builtbybel/ThisIsWin11/issues/new?labels=crash+report&title={header}&body={body}";
            Process.Start(uri);
        }
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
