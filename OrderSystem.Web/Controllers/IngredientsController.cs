using System.Linq;
using System.Web.Mvc;
using OrderSystem.Web.Models;

namespace OrderSystem.Web.Controllers
{
  public class IngredientsController : Controller
  {
    private readonly OrdersDbContext _db = new OrdersDbContext();

    // 一覧表示
    public ActionResult Index()
    {
      var ingredients = _db.Ingredients.ToList();
      return View(ingredients);
    }
  }
}
