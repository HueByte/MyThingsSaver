using System;

namespace Core.Abstraction
{
    public class IdentityDbModel<TKey, TUserKey> where TKey : IConvertible
    {
        public virtual TKey Id { get; set; }
        public virtual TUserKey UserId { get; set; }
    }
}