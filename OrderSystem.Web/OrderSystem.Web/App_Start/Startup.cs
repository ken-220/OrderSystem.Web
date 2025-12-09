using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OrderSystem.Web.Startup))]

namespace OrderSystem.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // SignalR を有効化
            app.MapSignalR();
        }
    }
}

