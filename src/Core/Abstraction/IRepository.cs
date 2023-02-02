namespace MTS.Core.Abstraction
{
    public interface IRepository<Tkey, TEntity>
        where Tkey : IConvertible
        where TEntity : DbModel<Tkey>
    {
        Task<TEntity?> GetAsync(Tkey id);
        IQueryable<TEntity> AsQueryable();
        Task<bool> AddAsync(TEntity? entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(Tkey id);
        Task<bool> RemoveAsync(TEntity? entity);
        Task UpdateAsync(TEntity? entity);
        Task UpdateRange(IEnumerable<TEntity> entities);
        Task SaveChangesAsync();
    }
}