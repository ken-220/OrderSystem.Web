using OrderSystem.Web.Models; // Order クラスを使うため（必要なら）

namespace OrderSystem.Web.Models.ViewModels
{
    public class TableCardViewModel
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public Order CurrentOrder { get; set; }

        public bool IsOccupied => CurrentOrder != null;
        public int NumPeople => CurrentOrder?.NumPeople ?? 0;

        public string ElapsedText
        {
            get
            {
                if (CurrentOrder == null) return "";
                var span = System.DateTime.Now - CurrentOrder.OrderTime;
                return string.Format("{0:00}:{1:00}", (int)span.TotalMinutes, span.Seconds);
            }
        }

        public int TotalAmount { get; set; }
    }
}
