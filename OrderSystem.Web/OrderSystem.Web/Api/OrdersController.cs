using OrderSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using OrderSystem.Web.Hubs;

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
                           .Select(i => new {
                               i.MenuId,
                               MenuName = _db.MenuItems.Where(m => m.MenuId == i.MenuId)
                                                       .Select(m => m.Name).FirstOrDefault(),
                               i.Quantity
                           })
            })
            .ToList();

        return Ok(orders);
    }


    [HttpPost, Route("")]
    public IHttpActionResult Create(CreateOrderRequest req)
    {
        if (req.Items == null || !req.Items.Any())
            return BadRequest("Items is empty");

        using (var tx = _db.Database.BeginTransaction())
        {
            var order = new Order
            {
                TableId = req.TableId,
                UserId = req.UserId,
                NumPeople = req.NumPeople,
                Status = "NEW",
                OrderTime = DateTime.Now
            };
            _db.Orders.Add(order);
            _db.SaveChanges();

            foreach (var it in req.Items)
            {
                var menu = _db.MenuItems.Find(it.MenuId);
                if (menu == null)
                {
                    tx.Rollback();
                    return BadRequest("Menu not found: " + it.MenuId);
                }

                var oi = new OrderItem
                {
                    OrderId = order.OrderId,
                    MenuId = it.MenuId,
                    Quantity = it.Quantity,
                    Price = menu.Price,
                    KitchenStatus = "PENDING",
                    StatusUpdatedAt = DateTime.Now
                };
                _db.OrderItems.Add(oi);
                _db.SaveChanges();

                if (it.OptionIds != null)
                {
                    foreach (var oid in it.OptionIds)
                    {
                        _db.OrderItemOptions.Add(new OrderItemOption
                        {
                            OrderItemId = oi.OrderItemId,
                            OptionId = oid
                        });
                    }
                }
            }

            _db.SaveChanges();
            tx.Commit();

            // 厨房へ SignalR 通知
            KitchenHub.NotifyOrderCreated(order.OrderId, order.TableId);

            return Ok(new { orderId = order.OrderId });
        }
    }
}