using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ThirdPartyLicenceGenerator.Models;
using ThirdPartyLicenceGenerator.Parsers;

namespace ThirdPartyLicenceGenerator.Services
{
    public class GetPackagesService
    {
        private readonly INuGetPackageRepository _nuGetPackagesRepository;
        public GetPackagesService(
            INuGetPackageRepository nuGetPackagesRepository)
        {
            _nuGetPackagesRepository = nuGetPackagesRepository;
        }

        public async Task<PackagesFileResult<IPackage>> ParseFileAsync(string filePath, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = Path.GetFileName(filePath);

            string extension = System.IO.Path.GetExtension(fileName);

            IFileParser fileParser = null;
            List<IPackage> packages = new List<IPackage>();
            PackagesFileResult<IPackage> result = new PackagesFileResult<IPackage>
            {
                FileName = fileName,
                Packages = packages
            };

            if (string.Compare(fileName, "packages.config", StringComparison.OrdinalIgnoreCase) == 0)
            {
                fileParser = new PackagesConfigFileParser();
                result.PackageType = PackageType.NuGet;
            }
            else if (string.Compare(extension, ".csproj", StringComparison.OrdinalIgnoreCase) == 0)
            {
                fileParser = new CsprojFileParser();
                result.PackageType = PackageType.NuGet;
            }

            if(fileParser == null)
                return result;

            IEnumerable<PackageMetadata> packagesMetadata = fileParser.GetPackages(filePath);

            foreach (var item in packagesMetadata)
            {
                NuGetPackage package = await _nuGetPackagesRepository.FindPackageAsync(item.PackageId, item.Version);
                packages.Add(package);
            }

            return result;
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

            return result;
        }
    }
}
