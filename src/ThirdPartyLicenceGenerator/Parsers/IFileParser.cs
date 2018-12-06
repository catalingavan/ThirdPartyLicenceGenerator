using System.Collections.Generic;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Parsers
{
    public interface IFileParser
    {
        PackageType PackageType { get; }
        IEnumerable<PackageMetadata> GetPackages(string filePath);
    }
}
