using System.Web.Optimization;

namespace Project
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/css/styles.css", new CssRewriteUrlTransform()));
        }
    }
}