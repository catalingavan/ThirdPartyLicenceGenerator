using System.Collections.Generic;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Parsers
{
    public interface IFileParser
    {
        IEnumerable<PackageMetadata> GetPackages(string filePath);
    }
}
