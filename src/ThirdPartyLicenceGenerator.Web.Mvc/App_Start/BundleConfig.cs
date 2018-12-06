using System.Web.Optimization;

namespace ThirdPartyLicenceGenerator.Web.Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Content/js/jquery-{version}.min.js")
                .Include("~/Content/js/jquery.validate*")
            );

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/site.css"));
        }
    }
}
