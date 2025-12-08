using System.Collections.Generic;

namespace OrderSystem.Web.Models
{
    /// <summary>
    /// 1品分（メニュー + 数量 + オプションID）の情報
    /// </summary>
    public class OrderSubmitItemDto
    {
        public int MenuId { get; set; }
        public int Qty { get; set; }
        public List<int> OptionIds { get; set; }
    }

    /// <summary>
    /// 注文送信リクエスト全体
    /// </summary>
    public class OrderSubmitRequest
    {
        public int TableId { get; set; }
        public int NumPeople { get; set; }   // 今は 0 でもOK（あとで人数入力を付ける）

        public List<OrderSubmitItemDto> Items { get; set; }
    }
}
