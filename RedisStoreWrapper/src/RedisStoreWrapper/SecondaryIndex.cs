using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;

namespace RedisStoreWrapper
{
    internal class SecondaryIndex<T> : ISecondaryIndex<T>
    {
        private readonly string _keyPrefix;
        private readonly SearchTerm<T> _primarySearchTerm;
        private readonly SearchTerm<T> _secondarySearchTerm;
        private readonly IRedisClient _client;

        public SecondaryIndex(string keyPrefix, SearchTerm<T> primarySearchTerm, SearchTerm<T> secondarySearchTerm, IRedisClient client)
        {
            _keyPrefix = keyPrefix;
            _primarySearchTerm = primarySearchTerm;
            _secondarySearchTerm = secondarySearchTerm;
            _client = client;
        }

        public string PropertyName => _secondarySearchTerm.PropertyName;

        private string CreateKey(string keyValue) => $"{_keyPrefix}:{_secondarySearchTerm.PropertyName}:{keyValue}";

        private string CreateScanPattern(string value) => CreateKey($"*{value}*");

        public void Add(T item) => _client[CreateKey(_secondarySearchTerm.ValueSelector(item))] = _primarySearchTerm.ValueSelector(item);

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public IEnumerable<string> SearchKeys(string value) => _client.GetKeysByPattern(CreateScanPattern(value));

        public IEnumerable<string> Find(IEnumerable<string> keys) => _client.GetValues<string>(keys.ToList());
    }
}
