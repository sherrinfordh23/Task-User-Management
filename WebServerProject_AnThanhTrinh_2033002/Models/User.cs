using System.ComponentModel.DataAnnotations;

namespace WebServerProject_AnThanhTrinh_2033002.Models
{
    public class User
    {
        [Key]
        public string Uid { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public User()
        {
            Uid = Guid.NewGuid().ToString();
        }

    }
}
