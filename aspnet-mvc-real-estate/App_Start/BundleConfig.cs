using System.Web;
using System.Web.Optimization;

namespace aspnet_mvc_real_estate
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

            bundles.Add(new ScriptBundle("~/Content/javascript").Include(
                      "~/Content/Assets/Js/app.js",
                      "~/Content/Ckeditor/ckeditor.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/Fontawesome/css/all.min.css"));

            bundles.Add(new ScriptBundle("~/Content/admin/script").Include(
                      "~/Content/Admin/js/jquery.min.jss",
                      "~/Content/Admin/js/bootstrap.min.js",
                      "~/Content/Admin/js/metisMenu.min.js",
                      "~/Content/Admin/js/raphael.min.js",
                      "~/Content/Admin/js/morris.min.js",
                      "~/Content/Admin/js/morris-data.js",
                      "~/Content/Admin/js/startmin.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/admin/style").Include(
                      "~/Content/Admin/css/bootstrap.min.css",
                      "~/Content/Admin/css/metisMenu.min.css\"",
                      "~/Content/Admin/css/timeline.css",
                      "~/Content/Admin/css/startmin.css",
                      "~/Content/Admin/css/morris.css",
                      "~/Content/Fontawesome/css/all.min.css"));

            bundles.Add(new StyleBundle("~/Content/style/css").Include(
                      "~/Content/Assets/Css/style.css",
                      "~/Content/Assets/Css/admin.css"));
        }
    }
}
