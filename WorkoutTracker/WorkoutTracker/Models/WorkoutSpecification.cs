using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class WorkoutSpecification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WorkoutHeaderId { get; set; }
        [ForeignKey(nameof(WorkoutHeaderId))]
        public WorkoutHeader WorkoutHeader { get; set; }
        [Required]
        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; }
        public int Position { get; set; }
        public List<WorkoutSet> Sets { get; set; } = new();




        public WorkoutSpecification() { }

        public WorkoutSpecification(int workoutHeaderId, int exerciseId, int position)
        {
            WorkoutHeaderId = workoutHeaderId;
            ExerciseId = exerciseId;
            Position = position;
        }
    }
}
