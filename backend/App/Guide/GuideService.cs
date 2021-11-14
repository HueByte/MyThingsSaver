using System;
using System.IO;
using System.Text;

namespace App.Guide
{
    public class GuideService
    {
        public string WELCOME { get; set; }
        public int WELCOME_SIZE { get; set; }
        public string GUIDE { get; set; }
        public int GUIDE_SIZE { get; set; }

        private static string FolderPath
            => AppContext.BaseDirectory + Path.Combine("Guide", "Templates");

        public GuideService()
        {
            WELCOME = File.ReadAllText(Path.Combine(FolderPath, "Welcome.md"));
            GUIDE = File.ReadAllText(Path.Combine(FolderPath, "Guide.md"));
            WELCOME_SIZE = ASCIIEncoding.Unicode.GetByteCount(WELCOME);
            GUIDE_SIZE = ASCIIEncoding.Unicode.GetByteCount(GUIDE);
        }
    }
}