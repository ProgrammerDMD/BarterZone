using System.Web.Optimization;

namespace Project.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/css/styles.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/node_modules/preline/dist/preline.js",
                "~/node_modules/nouislider/dist/nouislider.min.js",
                "~/node_modules/lodash/lodash.min.js"));
        }
    }
}