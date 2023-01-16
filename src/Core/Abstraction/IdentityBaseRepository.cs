using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace MTS.Core.Abstraction
{
    public class IdentityBaseRepository<TKeyType, TEntity, TContext> : IIdentityRepository<TKeyType, TEntity>
        where TKeyType : IConvertible
        where TEntity : IdentityDbModel<TKeyType, string>, new()
        where TContext : IdentityDbContext<ApplicationUserModel>, new()
    {
        protected internal readonly TContext _context;
        private readonly ICurrentUserService _currentUser;
        public IdentityBaseRepository(TContext context, ICurrentUserService currentUser)
        {
            _context = context ?? new TContext();
            _currentUser = currentUser;
        }

        public virtual async Task<bool> AddAsync(TEntity? entity)
        {
            if (entity is null) return false;

            var doesExist = await _context.Set<TEntity>().AnyAsync(entry => entry.Id.Equals(entity.Id) && entry.UserId.Equals(_currentUser.UserId));

            if (doesExist) return false;

            await _context.Set<TEntity>().AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities is null) return false;

            await _context.Set<TEntity>().AddRangeAsync(entities);
            return true;
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>()
                .AsQueryable();
        }

        public virtual Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_context.Set<TEntity>()
                .Where(cat => cat.UserId == _currentUser.UserId)
                .AsQueryable());
        }

        public virtual async Task<TEntity?> GetAsync(TKeyType id)
        {
            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(entry => entry.Id.Equals(id) && entry.UserId.Equals(_currentUser.UserId));
        }

        public virtual async Task<bool> RemoveAsync(TKeyType id)
        {
            TEntity entity = new() { Id = id };

            var doesExist = await _context.Set<TEntity>().AnyAsync(entry => entry.Id.Equals(entity.Id) && entry.UserId.Equals(_currentUser.UserId));

            if (!doesExist) return false;

            _context.Set<TEntity>().Remove(entity);

            return true;
        }

        public virtual async Task<bool> RemoveAsync(TEntity? entity)
        {
            if (entity is null) return false;

            var doesExist = await _context.Set<TEntity>().AnyAsync(entry => entry.Id.Equals(entity.Id) && entry.UserId.Equals(_currentUser.UserId));
            if (!doesExist) return false;

            _context.Set<TEntity>().Remove(entity);

            return true;
        }

        public virtual Task UpdateAsync(TEntity? entity)
        {
            if (entity is null) return Task.CompletedTask;

            _context.Set<TEntity>().Update(entity);

            return Task.CompletedTask;
        }

        public virtual Task UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);

            return Task.CompletedTask;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}