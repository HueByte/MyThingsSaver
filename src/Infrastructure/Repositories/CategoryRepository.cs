using Core.Abstraction;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services.CurrentUser;

namespace Infrastructure.Repositories;

public class CategoryRepository : IdentityBaseRepository<string, CategoryModel, AppDbContext>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
}