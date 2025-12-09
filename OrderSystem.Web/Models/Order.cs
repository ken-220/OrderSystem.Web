using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int TableId { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual Table Table { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } // NEW/IN_PROGRESS/DONE

       public int? NumPeople { get; set; }  // nullable int


        // ★ レジ用の追加項目
        public DateTime? PaidAt { get; set; }      // 会計日時
        public string PaymentMethod { get; set; }  // "CASH" / "CARD" など
        public int? PaidAmount { get; set; }       // 支払金額（合計）
    }
}
