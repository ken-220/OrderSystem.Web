using OrderSystem.Web.Hubs;
using OrderSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

[Authorize]
[RoutePrefix("api/orders")]
public class OrdersController : ApiController
{
    private readonly OrdersDbContext _db = new OrdersDbContext();

    public class CreateOrderRequest
    {
        public int TableId { get; set; }
        public int UserId { get; set; }
        public int NumPeople { get; set; }
        public IEnumerable<Item> Items { get; set; }

        public class Item
        {
            public int MenuId { get; set; }
            public int Quantity { get; set; }
            public int[] OptionIds { get; set; }
        }
    }

    public class NewOrderRequest
    {
        public int TableId { get; set; }
        public int NumPeople { get; set; }
        public IEnumerable<OrderItemDto> Items { get; set; }

        public class OrderItemDto
        {
            public int MenuId { get; set; }
            public int Quantity { get; set; }
            public int[] OptionIds { get; set; }
        }
    }

    public class UpdateQtyRequest
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
    }


[HttpGet, Route("")]
    public IHttpActionResult GetOrders(string status = "NEW")
    {
        var orders = _db.Orders
            .Where(x => x.Status == status)
            .Select(o => new
            {
                o.OrderId,
                o.TableId,
                Items = _db.OrderItems
                           .Where(i => i.OrderId == o.OrderId)
                           .Select(i => new
                           {
                               i.MenuId,
                               MenuName = _db.MenuItems.Where(m => m.MenuId == i.MenuId)
                                                       .Select(m => m.Name).FirstOrDefault(),
                               i.Quantity
                           })
            })
            .ToList();

        return Ok(orders);
    }



    [HttpPost, Route("submit")]
    public IHttpActionResult Submit(NewOrderRequest req)
    {
        if (req.Items == null || !req.Items.Any())
            return BadRequest("Items empty");

        using (var tx = _db.Database.BeginTransaction())
        {
            // 1) OPEN状態の注文を取る（なければ作る）
            var order = _db.Orders
                .FirstOrDefault(x => x.TableId == req.TableId && x.Status == "OPEN");

            if (order == null)
            {
                order = new Order
                {
                    TableId = req.TableId,
                    OrderTime = DateTime.Now,
                    Status = "OPEN",
                    NumPeople = req.NumPeople
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
            }

            // 2) Items を数量方式で処理
            foreach (var item in req.Items)
            {
                var optionKey = string.Join(",", item.OptionIds.OrderBy(x => x));

                var existing = _db.OrderItems
                    .Where(x => x.OrderId == order.OrderId && x.MenuId == item.MenuId)
                    .ToList()
                    .Where(x =>
                    {
                        var dbOpts = _db.OrderItemOptions
                                        .Where(o => o.OrderItemId == x.OrderItemId)
                                        .Select(o => o.OptionId)
                                        .OrderBy(o => o)
                                        .ToList();

                        return dbOpts.SequenceEqual(item.OptionIds.OrderBy(o => o));
                    })
                    .FirstOrDefault();

                var menu = _db.MenuItems.Find(item.MenuId);

                if (existing == null)
                {
                    // 新規作成
                    var oi = new OrderItem
                    {
                        OrderId = order.OrderId,
                        MenuId = item.MenuId,
                        Price = menu.Price,
                        Quantity = item.Quantity,
                        KitchenStatus = "PENDING",
                        StatusUpdatedAt = DateTime.Now
                    };
                    _db.OrderItems.Add(oi);
                    _db.SaveChanges();

                    foreach (var optId in item.OptionIds)
                    {
                        _db.OrderItemOptions.Add(new OrderItemOption
                        {
                            OrderItemId = oi.OrderItemId,
                            OptionId = optId
                        });
                    }
                }
                else
                {
                    // 既存に加算
                    existing.Quantity += item.Quantity;
                }

                _db.SaveChanges();
            }

            tx.Commit();
            return Ok(new { success = true });
        }
    }


    [HttpPost, Route("updateQuantity")]
    public IHttpActionResult UpdateQuantity(UpdateQtyRequest req)
    {
        var item = _db.OrderItems.Find(req.OrderItemId);
        if (item == null) return NotFound();

        if (req.Quantity <= 0)
        {
            // 削除
            var opts = _db.OrderItemOptions
                .Where(o => o.OrderItemId == req.OrderItemId)
                .ToList();

            _db.OrderItemOptions.RemoveRange(opts);
            _db.OrderItems.Remove(item);
        }
        else
        {
            item.Quantity = req.Quantity;
        }

        _db.SaveChanges();
        return Ok(new { success = true });
    }






    [HttpGet, Route("table/{tableId}")]
    public IHttpActionResult GetTableOrders(int tableId)
    {
        var order = _db.Orders
            .FirstOrDefault(o => o.TableId == tableId && o.Status == "OPEN");

        if (order == null)
            return Ok(new List<object>());

        var items = _db.OrderItems
            .Where(i => i.OrderId == order.OrderId)
            .Select(i => new
            {
                i.OrderItemId,
                i.MenuId,
                MenuName = i.MenuItem.Name,   // ★ここを必ず MenuName にする
                i.Quantity,
                i.Price,
                Options = _db.OrderItemOptions
                    .Where(o => o.OrderItemId == i.OrderItemId)
                    .Select(o => new {
                        o.OptionId,
                        o.Option.OptionName,
                        o.Option.ExtraPrice
                    })
                    .ToList()
            })
            .ToList();

        return Ok(items);
    }


}

