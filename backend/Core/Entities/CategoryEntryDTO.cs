using System.Dynamic;

namespace Core.Entities
{
    public class CategoryEntryDTO
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string EntryName { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
    }
}