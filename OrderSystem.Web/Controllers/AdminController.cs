using System.Web.Mvc;

namespace OrderSystem.Web.Controllers
{
  [Authorize(Roles = "Admin")]   //Admin ロールを持つユーザーだけアクセス可能
  public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}