using System.Collections.Generic;
using System.Linq;

namespace RedisStoreWrapper
{
    internal class RedisStore<T> : IRedisStore<T>
    {
        private readonly IPrimaryIndex<T> _primaryIndex;
        private readonly List<ISecondaryIndex<T>> _secondaryIndices;

        public RedisStore(IPrimaryIndex<T> primaryIndex, IEnumerable<ISecondaryIndex<T>> secondaryIndices)
        {
            _primaryIndex = primaryIndex;
            _secondaryIndices = secondaryIndices.ToList();
        }

        public void Add(T item)
        {
            _primaryIndex.Add(item);
            _secondaryIndices.ForEach(index => index.Add(item));
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public T Find(object key) => _primaryIndex.Find(key.ToString());

        public IEnumerable<T> Search(string propertyName, object value)
        {
            if (propertyName == _primaryIndex.PropertyName) return _primaryIndex.Search(value.ToString());

            var secondaryIndex = _secondaryIndices.Single(index => index.PropertyName == propertyName);

            return _primaryIndex.Find(secondaryIndex.Find(secondaryIndex.SearchKeys(value.ToString())));
        }
    }
}
