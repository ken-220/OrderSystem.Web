using Microsoft.Owin;
// ← OWIN（Web サーバーとアプリをつなぐ仕組み）の基本クラスが入っている

using Owin;
// ← Startup クラスや、IAppBuilder などを使うための名前空間

// ------------------------------------------------------------
// このアプリの Startup クラスを OWIN のスタートアップとして
// 使用することを指定する“おまじない”。
// 
// OWIN は Web サーバー（IIS など）とアプリの接続部分で働くフレームワーク。
// SignalR 2 を動かすには必須の設定。
// ------------------------------------------------------------
[assembly: OwinStartup(typeof(OrderSystem.Web.Startup))]

namespace OrderSystem.Web
{
    // ★ Startup クラス：アプリ起動時に呼ばれる初期化クラス
    public class Startup
    {
        // ★ Configuration メソッド：起動時に OWIN によって実行される
        public void Configuration(IAppBuilder app)
        {
            // --------------------------------------------------------
            // SignalR を有効化する
            // --------------------------------------------------------
            // app.MapSignalR() を呼ぶことで、
            // /signalr/hubs という特別なエンドポイントが自動作成される。
            //
            // この "/signalr" の URL を通じて、
            // ブラウザ（JavaScript）とサーバー（C#）が
            // 双方向リアルタイム通信をできるようになる。
            //
            // 厨房画面（Kitchen）はこの機能を使い、
            // “新しい注文が入った瞬間” に自動で画面を更新できる。
            // --------------------------------------------------------
            app.MapSignalR();
        }
    }
}
