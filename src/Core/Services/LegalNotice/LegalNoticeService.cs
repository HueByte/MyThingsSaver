namespace MTS.Core.Services.LegalNotice
{
    public class LegalNoticeService
    {
        public string LEGAL_NOTICE { get; set; } = string.Empty;

        private static string FolderPath
            => AppContext.BaseDirectory + Path.Combine("Templates");

        public LegalNoticeService()
        {
            LEGAL_NOTICE = File.ReadAllText(Path.Combine(FolderPath, "LegalNotice.md"));
        }
    }
}