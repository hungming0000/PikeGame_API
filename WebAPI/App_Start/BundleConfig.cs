using System.Web;
using System.Web.Optimization;

namespace WebAPI
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            #region 林政三期所需

            bundles.Add(new StyleBundle("~/assets/css").Include(
             "~/assets/css/bootstrap.min.css",
             "~/assets/font-awesome/4.5.0/css/font-awesome.min.css",
             "~/assets/css/fonts.googleapis.com.css",
             "~/assets/css/ace.min.css",
             "~/assets/css/ace-skins.min.css",
             "~/assets/css/ace-rtl.min.css"           
             ));

            bundles.Add(new StyleBundle("~/assets/js").Include(            
             "~/assets/js/ace-extra.min.js"             
             ));




            #endregion 林政三期所需


        }
    }
}
