using System.Collections.Generic;

namespace RedisStoreWrapper
{
    internal interface IPrimaryIndex<T> : IIndex<T>
    {
        T Find(string key);

        IEnumerable<T> Find(IEnumerable<string> keys);

        IEnumerable<T> Search(string value);
    }
}