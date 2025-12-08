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
            // ① 全テーブル取得
            var tables = _db.Tables.ToList();

            // ② テーブルID一覧
            var tableIds = tables.Select(t => t.TableId).ToList();

            // ③ OPEN の全注文をまとめて取得（SQL 1回）
            var openOrders = _db.Orders
                .Where(o => tableIds.Contains(o.TableId) && o.Status == "OPEN")
                .ToList();

            // ④ OPEN注文の OrderId を取得
            var orderIds = openOrders.Select(o => o.OrderId).ToList();

            // ⑤ OrderItems（料理本体）を一括取得
            var allItems = _db.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderId))
                .ToList();

            // ⑥ Options（追加料金）も一括取得
            var allOptions =
                (from oio in _db.OrderItemOptions
                 join opt in _db.Options on oio.OptionId equals opt.OptionId
                 join oi in _db.OrderItems on oio.OrderItemId equals oi.OrderItemId
                 where orderIds.Contains(oi.OrderId)
                 select new
                 {
                     oi.OrderId,
                     oi.Quantity,
                     opt.ExtraPrice
                 }).ToList();

            // ⑦ テーブルごとに ViewModel を生成
            var model = tables.Select(t =>
            {
                // このテーブルの OPEN 注文だけ抽出
                var ordersOfTable = openOrders
                    .Where(o => o.TableId == t.TableId)
                    .ToList();

                int totalAmount = 0;

                if (ordersOfTable.Any())
                {
                    var tOrderIds = ordersOfTable.Select(o => o.OrderId).ToList();

                    // 料理本体の合計
                    var baseTotal = allItems
                        .Where(x => tOrderIds.Contains(x.OrderId))
                        .Sum(x => x.Price * x.Quantity);

                    // オプションの合計
                    var optionTotal = allOptions
                        .Where(x => tOrderIds.Contains(x.OrderId))
                        .Sum(x => x.ExtraPrice * x.Quantity);

                    totalAmount = baseTotal + optionTotal;
                }

                return new TableCardViewModel
                {
                    TableId = t.TableId,
                    TableName = t.TableName,
                    TotalAmount = totalAmount,

                    // 最新の注文（OrderId最大）
                    CurrentOrder = ordersOfTable
                        .OrderByDescending(o => o.OrderId)
                        .FirstOrDefault()
                };
            }).ToList();

            return View(model);
        }


    }
}