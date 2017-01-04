using System;

namespace RedisSandbox
{
    public struct SearchTerm<T>
    {
        public SearchTerm(string propertyName, Func<T, string> valueSelector)
        {
            PropertyName = propertyName;
            ValueSelector = valueSelector;
        }

        public readonly string PropertyName;
        public readonly Func<T, string> ValueSelector;
    }
}