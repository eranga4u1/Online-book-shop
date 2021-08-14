using System.Web;
using System.Web.Optimization;

namespace Online_book_shop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                      "~/Content/js/jquery-3.5.1.min.js",                     
                      //"~/Scripts/ShopingCartScript.js",
                      //"~/Content/script.js",
                      "~/Scripts/jquery.validate.min.js",
                      //"~/Scripts/DeliveryScript.js",
                      "~/Scripts/bootbox.min.js",
                       "~/Content/js/slick.min.js",
                        "~/Content/js/script.js",
                        "~/Scripts/Reviews.js",
                        "~/Scripts/jquery-ui-1.9.0.js",
                        "~/Scripts/SiteScript.js",
                        "~/Scripts/xzoom.min.js"
                      ));


            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      //"~/Content/fonts/fonts.css",
                      "~/Content/bootstrap.css",
                      "~/Content/css/slick.css",
                      "~/Content/css/grid.css",
                      "~/Content/css/slick-theme.css",
                      "~/Content/css/muses-main.css",
                      "~/Content/muses.css",
                      "~/Content/css/responsive.css"));
           // bundles.Add(new StyleBundle("").Include());

        }
    }
}
