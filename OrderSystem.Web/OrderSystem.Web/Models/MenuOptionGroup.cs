using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystem.Web.Models
{
    public class MenuOptionGroup
    {
        [Key, Column(Order = 0)]
        public int MenuId { get; set; }

        [Key, Column(Order = 1)]
        public int GroupId { get; set; }
    }
}
