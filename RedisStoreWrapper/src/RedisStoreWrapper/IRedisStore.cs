using System.Collections.Generic;

namespace RedisStoreWrapper
{
    public interface IRedisStore<T>
    {
        void Add(T item);
        void Add(IEnumerable<T> items);
        T Find(object key);
        IEnumerable<T> Search(string propertyName, object value);
    }
}