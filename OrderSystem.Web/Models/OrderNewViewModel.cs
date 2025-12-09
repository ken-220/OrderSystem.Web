using System.Collections.Generic;

namespace OrderSystem.Web.Models
{
    public class OrdersNewViewModel
    {
        public int TableId { get; set; }
        public string TableName { get; set; }

        // LUNCH / DINNER のどちらを開くか
        public string CurrentMenuTime { get; set; }

        // パスタ / ピザ / サラダ / 肉 / ドリンク … のような一覧
        public List<string> Categories { get; set; }

        // その時間帯（ランチ or ディナー）の全メニュー
        public List<MenuItem> MenuItems { get; set; }

        // GroupId → Options 一覧
        public Dictionary<int, List<Option>> OptionListGrouped { get; set; }

        //人数の選択
        public int NumPeople { get; set; }
    }
}

