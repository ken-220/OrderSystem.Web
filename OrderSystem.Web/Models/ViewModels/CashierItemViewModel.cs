using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.Web.Models.ViewModels
{
    public class CashierItemViewModel
    {
        public string MenuName { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }

       
        public int OptionPrice { get; set; }

        // 小計（本体価格 × 数量 + オプション価格）
        public int Subtotal { get; set; }
    }


}