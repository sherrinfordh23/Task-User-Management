using System.ComponentModel.DataAnnotations;

namespace WebServerProject_AnThanhTrinh_2033002.Models
{
    public class Task
    {
        [Key]
        public string TaskUid { get; set; }
        [Required]
        public string CreatedByUid { get; set; }
        [Required]
        public string CreatedByName { get; set; }
        [Required]
        public string AssignedToUid { get; set; }
        [Required]
        public string AssignedToName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Done { get; set; }

        public Task()
        {
            TaskUid = Guid.NewGuid().ToString();
            Done = false;
        }


    }
}
