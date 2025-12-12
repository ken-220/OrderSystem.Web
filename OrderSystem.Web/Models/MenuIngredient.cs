using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystem.Web.Models
{
    public class MenuIngredient
    {
        // 複合主キー：MenuId + IngredientId

        [Key]
        [Column(Order = 0)]
        public int MenuId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int IngredientId { get; set; }

        // メニュー1皿あたり使用する量（g / 個など）
        [Required]
        public int QuantityUsed { get; set; }

        // ナビゲーションプロパティ
        public virtual MenuItem MenuItem { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
