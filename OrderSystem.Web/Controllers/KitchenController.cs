using OrderSystem.Web.Models;
using OrderSystem.Web.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace OrderSystem.Web.Controllers
{
    [Authorize]
    public class KitchenController : Controller
    {
        private readonly OrdersDbContext _db = new OrdersDbContext();

        public ActionResult Index()
        {
            var role = Session["Role"]?.ToString();

            if (role == null || (role != "Kitchen" && role != "Admin"))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // =========================
        // 厨房伝票印刷
        // URL: /Kitchen/Print?tableId=1
        // =========================
        public ActionResult Print(int tableId)
        {
            // 1) 対象テーブルの「最新の OPEN 注文」を取得
            var order = _db.Orders
                .Where(o => o.TableId == tableId && o.Status == "OPEN")
                .OrderByDescending(o => o.OrderId)
                .FirstOrDefault();

            if (order == null)
            {
                // OPEN な注文が無い場合は 404 にしておく
                return HttpNotFound("このテーブルにはオープン中の注文がありません。");
            }

            // 2) テーブル名を取得
            var tableName = _db.Tables
                .Where(t => t.TableId == tableId)
                .Select(t => t.TableName)
                .FirstOrDefault() ?? $"T-{tableId:00}";

            // 3) 明細 + オプションを ViewModel に詰める
            var items = _db.OrderItems
                .Where(oi => oi.OrderId == order.OrderId)
                .Select(oi => new KitchenPrintItem
                {
                    // MenuItems からメニュー名を取得（API と同じ書き方）
                    MenuName = _db.MenuItems
                        .Where(m => m.MenuId == oi.MenuId)
                        .Select(m => m.Name)
                        .FirstOrDefault(),
                    Quantity = oi.Quantity,

                    Options = _db.OrderItemOptions
                        .Where(oio => oio.OrderItemId == oi.OrderItemId)
                        .Select(oio => new KitchenPrintOption
                        {
                            OptionName = _db.Options
                                .Where(opt => opt.OptionId == oio.OptionId)
                                .Select(opt => opt.OptionName)
                                .FirstOrDefault(),
                            ExtraPrice = _db.Options
                                .Where(opt => opt.OptionId == oio.OptionId)
                                .Select(opt => opt.ExtraPrice)
                                .FirstOrDefault()
                        })
                        .ToList()
                })
                .ToList();

            var vm = new KitchenPrintViewModel
            {
                TableName = tableName,
                OrderTime = order.OrderTime,
                Items = items
            };

            // Views/Kitchen/Print.cshtml を KitchenPrintViewModel で描画
            return View("Print", vm);
        }
    }
}
