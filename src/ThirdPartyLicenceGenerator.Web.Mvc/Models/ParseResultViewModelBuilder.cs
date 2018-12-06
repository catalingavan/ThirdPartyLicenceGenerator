using System.Collections.Generic;
using System.Linq;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Web.Mvc.Models
{
    public class ParseResultViewModelBuilder
    {
        public ParseResultViewModel Build(IEnumerable<PackagesFileResult<IPackage>> files)
        {
            var viewModel = new ParseResultViewModel
            {
                Files = GetDistinctPackages(files)
            };

            return viewModel;
        }

        private IEnumerable<PackagesFileResult<IPackage>> GetDistinctPackages(IEnumerable<PackagesFileResult<IPackage>> files)
        {
            List<PackagesFileResult<IPackage>> result = new List<PackagesFileResult<IPackage>>();

            foreach (var item1 in files.GroupBy(p => p.PackageType))
            {
                List<IPackage> uniquePackages = new List<IPackage>();

                var packages = item1.SelectMany(p => p.Packages).ToList();

                foreach (var package in packages)
                {
                    IPackage existing = uniquePackages.FirstOrDefault(p => p.PackageId == package.PackageId);
                    if (existing == null)
                    {
                        uniquePackages.Add(package);
                    }
                }

                result.Add(new PackagesFileResult<IPackage>
                {
                    FileName = $"{item1.Key}",
                    PackageType = item1.Key,
                    Packages = uniquePackages.OrderBy(p => p.PackageId).ToList()
                });
            }

            return result;
        }
    }
}