using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        public abstract Task<T> GetOneAsync(string id);
        public abstract Task<List<T>> GetAllAsync();
        public abstract Task AddOneAsync(T entity);
        public abstract Task RemoveOneAsync(string id);
    }
}