using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThirdPartyLicenceGenerator.Models;
using ThirdPartyLicenceGenerator.Npm;
using ThirdPartyLicenceGenerator.NuGet;
using ThirdPartyLicenceGenerator.Repositories;
using ThirdPartyLicenceGenerator.Services;
using ThirdPartyLicenceGenerator.Web.Mvc.Models;

namespace ThirdPartyLicenceGenerator.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly INuGetPackageRepository _nuGetPackagesRepository;
        private readonly IUiLibraryPackageRepository _uiLibraryPackageRepository;
        public HomeController()
        {
            _nuGetPackagesRepository = new CachedNuGetPackageRepository(
                new NuGetPackageRepository(),
                new InMemoryCacheManager()
            );

            _uiLibraryPackageRepository = new CachedUiLibraryPackageRepository(
                new NpmUiLibraryPackageRepository(),
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
            var service = new GetPackagesService(_nuGetPackagesRepository, _uiLibraryPackageRepository);

            PackagesFileResult<IPackage> result = null;

            using (TemporaryFile tempFile = new TemporaryFile())
            {
                file.SaveAs(tempFile.FileName);

                result = await service.ParseFileAsync(tempFile.FileName, file.FileName);
            }

            var builder = new ParseResultViewModelBuilder();
            var viewModel = builder.Build(new[] {result});

            return View("~/Views/HtmlOutput/HtmlOutput.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ParseDirectory(string path)
        {
            var service = new GetPackagesService(_nuGetPackagesRepository, _uiLibraryPackageRepository);

            IEnumerable<PackagesFileResult<IPackage>> result = await service.ParseDirectoryAsync(path);

            var builder = new ParseResultViewModelBuilder();
            var viewModel = builder.Build(result);

            return View("~/Views/HtmlOutput/HtmlOutput.cshtml", viewModel);
        }
    }
}