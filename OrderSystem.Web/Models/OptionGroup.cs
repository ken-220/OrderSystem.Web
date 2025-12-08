using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class OptionGroup
    {
        [Key]                     
        public int GroupId { get; set; }       // グループを一意に識別する番号

        [Required, StringLength(100)]          // 必須 かつ 最大100文字まで
        public string GroupName { get; set; }  // 「パスタの種類」「ドリンクの種類」などの名前

        [Required, StringLength(10)]
        public string SelectMode { get; set; } // SINGLE/MULTI

        [Required]
        public bool IsRequired { get; set; }
    }
}

