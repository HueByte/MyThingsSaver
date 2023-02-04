using System.Text;

namespace MTS.Core.Services.Guide
{
    public class GuideService
    {
        public string WELCOME { get; set; }
        public int WELCOME_SIZE { get; set; }
        public string GUIDE { get; set; }
        public int GUIDE_SIZE { get; set; }

        private static string FolderPath
            => AppContext.BaseDirectory + Path.Combine("Templates");

        public GuideService()
        {
            WELCOME = File.ReadAllText(Path.Combine(FolderPath, "Welcome.md"));
            GUIDE = File.ReadAllText(Path.Combine(FolderPath, "Guide.md"));
            WELCOME_SIZE = Encoding.UTF8.GetByteCount(WELCOME);
            GUIDE_SIZE = Encoding.UTF8.GetByteCount(GUIDE);
        }
    }
}