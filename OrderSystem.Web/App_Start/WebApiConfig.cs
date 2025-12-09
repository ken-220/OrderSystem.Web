using System.Net.Http.Headers;
// ← HTTP のヘッダー（Content-Type: application/json など）を扱うクラスが入っている

using System.Web.Http;
// ← Web API 2 のコントローラー / ルート設定 / JSON設定 等が入っている

namespace OrderSystem.Web
{
    // ★ Web API（/api/...）のルーティング & 出力フォーマット設定を行うクラス
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // ---------------------------------------------------
            // ① 属性ルーティングを有効化
            // ---------------------------------------------------
            // [Route("api/test")] のようにコントローラ側に書いたルートを有効化する。
            //
            // 例：
            // [Route("api/orders/latest")]
            // public IHttpActionResult GetLatest() { ... }
            //
            // これが使えるようになる。
            config.MapHttpAttributeRoutes();

            // ---------------------------------------------------
            // ② 通常のルート設定（従来のMVCのような書き方）
            // ---------------------------------------------------
            // /api/{controller}/{id}
            //
            // 例：
            // /api/orders            → OrdersController
            // /api/orders/10         → OrdersController.Get(id=10)
            //
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // ---------------------------------------------------
            // ③ ★ XML 出力を無効化する
            // ---------------------------------------------------
            // Web API はデフォルトで「XML と JSON の両方を返す」。
            // ブラウザやアプリが「XMLください」と言うと XML を返してしまう。
            //
            // 今回の飲食店システムでは
            //   - JavaScript (jQuery, Fetch)
            //   - Android/iOS
            //   - Kitchen の Ajax
            // 全部 JSON を使うので、XML は不要。
            //
            // XMLFormatter の対応メディアタイプを空にして無効化。
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // ---------------------------------------------------
            // ④ ★ JSON をデフォルトの返却形式にする
            // ---------------------------------------------------
            // 「application/json」を正式にサポート媒体として追加。
            // ブラウザやアプリはこの設定により JSON を受け取ることができる。
            //
            // 例：API を叩くと必ず JSON になる
            // { "orderId": 1, "tableId": 3, ... }
            //
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("application/json"));
        }
    }
}
