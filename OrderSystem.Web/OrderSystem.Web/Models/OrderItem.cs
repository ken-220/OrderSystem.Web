using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public int Quantity { get; set; } // >= 1

        [Required]
        public int Price { get; set; }

        [Required, StringLength(20)]
        public string KitchenStatus { get; set; } // PENDING/IN_PROGRESS/DONE

        public DateTime StatusUpdatedAt { get; set; }

        // 🔥 これを追加
        public virtual Order Order { get; set; }

        // 🔥 ついでに MenuItem もあると便利
        public virtual MenuItem MenuItem { get; set; }
    }
}

