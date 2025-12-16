using OrderSystem.Web.Models;
using System;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using System.Web.Http;          // Web API（/api/...）を使うため
using System.Web.Mvc;           // MVC（Controller / View）を使うため
using System.Web.Optimization;  // CSS / JS の Bundle（圧縮/結合）機能
using System.Web.Routing;       // ルーティング（URLとアクションを結びつける）
using System.Web.Security;



namespace OrderSystem.Web
{
    // ASP.NET アプリ全体を表すクラス
    public class MvcApplication : System.Web.HttpApplication
    {
        // アプリ起動時に1回だけ呼ばれるメソッド
        protected void Application_Start()
        {
            Database.SetInitializer<OrdersDbContext>(null);

            // ① Areas をすべて登録（Admin/Apiなど階層化している場合に必要）
            AreaRegistration.RegisterAllAreas();

            // ② Web API のルーティングなどを登録
            //    /api/orders などの API が使えるようになる
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // ③ MVC のグローバルフィルターを設定（例：全画面にログイン必須など）
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // ④ MVC の URL ルーティングを設定
            //    "/" → Tables/Index などのルート設定
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // ⑤ CSS / JS をまとめて圧縮する仕組み（Bundle）を登録
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
      if (HttpContext.Current.User != null &&
          HttpContext.Current.User.Identity.IsAuthenticated)
      {
        var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie == null) return;

        var ticket = FormsAuthentication.Decrypt(authCookie.Value);
        if (ticket == null) return;

        var roles = ticket.UserData.Split(',');

        Context.User = new GenericPrincipal(
            Context.User.Identity,
            roles
        );
      }
    }

  }
}
