using System;
using System.Collections.Generic;

namespace ThirdPartyLicenceGenerator
{
    public class InMemoryCacheManager : ICacheManager
    {
        private readonly Dictionary<string, object> _cache;
        public InMemoryCacheManager()
        {
            _cache = new Dictionary<string, object>();
        }

        public void Set(string key, object item, TimeSpan expiry)
        {
            if (_cache.ContainsKey(key))
                _cache.Remove(key);

            _cache.Add(key, item);
        }

        public T Get<T>(string key)
        {
            if (!_cache.ContainsKey(key))
                return default(T);

            var item = _cache[key];
            return (T) item;
        }

        public void Remove(string key)
        {
            if (_cache.ContainsKey(key))
                _cache.Remove(key);
        }
    }
}
