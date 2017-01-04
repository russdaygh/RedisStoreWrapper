using System.Collections.Generic;

namespace RedisSandbox
{
    internal interface ISecondaryIndex<T> : IIndex<T>
    {
        IEnumerable<string> SearchKeys(string value);

        IEnumerable<string> Find(IEnumerable<string> keys);
    }
}