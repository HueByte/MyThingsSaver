using ClientWindows.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                FileName = "https://google.pl/",
                UseShellExecute = true
            });
        }
    }
}
