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

        [Required]
        public DateTime OrderTime { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } // NEW/IN_PROGRESS/DONE

        public int NumPeople { get; set; }
    }
}
