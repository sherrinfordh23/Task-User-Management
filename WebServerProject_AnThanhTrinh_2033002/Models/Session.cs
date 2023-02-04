using System.ComponentModel.DataAnnotations;

namespace WebServerProject_AnThanhTrinh_2033002.Models
{
    public class Session
    {
        [Key]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }

        public Session()
        {
            Token = Guid.NewGuid().ToString();
        }
    }
}
