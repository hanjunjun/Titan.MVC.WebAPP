using System.Web;
using System.Web.Optimization;

namespace Titan.WebMVC
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //样式
            bundles.Add(new StyleBundle("~/css/google_fonts").Include(
                "~/css/google_fonts/googleFonts.css",
                "~/css/google_fonts/googleIcon.css"));//谷歌字体

            bundles.Add(new StyleBundle("~/commoncss").Include(
                "~/plugins/bootstrap/css/bootstrap.css",
                "~/plugins/node-waves/waves.css",
                "~/plugins/animate-css/animate.css",
                "~/plugins/morrisjs/morris.css",
                "~/css/style.css",
                "~/css/themes/all-themes.css",
                "~/plugins/sweetalert/sweetalert.css",
                "~/css/MyCommon.css",
                "~/plugins/bootstrap-select/css/bootstrap-select.css"));//公共样式

            //js脚本
            bundles.Add(new ScriptBundle("~/Script/jqUI").Include(
                      "~/Scripts/jquery-1.10.2.min.js",
                      "~/plugins/jquery-validation/jquery.validate.js",
                "~/plugins/jquery-validation/localization/messages_zh.js",
                "~/plugins/jquery-validation/additional-methods.js"));
            bundles.Add(new ScriptBundle("~/Script/CoreJs").Include(
                "~/plugins/jquery/jquery.min.js",
                "~/plugins/bootstrap/js/bootstrap.js",
                "~/plugins/bootstrap-select/js/bootstrap-select.js",
                "~/plugins/bootstrap-select/js/i18n/defaults-zh_CN.min.js",//bootstarp-select 中文包
                "~/plugins/jquery-slimscroll/jquery.slimscroll.js",
                "~/plugins/node-waves/waves.js",
                "~/plugins/jquery-countto/jquery.countTo.js",
                "~/plugins/raphael/raphael.min.js",
                "~/plugins/morrisjs/morris.js",
                "~/plugins/chartjs/Chart.bundle.js",
                "~/plugins/flot-charts/jquery.flot.js",
                "~/plugins/flot-charts/jquery.flot.resize.js",
                "~/plugins/flot-charts/jquery.flot.pie.js",
                "~/plugins/flot-charts/jquery.flot.categories.js",
                "~/plugins/flot-charts/jquery.flot.time.js",
                "~/plugins/jquery-sparkline/jquery.sparkline.js",
                "~/js/admin.js",
                "~/js/demo.js",
                "~/plugins/sweetalert/sweetalert.min.js"));//swal弹窗

            //统一验证脚本
            bundles.Add(new ScriptBundle("~/Script/Validate").Include(
                      "~/plugins/jquery-validation/jquery.validate.js",
                      "~/plugins/jquery-validation/localization/messages_zh.js",
                      "~/plugins/jquery-validation/additional-methods.js"));
        }
    }
}
