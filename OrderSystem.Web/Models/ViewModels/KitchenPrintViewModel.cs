using System;
using System.Collections.Generic;

namespace OrderSystem.Web.ViewModels
{
    public class KitchenPrintViewModel
    {
        public int OrderId { get; set; }           //伝票番号
        public string TableName { get; set; }      // T-01 など
        public DateTime OrderTime { get; set; }    // 注文時刻
        public List<KitchenPrintItem> Items { get; set; }
    }

    public class KitchenPrintItem
    {
        public int OrderIndex { get; set; } 
        public string MenuName { get; set; }       // メニュー名
        public int Quantity { get; set; }          // 個数
        public List<KitchenPrintOption> Options { get; set; }
    }

    public class KitchenPrintOption
    {
        public string OptionName { get; set; }     // 例：温泉卵トッピング
        public int ExtraPrice { get; set; }        // 例：120
    }
}

