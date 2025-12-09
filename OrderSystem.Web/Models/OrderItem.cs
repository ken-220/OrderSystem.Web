using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public int MenuId { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }

        public string KitchenStatus { get; set; }
        public DateTime StatusUpdatedAt { get; set; }

        // ★ これが必要（Kitchen側のエラーを解消）
        public virtual Order Order { get; set; }

        // これも必要（GetTableOrdersで MenuItem.Name を使うなら）
        public virtual MenuItem MenuItem { get; set; }

        // オプション
        public virtual ICollection<OrderItemOption> OrderItemOptions { get; set; }
    }


}

