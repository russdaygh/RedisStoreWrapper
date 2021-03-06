using System.Collections.Generic;

namespace RedisStoreWrapper
{
    // ReSharper disable once TypeParameterCanBeVariant
    internal interface IIndex<T>
    {
        string PropertyName { get; }

        void Add(T item);

        void Add(IEnumerable<T> items);
    }
}