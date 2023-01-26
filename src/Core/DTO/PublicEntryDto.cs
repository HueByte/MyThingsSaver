namespace Core.DTO
{
    public class PublicEntryDto
    {
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string? Content { get; set; }
        public int Size { get; set; }
        public string Owner { get; set; } = string.Empty;
    }
}