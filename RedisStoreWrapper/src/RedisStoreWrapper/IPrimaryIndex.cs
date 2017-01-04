using System.Collections.Generic;

namespace RedisSandbox
{
    internal interface IPrimaryIndex<T> : IIndex<T>
    {
        T Find(string key);

        IEnumerable<T> Find(IEnumerable<string> keys);

        IEnumerable<T> Search(string value);
    }
}