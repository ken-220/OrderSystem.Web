namespace OrderSystem.Web.Models
{
    public class UpdateOrderRequest
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        // ★ ここが最重要！ 初期値を必ず設定する！
        public int[] OptionIds { get; set; } = new int[] { };
    }
}
