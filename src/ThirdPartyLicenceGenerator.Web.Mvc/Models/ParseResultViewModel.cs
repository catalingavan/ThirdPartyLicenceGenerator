using System.Collections.Generic;
using ThirdPartyLicenceGenerator.Models;

namespace ThirdPartyLicenceGenerator.Web.Mvc.Models
{
    public class ParseResultViewModel
    {
        public IEnumerable<PackagesFileResult<IPackage>> Files { get; set; }

        public ParseResultViewModel()
        {
            Files = new List<PackagesFileResult<IPackage>>();
        }
    }
}