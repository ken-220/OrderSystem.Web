using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// ← ASP.NET（System.Web）の基本処理

using System.Web.Mvc;
// ← MVC の機能（Controller, Actionなど）

using System.Web.Routing;
// ← URL をどの Controller/Action に結び付けるか（ルーティング）の機能

namespace OrderSystem.Web
{
    // ★ ルーティング（URL ルール）を登録するクラス
    public class RouteConfig
    {
        // ★ アプリ起動時（Application_Start）に呼ばれる
        public static void RegisterRoutes(RouteCollection routes)
        {
            // -----------------------------------------------------------
            // ① 静的ファイル（.axd）へのアクセスを無視する設定
            // -----------------------------------------------------------
            // WebResource.axd や ScriptResource.axd などのリソースは
            // MVC ルートに入れず直接配信するため、ルーティングから除外します。
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // -----------------------------------------------------------
            // ② ルート（URL → Controller/Action）を登録する
            // -----------------------------------------------------------
            // routes.MapRoute( ... ) で 1つのルールを定義する。

            routes.MapRoute(
                name: "Default",    // ルート名（なんでもOK）

                url: "{controller}/{action}/{id}",
                // URL の基本形
                // 例： /Auth/Login
                //      /Tables/Index
                //      /Orders/New/5  ← id=5

                defaults: new
                {
                    controller = "Auth",   // デフォルトのコントローラー
                    action = "Login",      // デフォルトのアクション
                    id = UrlParameter.Optional  // id は省略可能（/だけでもOK）
                }
            );
            // ------------------------------------------------------------
            // ★ この設定の意味
            // ------------------------------------------------------------
            // ブラウザが "/"（ホーム）にアクセスしたら
            // → AuthController の Login Action を開く
            //
            // つまり、このような動作になる：
            //   /                 → Auth/Login
            //   /Auth/Login       → Auth/Login
            //   /Tables/Index     → TablesController.Index
            //   /Kitchen/Index    → KitchenController.Index
            //
            // これが MVC の「URL と画面の紐づけ」になる。
        }
    }
}
