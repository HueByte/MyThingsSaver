using MTS.Core.Abstraction;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace MTS.Infrastructure.Repositories;

public class CategoryRepository : IdentityBaseRepository<string, CategoryModel, MTSContext>, ICategoryRepository
{
    public CategoryRepository(MTSContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
}