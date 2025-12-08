using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Role { get; set; } // Hall/Kitchen/Admin

        [Required, StringLength(256)]
        public string Password { get; set; }
    }
}
