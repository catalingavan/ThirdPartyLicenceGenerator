using ThirdPartyLicenceGenerator.Models;
using NuGet;
using System.Threading.Tasks;
using ThirdPartyLicenceGenerator.Repositories;

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
                Version = version,
                SourcePath = $"https://www.nuget.org/packages/{packageId}/{version}"
            };

            global::NuGet.IPackage package = _repository.FindPackage(packageId, SemanticVersion.Parse(version));
            if (package != null)
            {
                result.WasFound = true;
                result.LicencePath = package.LicenseUrl?.AbsoluteUri;
                result.ProjectPath = package.ProjectUrl?.AbsoluteUri;
            }

            return Task.FromResult(result);
        }
    }
}
