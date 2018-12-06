using System.Collections.Generic;
using System.Linq;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Parsers
{
    public class NullFileParser : IFileParser
    {
        public PackageType PackageType { get; } = PackageType.NuGet;
        public IEnumerable<PackageMetadata> GetPackages(string filePath)
        {
            return Enumerable.Empty<PackageMetadata>();
        }
    }
}
