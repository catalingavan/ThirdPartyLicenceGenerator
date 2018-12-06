using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyLicenceGenerator.Models;
using ThirdPartyLicenceGenerator.Parsers;
using ThirdPartyLicenceGenerator.Repositories;

namespace ThirdPartyLicenceGenerator.Services
{
    public class GetPackagesService
    {
        private readonly INuGetPackageRepository _nuGetPackagesRepository;
        private readonly IUiLibraryPackageRepository _uiLibraryPackageRepository;
        public GetPackagesService(
            INuGetPackageRepository nuGetPackagesRepository,
            IUiLibraryPackageRepository uiLibraryPackageRepository)
        {
            _nuGetPackagesRepository = nuGetPackagesRepository;
            _uiLibraryPackageRepository = uiLibraryPackageRepository;
        }

        public async Task<PackagesFileResult<IPackage>> ParseFileAsync(string filePath, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = Path.GetFileName(filePath);

            IFileParser fileParser = FileParserFactory.CreateParser(fileName);
            IEnumerable<PackageMetadata> packagesMetadata = fileParser.GetPackages(filePath);

            List<Task<NuGetPackage>> tasks = new List<Task<NuGetPackage>>();

            foreach (var item in packagesMetadata)
            {
                tasks.Add(_nuGetPackagesRepository.FindPackageAsync(item.PackageId, item.Version));
            }

            await Task.WhenAll(tasks);

            List<NuGetPackage> packages = tasks.Select(p => p.Result).ToList();

            return new PackagesFileResult<IPackage>
            {
                PackageType = fileParser.PackageType,
                FileName = fileName,
                Packages = packages.OrderBy(p => p.PackageId).ToList()
            };
        }

        public async Task<IEnumerable<PackagesFileResult<IPackage>>> ParseDirectoryAsync(string directoryPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            List<PackagesFileResult<IPackage>> result = new List<PackagesFileResult<IPackage>>();

            var files = dirInfo.EnumerateFiles("packages.config", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var item = await ParseFileAsync(file.FullName);
                result.Add(item);
            }

            files = dirInfo.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var item = await ParseFileAsync(file.FullName);
                result.Add(item);
            }

            PackagesFileResult<IPackage> uiFrameworks = await ParseUiFrameworksAsync(dirInfo);
            result.Add(uiFrameworks);

            return result;
        }

        private async Task<PackagesFileResult<IPackage>> ParseUiFrameworksAsync(DirectoryInfo dirInfo)
        {
            IEnumerable<PackageMetadata> packagesMetadata = FindUiPackages(dirInfo);

            List<Task<UiLibraryPackage>> tasks = new List<Task<UiLibraryPackage>>();

            foreach (var item in packagesMetadata)
            {
                tasks.Add(_uiLibraryPackageRepository.FindPackageAsync(item.PackageId));
            }

            await Task.WhenAll(tasks);

            List<UiLibraryPackage> packages = tasks.Select(p => p.Result).ToList();

            return new PackagesFileResult<IPackage>
            {
                PackageType = PackageType.UiLibrary,
                FileName = "Ui libraries",
                Packages = packages.OrderBy(p => p.PackageId).ToList()
            };
        }

        private IEnumerable<PackageMetadata> FindUiPackages(DirectoryInfo rootDirectory)
        {
            List<string> licenceFiles = new List<string> { "license", "licence", "copying" };
            List<string> uiLibrariesDirectories = new List<string> { "wwwroot", "content" };

            List<FileInfo> files = new List<FileInfo>();

            foreach (string directoryName in uiLibrariesDirectories)
            {
                foreach (DirectoryInfo uiLibraryDirectory in rootDirectory.EnumerateDirectories(directoryName, SearchOption.AllDirectories))
                {
                    foreach (var item in licenceFiles)
                    {
                        files.AddRange(
                            uiLibraryDirectory.EnumerateFiles(item, SearchOption.AllDirectories)
                        );

                        files.AddRange(
                            uiLibraryDirectory.EnumerateFiles($"{item}.txt", SearchOption.AllDirectories)
                        );

                        files.AddRange(
                            uiLibraryDirectory.EnumerateFiles($"{item}.md", SearchOption.AllDirectories)
                        );
                    }
                }
            }

            IEnumerable<string> packageIds =
                files.Select(p => p.Directory?.Name).Where(p => !string.IsNullOrEmpty(p)).Distinct().OrderBy(p => p).ToList();

            IEnumerable<PackageMetadata> packages = packageIds.Select(p => new PackageMetadata
            {
                PackageId = p
            }).ToList();

            return packages;
        }
    }
}
