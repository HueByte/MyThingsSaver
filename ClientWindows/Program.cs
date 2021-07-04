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

        private Process mainApp;

        public AppContext()
        {
            StartMainApp();

            trayIcon = new NotifyIcon()
            {
                Text = "My Things Saver",
                Icon = new System.Drawing.Icon("Favicon.ico"),
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Open", Open),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            trayIcon.ShowBalloonTip(500, "New info", "Starting app", ToolTipIcon.Info);
        }

        void Exit(object sender, EventArgs e)
        {
            // close server 
            mainApp.Kill();

            // show feedback
            trayIcon.ShowBalloonTip(500, "Closing App", "Your application server has been closed", ToolTipIcon.Info);
            trayIcon.Visible = false;
            
            // exit app
            Application.Exit();
        }

        void Open(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://localhost:5001/", // For testing
                UseShellExecute = true
            });
        }

        void StartMainApp()
        {
            mainApp = Process.Start(new ProcessStartInfo
            {
                FileName = $"{Directory.GetCurrentDirectory()}/App.exe",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }

        private void AppContext_Closing(object sender, FormClosedEventArgs e)
        {
            mainApp.Close();
        }
    }
}
