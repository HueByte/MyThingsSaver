namespace Core.DTO
{
    public class EntryDTO
    {
        public string? CategoryId { get; set; }
        public string? EntryId { get; set; }
        public string? EntryName { get; set; }
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
    }
}