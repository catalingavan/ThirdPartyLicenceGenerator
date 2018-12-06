using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Parsers
{
    public class PackagesConfigFileParser : IFileParser
    {
        public PackageType PackageType { get; } = PackageType.NuGet;

        public IEnumerable<PackageMetadata> GetPackages(string filePath)
        {
            if (!File.Exists(filePath))
                return Enumerable.Empty<PackageMetadata>();

            List<PackageMetadata> result = new List<PackageMetadata>();

            XDocument xDoc = XDocument.Load(filePath);
            foreach (XElement item in xDoc.Root.Descendants("package").ToList())
            {
                string packageId = item.Attribute("id")?.Value;
                string version = item.Attribute("version")?.Value;

                if (string.IsNullOrEmpty(packageId) || string.IsNullOrEmpty(version))
                    continue;

                PackageMetadata metadata = new PackageMetadata
                {
                    PackageId = packageId,
                    Version = version
                };
                result.Add(metadata);
            }

            return result;
        }
    }
}
