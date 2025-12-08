using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Web.Models.ViewModels
{
    public class CashierTableViewModel
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public bool HasOpenOrder { get; set; }
        public int TotalAmount { get; set; }

        public int OrderItemCount { get; set; }   // 注文数
        public int NumPeople { get; set; }        // 人数

    }

}