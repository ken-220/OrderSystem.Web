using System;
using System.Linq;
using System.Web.Mvc;
using OrderSystem.Web.Models;
using OrderSystem.Web.Models.ViewModels;

[Authorize]
public class CashierController : Controller
{
    private readonly OrdersDbContext _db = new OrdersDbContext();

    // ---------------------------
    // レジトップ画面（テーブル一覧）
    // ---------------------------
    public ActionResult Index()
    {
        var tables = _db.Tables.ToList();

        var model = tables.Select(t =>
        {
            var openOrder = _db.Orders
                .Where(o => o.TableId == t.TableId && o.Status == "OPEN")
                .OrderByDescending(o => o.OrderId)
                .FirstOrDefault();

            int itemCount = 0;
            int numPeople = 0;
            int totalAmount = 0;

            if (openOrder != null)
            {
                numPeople = openOrder.NumPeople ?? 0;

                var items = _db.OrderItems
                    .Where(oi => oi.OrderId == openOrder.OrderId)
                    .ToList();

                itemCount = items.Count;

                foreach (var item in items)
                {
                    int basePrice = item.Price * item.Quantity;

                    int optionPrice = _db.OrderItemOptions
                        .Where(o => o.OrderItemId == item.OrderItemId)
                        .Select(o => (int?)o.Option.ExtraPrice)
                        .Sum() ?? 0;

                    totalAmount += basePrice + optionPrice;
                }
            }

            return new CashierTableViewModel
            {
                TableId = t.TableId,
                TableName = t.TableName,
                HasOpenOrder = openOrder != null,
                TotalAmount = totalAmount,
                OrderItemCount = itemCount,
                NumPeople = numPeople
            };

        }).ToList();

        return View(model);
    }


    // ---------------------------
    // 会計画面
    // ---------------------------
    public ActionResult Settle(int tableId)
{
    var order = _db.Orders
        .FirstOrDefault(o => o.TableId == tableId && o.Status == "OPEN");

    if (order == null)
        return RedirectToAction("Index");

    var items = _db.OrderItems
       .Where(oi => oi.OrderId == order.OrderId)
       .Select(oi => new CashierItemViewModel
       {
           MenuName = oi.MenuItem.Name,
           Quantity = oi.Quantity,
           UnitPrice = oi.Price,

           // ★ ここが重要：Sum の後で null → 0
           OptionPrice = oi.OrderItemOptions
                            .Select(opt => (int?)opt.Option.ExtraPrice)
                            .Sum() ?? 0,

           Subtotal = (oi.Price * oi.Quantity)
                      + (oi.OrderItemOptions
                            .Select(opt => (int?)opt.Option.ExtraPrice)
                            .Sum() ?? 0)
       })
       .ToList();


    int total = items.Sum(x => x.Subtotal);

    var vm = new CashierSettleViewModel
    {
        TableId = tableId,
        TableName = order.Table.TableName,
        OrderId = order.OrderId,
        Items = items,
        TotalAmount = total
    };

    return View(vm);
}



    // ---------------------------
    // 会計確定（POST）
    // ---------------------------
    [HttpPost]
    public ActionResult SettleConfirm(int orderId, string paymentMethod)
    {
        var order = _db.Orders.Find(orderId);
        if (order == null) return HttpNotFound();

        int total = 0;

        var items = _db.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .ToList();

        foreach (var item in items)
        {
            int basePrice = item.Price * item.Quantity;

            int optionPrice = _db.OrderItemOptions
                .Where(o => o.OrderItemId == item.OrderItemId)
                .Select(o => (int?)o.Option.ExtraPrice)
                .Sum() ?? 0;

            total += basePrice + optionPrice;
        }

        // 会計処理
        order.Status = "PAID";
        order.PaidAt = DateTime.Now;
        order.PaymentMethod = paymentMethod;
        order.PaidAmount = total;

        _db.SaveChanges();

        return RedirectToAction("Index");
    }
}
