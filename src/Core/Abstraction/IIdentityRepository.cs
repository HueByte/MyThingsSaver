using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Abstraction
{
    public interface IIdentityRepository<Tkey, TEntity> : IRepository<Tkey, TEntity>
        where Tkey : IConvertible
        where TEntity : IdentityDbModel<Tkey, string>
    {
    }
}