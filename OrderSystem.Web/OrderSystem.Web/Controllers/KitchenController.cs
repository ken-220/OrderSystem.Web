using System.Web.Mvc;

public class KitchenController : Controller
{
    public ActionResult Index()
    {
        // セッションに記録した Role をチェック
        var role = Session["Role"]?.ToString();

        if (role == null || (role != "Kitchen" && role != "Admin"))
        {
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }
}
