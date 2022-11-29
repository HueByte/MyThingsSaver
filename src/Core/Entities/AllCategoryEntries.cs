using System.Collections.Generic;
using Core.Models;

namespace Core.Entities
{
    public class AllCategoryEntries
    {
        public AllCategoryEntries()
        {
            Entries = new List<EntryModel>();
            SubCategories = new List<CategoryModel>();
        }

        public List<EntryModel> Entries { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
    }
}