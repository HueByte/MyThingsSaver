using System;
using System.IO;

namespace App.Guide
{
    public class GuideService
    {
        public string WELCOME { get; set; }
        public string GUIDE { get; set; }

        private static string FolderPath
            => AppContext.BaseDirectory + @"\Guide\Templates";

        public GuideService()
        {
            WELCOME = File.ReadAllText(Path.Combine(FolderPath, "Welcome.md"));
            GUIDE = File.ReadAllText(Path.Combine(FolderPath, "Guide.md"));
        }
    }
}