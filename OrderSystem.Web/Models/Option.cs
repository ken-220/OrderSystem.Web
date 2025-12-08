using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class Option
    {
        public int OptionId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required, StringLength(100)]
        public string OptionName { get; set; }

        [Required]
        public int ExtraPrice { get; set; }
    }
}
