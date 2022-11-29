using System.Threading.Tasks;
using Core.Models;

namespace Core.Services.CurrentUser.Category
{
    public class CategoryService
    {
        public CategoryService()
        {

        }

        public async Task<CategoryModel> GetCategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return null;
        }
    }
}