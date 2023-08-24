using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Login_RegisterFormSession.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        [DisplayName("Email")]
        public string email { get; set; }
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [DisplayName("Username")]

        public string username { get; set; }

    }
}
