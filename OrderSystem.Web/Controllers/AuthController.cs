using OrderSystem.Web.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
      // 認証チケットを作成（Role を含める）
      var ticket = new FormsAuthenticationTicket(
          1,
          user.Name,                  // ユーザー名
          DateTime.Now,
          DateTime.Now.AddMinutes(60),
          false,
          user.Role                   // ★ Admin / Kitchen / Hall
      );

      // 暗号化
      string encryptedTicket = FormsAuthentication.Encrypt(ticket);

      // Cookie に詰める
      var authCookie = new HttpCookie(
          FormsAuthentication.FormsCookieName,
          encryptedTicket
      );

      Response.Cookies.Add(authCookie);


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
