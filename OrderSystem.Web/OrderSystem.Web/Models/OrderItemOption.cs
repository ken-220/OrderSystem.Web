using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystem.Web.Models
{
    public class OrderItemOption
    {
        [Key, Column(Order = 0)]
        public int OrderItemId { get; set; }

        [Key, Column(Order = 1)]
        public int OptionId { get; set; }
    }
}
