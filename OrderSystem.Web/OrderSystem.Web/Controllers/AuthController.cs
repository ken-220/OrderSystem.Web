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
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(string name, string password)
        {
            // 今は平文で比較
            var user = _db.Users.FirstOrDefault(u => u.Name == name && u.PasswordHash == password);

            if (user == null)
            {
                ModelState.AddModelError("", "ユーザー名またはパスワードが正しくありません。");
                return View();
            }

            // ログイン
            FormsAuthentication.SetAuthCookie(user.Name, false);

            // ロールを Session に記録（Kitchen 画面の判断に必要）
            Session["Role"] = user.Role;

            // ロール別のリダイレクト
            if (user.Role == "Kitchen")
                return RedirectToAction("Index", "Kitchen");

            if (user.Role == "Admin")
                return RedirectToAction("Index", "Kitchen");

            // Hall（ホールスタッフ）などはテーブル画面へ
            return RedirectToAction("Index", "Tables");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
