using ThirdPartyLicenceGenerator.Models;
using System.Threading.Tasks;

namespace ThirdPartyLicenceGenerator.Repositories
{
    public interface INuGetPackageRepository
    {
        Task<NuGetPackage> FindPackageAsync(string packageId, string version);
    }
}
