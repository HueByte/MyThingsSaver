using System.Collections.Generic;
using Core.Models;

namespace Core.Entities
{
    public class AllCategoryEntries
    {
        public AllCategoryEntries()
        {
            CategoryEntries = new List<EntryModel>();
            SubCategories = new List<CategoryModel>();
        }

        public List<EntryModel> CategoryEntries { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
    }
}