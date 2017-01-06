using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.Redis;

namespace RedisStoreWrapper
{
    internal class PrimaryIndex<T> : IPrimaryIndex<T>
    {
        private readonly SearchTerm<T> _searchTerm;
        private readonly IRedisClient _client;
        private readonly string _keyPrefix;

        public PrimaryIndex(SearchTerm<T> searchTerm, IRedisClient client, string keyPrefix)
        {
            _searchTerm = searchTerm;
            _client = client;
            _keyPrefix = keyPrefix;
        }

        public string PropertyName => _searchTerm.PropertyName;

        private string CreatePrimaryKey(string primaryKeyValue) => $"{_keyPrefix}:primaryKey:{primaryKeyValue}";

        public void Add(T item) => _client[CreatePrimaryKey(_searchTerm.ValueSelector(item))] = item.ToJson();

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public T Find(string value) => _client[CreatePrimaryKey(value)].FromJson<T>();

        public IEnumerable<T> Find(IEnumerable<string> keys) => _client.GetAll<T>(keys.Select(CreatePrimaryKey)).Select(pair => pair.Value);

        public IEnumerable<T> Search(string value) => Find(_client.GetKeysByPattern(CreatePrimaryKey($"*{value}*")));
    }
}