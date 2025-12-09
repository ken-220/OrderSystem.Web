using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class MenuItem
    {
        [Key]                      // ← これを追加！
        public int MenuId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required, StringLength(50)]
        public string Category { get; set; } // Pasta, Pizza, Main, Drink, Set...

        [Required, StringLength(10)]
        public string MenuTime { get; set; } // LUNCH/DINNER/BOTH
    }
}

