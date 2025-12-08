using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using OrderSystem.Web.Models;

namespace OrderSystem.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly OrdersDbContext _db = new OrdersDbContext();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml");
        }


        [HttpPost, AllowAnonymous, /*ValidateAntiForgeryToken*/]
        public ActionResult Login(string name, string password)
        {
            // 今は平文で比較
            var user = _db.Users.FirstOrDefault(u => u.Name == name && u.Password == password);


            if (user == null)
            {
                ModelState.AddModelError("", "ユーザー名またはパスワードが正しくありません。");
                return View();
            }

            // ログイン
            FormsAuthentication.SetAuthCookie(user.Name, false);

            // ロールを Session に記録（Kitchen 画面の判断に必要）
            Session["Role"] = user.Role;

          

            // 選択画面へ
            return RedirectToAction("Menu", "Home");

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
