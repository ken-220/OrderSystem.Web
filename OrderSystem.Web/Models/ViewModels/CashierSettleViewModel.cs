using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Web.Models.ViewModels
{
    public class CashierSettleViewModel
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public int OrderId { get; set; }
        public List<CashierItemViewModel> Items { get; set; }
        public int TotalAmount { get; set; }
    }

}