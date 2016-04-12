using System.Web.Optimization;

namespace smartData
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/libs/jquery-{version}.js"
                        "~/Scripts/JqGrid/jquery-1.11.0.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libs/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/commonjs").Include(
                        "~/Scripts/js/menu.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/libs/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/libs/bootstrap.min.js",
                      "~/Scripts/libs/bootstrap-datepicker.js",
                      "~/Scripts/libs/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include(
                      "~/Scripts/libs/jquery.unobtrusive-ajax.min.js",
                      "~/Scripts/libs/jquery.validate.min.js",
                      "~/Scripts/libs/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendojs").Include(
                      //"~/Areas/Scheduler/Content/kendojs/jquery.min.js",
                      //"~/Scripts/JqGrid/jquery-1.11.0.min.js",
                      "~/Areas/Scheduler/Content/kendojs/kendo.all.min.js",
                      "~/Areas/Scheduler/Content/kendojs/kendo.timezones.min.js",
                      "~/Scripts/libs/jquery.contextMenu.js"));

            bundles.Add(new ScriptBundle("~/bundles/areaScan").Include(
                      "~/Areas/Scan/Resources/dynamsoft.webtwain.initiate.js",
                      "~/Areas/Scan/Resources/dynamsoft.webtwain.config.js",
                      "~/Areas/Scan/js/online_demo_operation.js",
                      "~/Areas/Scan/js/online_demo_initpage.js"
                      //"~/Areas/Scan/js/jquery.js",
                      //"~/Scripts/JqGrid/jquery-1.11.0.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapgrid").Include(
                      //"~/Scripts/JqGrid/jquery-1.11.0.min.js",
                      "~/Scripts/libs/bootstrap.min.js",
                      "~/Scripts/BootStrapGrid/src/bootstrap-table.js",
                      "~/Scripts/BootStrapDailog/bootstrap-dialog.min.js",
                      "~/Scripts/BootStrapGrid/src/extensions/filter/bootstrap-table-filter.js"));

            bundles.Add(new StyleBundle("~/bundles/kendoStyle").Include(
                      "~/Areas/Scheduler/Content/kendoStyle/kendo.common.min.css",
                      "~/Areas/Scheduler/Content/kendoStyle/kendo.dataviz.default.min.css",
                      "~/Areas/Scheduler/Content/kendoStyle/kendo.dataviz.min.css",
                      "~/Areas/Scheduler/Content/kendoStyle/kendo.default.min.css",
                      "~/Areas/Scheduler/Content/kendoStyle/kendo.default.mobile.min.css"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/datepicker.css",
                      "~/Content/site.css",
                      "~/Content/jquery.contextMenu.css",
                      "~/Content/common.css"));

            bundles.Add(new StyleBundle("~/bundles/common").Include(
                      "~/Content/common.css"));


            bundles.Add(new StyleBundle("~/bundles/bootstrapDatepicker").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/datepicker.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrapCss").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Scripts/BootStrapGrid/src/bootstrap-table.css",
                     "~/Content/site.css",
                     "~/Content/jquery.contextMenu.css",
                     "~/Scripts/BootStrapDailog/bootstrap-dialog.min.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrapCGridDailog").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Scripts/BootStrapGrid/src/bootstrap-table.css",
                     "~/Scripts/BootStrapDailog/bootstrap-dialog.min.css"));
            
        }
    }
}
