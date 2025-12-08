using System.Linq;
using System.Web.Mvc;
using OrderSystem.Web.Models;
using OrderSystem.Web.Models.ViewModels;


namespace OrderSystem.Web.Controllers
{
    [Authorize]
    public class TablesController : Controller
    {
        private readonly OrdersDbContext _db = new OrdersDbContext();

        public ActionResult Index()
        {
            var tables = _db.Tables.ToList();

            var model = tables.Select(t =>
            {
                // テーブルの最新オーダー（OPEN のみ）
                var currentOrder = _db.Orders
                    .Where(o => o.TableId == t.TableId && o.Status == "OPEN")
                    .OrderByDescending(o => o.OrderId)
                    .FirstOrDefault();

                int totalAmount = 0;

                if (currentOrder != null)
                {
                    // --- ① OrderItem の本体価格 ---
                    var baseTotal = _db.OrderItems
                        .Where(oi => oi.OrderId == currentOrder.OrderId)
                        .Sum(oi => oi.Price * oi.Quantity);

                    // --- ② OrderItemOptions の追加料金 ---
                    var optionTotal =
                        (from oio in _db.OrderItemOptions
                         join opt in _db.Options on oio.OptionId equals opt.OptionId
                         join oi in _db.OrderItems on oio.OrderItemId equals oi.OrderItemId
                         where oi.OrderId == currentOrder.OrderId
                         select opt.ExtraPrice * oi.Quantity)
                        .DefaultIfEmpty(0)
                        .Sum();

                    totalAmount = baseTotal + optionTotal;
                }

                return new TableCardViewModel
                {
                    TableId = t.TableId,
                    TableName = t.TableName,
                    CurrentOrder = currentOrder,
                    TotalAmount = totalAmount
                };
            })
      .ToList();


            return View(model);
        }
    }
}

