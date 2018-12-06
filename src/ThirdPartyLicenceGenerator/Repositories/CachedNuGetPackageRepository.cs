using System;
using System.Threading.Tasks;
using ThirdPartyLicenceGenerator.Models;
using ThirdPartyLicenceGenerator.Repositories;

namespace ThirdPartyLicenceGenerator.Repositories
{
    public class CachedNuGetPackageRepository : INuGetPackageRepository
    {
        private readonly INuGetPackageRepository _repository;
        private readonly ICacheManager _cacheManager;
        public CachedNuGetPackageRepository(
            INuGetPackageRepository repository,
            ICacheManager cacheManager)
        {
            _repository = repository;
            _cacheManager = cacheManager;
        }

        public async Task<NuGetPackage> FindPackageAsync(string packageId, string version)
        {
            string key = $"{packageId}_{version}";

            NuGetPackage item = _cacheManager.Get<NuGetPackage>(key);
            if (item != null)
                return item;

            item = await _repository.FindPackageAsync(packageId, version);
            _cacheManager.Set(key, item, TimeSpan.FromDays(1));

            return item;
        }
    }
}
