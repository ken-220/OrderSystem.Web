using System.Web;
using System.Web.Optimization; 
// ← 「最適化（Optimization）」機能を使うための名前空間
//    JavaScript や CSS をまとめたり（bundling）
//    圧縮したり（minify）する機能が入っている

namespace OrderSystem.Web
{
    // ★ JavaScript と CSS のバンドル（まとめる設定）を書くクラス
    public class BundleConfig
    {
        // MVC アプリの起動時（Application_Start）に呼ばれるメソッド
        // BundleCollection = バンドルの一覧
        public static void RegisterBundles(BundleCollection bundles)
        {
            // =============================
            // 1. jQuery のバンドル
            // =============================
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            // "~/bundles/jquery" という名前で jQuery をまとめる
            // {version} と書くと jquery-3.7.1.js のように “最新バージョン” を自動で読み込む

            // =============================
            // 2. jQuery Validation (入力チェック用)
            // =============================
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            // jquery.validate.js / jquery.validate.unobtrusive.js など
            // 「validate」で始まるすべてのスクリプトをまとめて読み込む

            // =============================
            // 3. modernizr（古いブラウザ対応のチェック）
            // =============================
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            // modernizr の *（ワイルドカード）ですべての modernizr 関連ファイルをまとめる

            // =============================
            // 4. bootstrap（デザインCSS + 便利JS）
            // =============================
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            // Bootstrap 本体（bootstrap.js）と
            // 古いブラウザ対応の respond.js を一緒に読み込む

            // =============================
            // 5. CSS のバンドル（スタイルまとめ）
            // =============================
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            // bootstrap の CSS と、プロジェクト独自の site.css をまとめる
        }
    }
}




