using System;

namespace ThirdPartyLicenceGenerator
{
    public interface ICacheManager
    {
        void Set(string key, object item, TimeSpan expiry);

        T Get<T>(string key);

        void Remove(string key);
    }
}
