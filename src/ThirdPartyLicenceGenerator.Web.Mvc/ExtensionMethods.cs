using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Web.Mvc
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

        public static bool IsAbsoluteUrl(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.IsLocalPath())
                return false;

            return Uri.TryCreate(value, UriKind.Absolute, out _);
        }

        public static bool IsLocalPath(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.Contains("\\");
        }

        public static string ToSubPaths(this string value, int take)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            List<string> paths = null;

            if (value.IsLocalPath())
            {
                paths = value.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else if (value.IsAbsoluteUrl())
            {
                paths = value.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if (paths == null)
                return value;

            if (take > paths.Count)
                take = paths.Count;

            var subpaths = paths.Skip(paths.Count - take).Take(take);

            return string.Join("/", subpaths);
        }

        public static string ToMarkdownHeading(this IPackage package)
        {
            string projectUrl =
                package.ProjectPath.IsAbsoluteUrl() ? package.ProjectPath :
                package.SourcePath.IsAbsoluteUrl() ? package.SourcePath : null;

            string heading = null;

            if(!string.IsNullOrEmpty(projectUrl))
            {
                heading = $"### [{package.PackageId}]({projectUrl})";
            }
            else
            {
                heading = $"### {package.PackageId}";
            }

            string licenceText =
                package.LicencePath.IsAbsoluteUrl() ?
                    $"([{package.LicencePath.ToSubPaths(1)}]({package.LicencePath}))" :
                    $"({package.LicencePath.ToSubPaths(1)})";

            heading = $"{heading} {licenceText}";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(heading);
            sb.AppendLine("Here goes the License text");

            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}