using System.ComponentModel.DataAnnotations;
namespace TaskManger.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Status { get; set; } // "To Do", "In Progress", "Done"

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; } // FK
        // Navigation
        public User User { get; set; }
    }
}
