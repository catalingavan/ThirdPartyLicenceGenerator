using System;
using System.Linq;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Web.Mvc.Models
{
    public static class ExtensionMethods
    {
        public static PackagesFileResult<T> As<T>(this PackagesFileResult<IPackage> item) where T : IPackage
        {
            return new PackagesFileResult<T>
            {
                FileName = item.FileName,
                Packages = item.Packages.Select(p => (T)p).ToList()
            };
        }

        public static bool HasLicenceUrl(this NuGetPackage item)
        {
            return !string.IsNullOrEmpty(item.LicenceUrl?.AbsoluteUri);
        }

        public static bool HasProjectUrl(this NuGetPackage item)
        {
            return !string.IsNullOrEmpty(item.ProjectUrl?.AbsoluteUri);
        }

        public static string NuGetUrl(this NuGetPackage package)
        {
            return $"https://www.nuget.org/packages/{package.PackageId}/{package.Version}";
        }

        public static string GetLicenceName(this NuGetPackage package)
        {
            if (string.IsNullOrEmpty(package?.LicenceUrl?.AbsoluteUri))
                return null;

            string licenceName = package.LicenceUrl.AbsoluteUri.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last();
            return licenceName;
        }
    }
}