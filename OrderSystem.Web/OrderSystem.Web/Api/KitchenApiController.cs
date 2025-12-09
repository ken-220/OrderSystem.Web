using OrderSystem.Web.Hubs;
using OrderSystem.Web.Models;
using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Http;

namespace OrderSystem.Web.Api
{
    [Authorize]
    [RoutePrefix("api/kitchen")]
    public class KitchenApiController : ApiController
    {
        private readonly OrdersDbContext _db = new OrdersDbContext();

        // ============================
        // ① テーブル単位のオーダー一覧
        // ============================
        // /api/kitchen/table-orders
        [HttpGet, Route("table-orders")]
        public IHttpActionResult GetTableOrders()
        {
            // 全テーブルをベースに「空席でも必ず 1 カード」になるようにする
            var result = _db.Tables
                .Select(t => new
                {
                    t.TableId,
                    t.TableName,

                    // このテーブルで「OPEN な注文」の全明細を集約
                    Items = _db.OrderItems
                        .Where(oi =>
                            oi.Order.TableId == t.TableId &&
                            oi.Order.Status == "OPEN"        // 会計前だけ表示
                        )
                        .Select(oi => new
                        {
                            oi.OrderItemId,
                            oi.OrderId,
                            oi.MenuId,
                            MenuName = _db.MenuItems
                                .Where(m => m.MenuId == oi.MenuId)
                                .Select(m => m.Name)
                                .FirstOrDefault(),
                            oi.Quantity,
                            KitchenStatus = oi.KitchenStatus,   // "PENDING" / "COOKING" / "DONE" / "SERVED" など
                            oi.StatusUpdatedAt,

                            ElapsedMinutes =
                                (int)SqlFunctions.DateDiff("minute", oi.Order.OrderTime, DateTime.Now),
                        


                            //オプション一覧
                    Options = _db.OrderItemOptions
                        .Where(oio => oio.OrderItemId == oi.OrderItemId)
                        .Select(oio => new
                        {
                            oio.OptionId,
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
                        .OrderBy(x => x.OrderId)
                        .ThenBy(x => x.OrderItemId)
                        .ToList()
                })


                .OrderBy(t => t.TableId)
                .ToList()
                .Select(t => new {
                    t.TableId,
                    t.TableName,
                    t.Items,

                    // ← テーブル単位の経過時間（最古の注文を採用）
                    ElapsedMinutes = t.Items.Any()
                        ? t.Items.Min(i => i.ElapsedMinutes)
                        : 0
                });


            return Ok(result);
        }

        // ============================
        // ② 調理開始（COOKING）
        // ============================
        [HttpPost, Route("start/{orderItemId}")]
        public IHttpActionResult StartCooking(int orderItemId)
        {
            var item = _db.OrderItems.Find(orderItemId);
            if (item == null) return NotFound();

            item.KitchenStatus = "COOKING";
            item.StatusUpdatedAt = DateTime.Now;
            _db.SaveChanges();

            KitchenHub.NotifyStatusChanged(orderItemId, "COOKING");
            return Ok();
        }

        // ============================
        // ③ 調理完了（DONE）
        // ============================
        [HttpPost, Route("done/{orderItemId}")]
        public IHttpActionResult FinishCooking(int orderItemId)
        {
            var item = _db.OrderItems.Find(orderItemId);
            if (item == null) return NotFound();

            item.KitchenStatus = "DONE";
            item.StatusUpdatedAt = DateTime.Now;
            _db.SaveChanges();

            KitchenHub.NotifyStatusChanged(orderItemId, "DONE");
            return Ok();
        }

        // ============================
        // ④ 提供済み（SERVED）
        // ============================
        [HttpPost, Route("serve/{orderItemId}")]
        public IHttpActionResult Serve(int orderItemId)
        {
            var item = _db.OrderItems.Find(orderItemId);
            if (item == null) return NotFound();

            item.KitchenStatus = "SERVED";
            item.StatusUpdatedAt = DateTime.Now;
            _db.SaveChanges();

            KitchenHub.NotifyStatusChanged(orderItemId, "SERVED");
            return Ok();
        }

        // ============================
        // ⑤ 会計済み（テーブル単位でクローズ）
        // ============================
        [HttpPost, Route("close-table/{tableId}")]
        public IHttpActionResult CloseTable(int tableId)
        {
            var orders = _db.Orders
                .Where(o => o.TableId == tableId && o.Status == "OPEN")
                .ToList();

            if (!orders.Any())
            {
                return Ok(); // 何もなければそのまま
            }

            foreach (var o in orders)
            {
                o.Status = "CLOSED";
            }

            _db.SaveChanges();

            // ここで KitchenHub を飛ばせば、他端末も再読込できる
            // KitchenHub.NotifyTableClosed(tableId); などを将来追加してもOK

            return Ok();
        }
    }
}

