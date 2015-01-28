using System.Web;
using System.Web.Optimization;

namespace SuperMan
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleBundles(bundles);
            RegisterJavascriptBundles(bundles);
        }
        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
            .Include("~/Content/mycss/*.css")
            .Include("~/Content/*.css"));

            bundles.Add(new StyleBundle("~/fileuploadcss").Include("~/Content/jQuery-File-Upload/css/*.css"));
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js")
                            .Include("~/Scripts/bootbox.js")
                            .Include("~/Scripts/bootstrap-timepicker.js")
                            .Include("~/Scripts/jquery.validate.js")
                            .Include("~/Scripts/Extension/*.js")
                            .Include("~/Scripts/jquery.loadmask.js")
                            );

            bundles.Add(new ScriptBundle("~/fileuploadjs").Include("~/Content/jQuery-File-Upload/js/jquery.ui.widget.js")
                .Include("~/Content/jQuery-File-Upload/js/jquery.iframe-transport.js")
                .Include("~/Content/jQuery-File-Upload/js/jquery.fileupload.js"));
        }
        
    }
}
