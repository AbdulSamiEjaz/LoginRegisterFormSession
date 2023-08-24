using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Login_RegisterFormSession.Models
{
    public class TaskModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        [DisplayName("Title")]
        public string title { get; set; }
        [Required]
        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string desctiption { get; set; }
        [Required]
        [DisplayName("UserId")]
        public int userId { get; set; }
    }
}
