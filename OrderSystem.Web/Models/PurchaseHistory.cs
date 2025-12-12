using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
  public class PurchaseHistory
  {
    // 主キー
    public int PurchaseHistoryId { get; set; }

    // 発注した食材
    [Required]
    public int IngredientId { get; set; }

    // 発注数量
    [Required]
    public int Quantity { get; set; }

    // 発注日
    [Required]
    public DateTime PurchaseDate { get; set; }

    // 備考（仕入れ先など任意）
    [StringLength(200)]
    public string Note { get; set; }

    // ナビゲーションプロパティ
    public virtual Ingredient Ingredient { get; set; }
  }
}
