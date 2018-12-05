using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThirdPartyLicenceGenerator.Models;
using ThirdPartyLicenceGenerator.NuGet;
using ThirdPartyLicenceGenerator.Services;
using ThirdPartyLicenceGenerator.Web.Mvc.Models;

namespace ThirdPartyLicenceGenerator.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly INuGetPackageRepository _nuGetPackagesRepository;
        public HomeController()
        {
            _nuGetPackagesRepository = new CachedNuGetPackageRepository(
                new NuGetPackageRepository(),
                new InMemoryCacheManager()
            );
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ParseFile(HttpPostedFileBase file)
        {
            var service = new GetPackagesService(_nuGetPackagesRepository);

            PackagesFileResult<IPackage> result = null;

            using (TemporaryFile tempFile = new TemporaryFile())
            {
                file.SaveAs(tempFile.FileName);

                result = await service.ParseFileAsync(tempFile.FileName, file.FileName);
            }

            var viewModel = new List<PackagesViewModel>
            {
                new PackagesViewModel
                {
                    Result = result
                }
            };

            return View("ParsedPackages", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ParseDirectory(string path)
        {
            var service = new GetPackagesService(_nuGetPackagesRepository);

            IEnumerable<PackagesFileResult<IPackage>> result = await service.ParseDirectoryAsync(path);

            var viewModels = result.Select(p => new PackagesViewModel
            {
                Result = p
            })
            .ToList();

            return View("ParsedPackages", viewModels);
        }
    }
}