using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class Table
    {
        public int TableId { get; set; }

        [Required, StringLength(50)]
        public string TableName { get; set; }
    }
}
