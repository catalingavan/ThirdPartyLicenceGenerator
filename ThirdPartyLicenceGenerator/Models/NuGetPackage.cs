using System;

namespace ThirdPartyLicenceGenerator.Models
{
    public class NuGetPackage : IPackage
    {
        public bool WasFound { get; set; }

        public string PackageId { get; set; }
        public string Version { get; set; }
        public Uri LicenceUrl { get; set; }
        public Uri ProjectUrl { get; set; }

        public string GetFullName()
        {
            return $"{PackageId} {Version}";
        }
    }
}
