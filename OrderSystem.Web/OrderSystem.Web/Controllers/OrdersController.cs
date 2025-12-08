using OrderSystem.Web.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace OrderSystem.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrdersDbContext _db = new OrdersDbContext();

        // POST: /Orders/Submit
        [HttpPost]
        public ActionResult Submit(OrderSubmitRequest request)
        {
            // 簡単なバリデーション
            if (request == null || request.Items == null || !request.Items.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "注文内容が空です。");
            }

            using (var db = new OrdersDbContext())
            {
                // 1) Orders テーブルに親レコードを作成
                var order = new Order
                {
                    TableId = request.TableId,
                    OrderTime = DateTime.Now,
                    Status = "OPEN",          // 設計書に合わせて適宜変更
                    NumPeople = request.NumPeople
                };

                db.Orders.Add(order);
                db.SaveChanges(); // OrderId 採番

                // 2) 各品目を OrderItems / OrderItemOptions に保存
                foreach (var itemDto in request.Items)
                {
                    // ★ 数量チェックを追加
                    if (itemDto.Qty <= 0)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "数量が不正です。");
                    }

                    // Menu の価格を DB から取得
                    var menu = db.MenuItems.Find(itemDto.MenuId);
                    if (menu == null) continue; // 万が一メニューがなければスキップ

                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        MenuId = itemDto.MenuId,
                        Quantity = itemDto.Qty,
                        Price = menu.Price,
                        KitchenStatus = "PENDING",          // ★ 追加：初期状態
                        StatusUpdatedAt = DateTime.Now      // ★ 追加：状態更新日時
                    };
                    db.OrderItems.Add(orderItem);

                    db.SaveChanges(); // OrderItemId を取得

                    // オプションがあれば OrderItemOptions に保存
                    if (itemDto.OptionIds != null)
                    {
                        foreach (var optId in itemDto.OptionIds)
                        {
                            var orderItemOpt = new OrderItemOption
                            {
                                OrderItemId = orderItem.OrderItemId,
                                OptionId = optId
                            };
                            db.OrderItemOptions.Add(orderItemOpt);
                        }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Entity {eve.Entry.Entity.GetType().Name} の検証エラー:");

                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(
                                $"- プロパティ {ve.PropertyName}: {ve.ErrorMessage}");
                        }
                    }
                    throw;
                }


                db.SaveChanges();

                // 成功レスポンス（JavaScript側で使えるようにJSONで返す）
                return Json(new
                {
                    success = true,
                    orderId = order.OrderId
                });
            }
        }


        // GET: /Orders/New?tableId=1
        public ActionResult New(int tableId)
        {
            var table = _db.Tables.FirstOrDefault(t => t.TableId == tableId);
            if (table == null) return HttpNotFound();

            // 現在の時間帯（ランチ or ディナー）
            var nowHour = System.DateTime.Now.Hour;
            string menuTime = (nowHour < 17) ? "LUNCH" : "DINNER";

            // メニュー読み込み
            var items = _db.MenuItems
                          .Where(m => m.MenuTime == menuTime)
                          .ToList();

            // カテゴリ一覧（重複排除）
            var categories = items
                .Select(x => x.Category)
                .Distinct()
                .ToList();

            // オプション一覧（GroupId ごと）
            var optionList = _db.Options
                .GroupBy(o => o.GroupId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var vm = new OrdersNewViewModel
            {
                TableId = table.TableId,
                TableName = table.TableName,
                CurrentMenuTime = menuTime,
                Categories = categories,
                MenuItems = items,
                OptionListGrouped = optionList
            };



            return View(vm);


        }
    }
}


