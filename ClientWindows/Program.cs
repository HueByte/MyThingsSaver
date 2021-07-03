using ClientWindows.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientWindows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppContext());
        }
    }

    public class AppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public AppContext()
        {
            StartApi();

            trayIcon = new NotifyIcon()
            {
                Icon = new System.Drawing.Icon("Favicon.ico"),
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit),
                    new MenuItem("Open", Open)
                }),
                Visible = true
            };
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }

        void Open(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://localhost:5001/", // For testing
                UseShellExecute = true
            });
        }

        void StartApi()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"{Directory.GetCurrentDirectory()}/App.exe",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }
}
