using System.Collections.Generic;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator
{
    public class PackagesFileResult<T> where T : IPackage
    {
        public PackageType PackageType { get; set; }
        public string FileName { get; set; }
        public IEnumerable<T> Packages { get; set; }

        public PackagesFileResult()
        {
            Packages = new List<T>();
        }
    }
}
