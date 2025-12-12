using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class Ingredient
    {
        // 主キー
        public int IngredientId { get; set; }

        // 食材名（玉ねぎ / ベーコン など）
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // 単位（個 / g / ml など）
        [Required]
        [StringLength(20)]
        public string Unit { get; set; }

        // 現在の在庫量
        [Required]
        public int CurrentStock { get; set; }

        // 最低限確保しておきたい在庫量
        [Required]
        public int MinimumStock { get; set; }

        // 登録日時
        [Required]
        public DateTime CreatedAt { get; set; }

        // 更新日時
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
