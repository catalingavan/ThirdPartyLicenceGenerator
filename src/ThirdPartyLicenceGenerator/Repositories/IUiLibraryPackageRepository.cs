using System.Threading.Tasks;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Repositories
{
    public interface IUiLibraryPackageRepository
    {
        Task<UiLibraryPackage> FindPackageAsync(string packageId);
    }
}
