using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class WorkoutHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required(ErrorMessage = "Должно быть заполнено")]
        public string Name { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public DateTime? FinishedAt { get; set; }

        public List<WorkoutSpecification> Specification { get; set; } = new();



        public WorkoutHeader() { }

        public WorkoutHeader(int userId, string name)
        {
            UserId = userId;
            Name = name;
            StartedAt = DateTime.UtcNow;
        }
    }
}
