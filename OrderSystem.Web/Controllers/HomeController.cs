using System.Web.Mvc;

namespace OrderSystem.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // 🔵 ログイン後のメニュー画面
        public ActionResult Menu()
        {
            // ログイン時にセットした Role を取得
            var role = Session["Role"] as string;

            if (string.IsNullOrEmpty(role))
            {
                role = "Unknown";
            }

            ViewBag.Role = role;
            return View();
        }

    }
}
