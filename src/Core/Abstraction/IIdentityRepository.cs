using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Abstraction
{
    public interface IIdentityRepository<Tkey, TEntity> where Tkey : IConvertible where TEntity : IdentityDbModel<Tkey, string>
    {
        Task<TEntity?> GetAsync(Tkey id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<bool> AddAsync(TEntity? entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(Tkey id);
        Task<bool> RemoveAsync(TEntity? entity);
        Task UpdateAsync(TEntity? entity);
        Task UpdateRange(IEnumerable<TEntity> entities);
        Task SaveChangesAsync();
    }
}