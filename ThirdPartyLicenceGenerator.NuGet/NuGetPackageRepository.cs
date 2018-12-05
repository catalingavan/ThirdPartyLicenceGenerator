using ThirdPartyLicenceGenerator.Models;
using NuGet;
using System.Threading.Tasks;

namespace ThirdPartyLicenceGenerator.NuGet
{
    public class NuGetPackageRepository : INuGetPackageRepository
    {
        private readonly IPackageRepository _repository;
        public NuGetPackageRepository()
        {
            _repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
        }

        public Task<NuGetPackage> FindPackageAsync(string packageId, string version)
        {
            NuGetPackage result = new NuGetPackage
            {
                WasFound = false,
                PackageId = packageId,
                Version = version
            };

            global::NuGet.IPackage package = _repository.FindPackage(packageId, SemanticVersion.Parse(version));
            if (package != null)
            {
                result.WasFound = true;
                result.LicenceUrl = package.LicenseUrl;
                result.ProjectUrl = package.ProjectUrl;
            }

            return Task.FromResult(result);
        }
    }
}
