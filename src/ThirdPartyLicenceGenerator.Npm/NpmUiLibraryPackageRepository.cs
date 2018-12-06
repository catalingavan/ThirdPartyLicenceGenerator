using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using ThirdPartyLicenceGenerator.Models;
using ThirdPartyLicenceGenerator.Repositories;

namespace ThirdPartyLicenceGenerator.Npm
{
    public class NpmUiLibraryPackageRepository : IUiLibraryPackageRepository
    {
        private async Task<NpmPackage> GetNpmPackage(string packageId)
        {
            Uri uri = new Uri($"http://registry.npmjs.org/{HttpUtility.UrlEncode(packageId)}/latest");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<NpmPackage>(responseString);
                    }
                }
            }

            return null;
        }

        public async Task<UiLibraryPackage> FindPackageAsync(string packageId)
        {
            UiLibraryPackage result = new UiLibraryPackage
            {
                WasFound = false,
                PackageId = packageId
            };

            NpmPackage npmPackage = await GetNpmPackage(packageId);
            if (npmPackage != null)
            {
                result.WasFound = true;
                result.ProjectPath = npmPackage.Homepage;
                result.LicencePath = npmPackage.License;
                result.SourcePath = $"https://www.npmjs.com/package/{HttpUtility.UrlEncode(packageId)}";
            }

            return result;
        }
    }
}
