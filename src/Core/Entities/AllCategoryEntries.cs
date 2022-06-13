using System.Collections.Generic;
using Core.Models;

namespace Core.Entities
{
    public class AllCategoryEntries
    {
        public AllCategoryEntries()
        {
            CategoryEntries = new List<CategoryEntry>();
            SubCategories = new List<Category>();
        }

        public List<CategoryEntry> CategoryEntries { get; set; }
        public List<Category> SubCategories { get; set; }
    }
}