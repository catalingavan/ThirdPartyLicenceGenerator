using System;
using System.Threading.Tasks;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Repositories
{
    public class CachedUiLibraryPackageRepository : IUiLibraryPackageRepository
    {
        private readonly IUiLibraryPackageRepository _repository;
        private readonly ICacheManager _cacheManager;
        public CachedUiLibraryPackageRepository(
            IUiLibraryPackageRepository repository,
            ICacheManager cacheManager)
        {
            _repository = repository;
            _cacheManager = cacheManager;
        }

        public async Task<UiLibraryPackage> FindPackageAsync(string packageId)
        {
            string key = $"{packageId}";

            UiLibraryPackage item = _cacheManager.Get<UiLibraryPackage>(key);
            if (item != null)
                return item;

            item = await _repository.FindPackageAsync(packageId);
            _cacheManager.Set(key, item, TimeSpan.FromDays(1));

            return item;
        }
    }
}
