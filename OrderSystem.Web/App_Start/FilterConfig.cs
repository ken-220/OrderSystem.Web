using System.Web;
// ← ASP.NET（System.Web）の基本機能を使うための名前空間

using System.Web.Mvc;
// ← MVC の機能（Controller / Filter / Attributes）を使うための名前空間

namespace OrderSystem.Web
{
    // ★ フィルター（Filter）の設定を登録するクラス
    // フィルターとは「全画面に共通で適用される処理」
    public class FilterConfig
    {
        // ★ MVC アプリ起動時（Application_Start）に呼び出される
        // GlobalFilterCollection = グローバル（全体）に適用されるフィルターのリスト
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // -----------------------------------------
            // 1. HandleErrorAttribute とは？
            // -----------------------------------------
            // これは MVC の「共通エラーハンドラー」。
            // 画面で例外（エラー）が発生したとき：
            // ・勝手に Error.cshtml（Views/Shared/Error.cshtml）へ移動する
            // ・エラーの詳細なメッセージ（例外内容）は隠してくれる（セキュリティ）
            //
            // ★ 全てのコントローラ/アクションに適用される
            //
            // 入れておくのが MVC の基本形。

            filters.Add(new HandleErrorAttribute());
            // ↑ これ1行で「全アクション共通のエラーページ処理」が有効になる
        }
    }
}
