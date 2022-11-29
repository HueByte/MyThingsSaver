using System;

namespace Core.Abstraction
{
    public class DbModel<TKey> where TKey : IConvertible
    {
        public virtual TKey Id { get; set; }
    }
}