using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class OptionGroup
    {
        [Key]                      // ← これを追加！
        public int GroupId { get; set; }

        [Required, StringLength(100)]
        public string GroupName { get; set; }

        [Required, StringLength(10)]
        public string SelectMode { get; set; } // SINGLE/MULTI

        [Required]
        public bool IsRequired { get; set; }
    }
}

