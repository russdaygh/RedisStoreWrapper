using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;

namespace RedisStoreWrapper
{
    public class RedisStoreBuilder<T>
    {
        private readonly SearchTerm<T> _primarySearchTerm;
        private readonly RedisEndpoint _redisEndpoint;
        private readonly List<SearchTerm<T>> _searchTerms = new List<SearchTerm<T>>();
        private readonly List<T> _initialData = new List<T>();

        public RedisStoreBuilder(SearchTerm<T> primarySearchTerm, RedisEndpoint redisEndpoint)
        {
            _primarySearchTerm = primarySearchTerm;
            _redisEndpoint = redisEndpoint;
        }

        public RedisStoreBuilder<T> AddData(IEnumerable<T> items)
        {
            _initialData.AddRange(items);

            return this;
        }
        
        public RedisStoreBuilder<T> AddSearchTerm(SearchTerm<T> searchTerm)
        {
            _searchTerms.Add(searchTerm);

            return this;
        }
        
        public IRedisStore<T> Build()
        {
            var keyPrefix = typeof(T).Name;
            var redisClient = new RedisClient(_redisEndpoint);

            var store = new RedisStore<T>(new PrimaryIndex<T>(_primarySearchTerm, redisClient, keyPrefix),
                _searchTerms.Select(term => new SecondaryIndex<T>(keyPrefix, _primarySearchTerm, term, redisClient))
                    .ToList());

            store.Add(_initialData);

            return store;
        }
    }
}