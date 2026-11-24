using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class WorkoutSet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkoutSpecificationId { get; set; }

        [ForeignKey(nameof(WorkoutSpecificationId))]
        public WorkoutSpecification WorkoutSpecification { get; set; }

        public int SetNumber { get; set; }

        public decimal Weight { get; set; }

        public int Reps { get; set; }

        public WorkoutSet() { }

        public WorkoutSet(int workoutSpecificationId, int setNumber, decimal weight, int reps)
        {
            WorkoutSpecificationId = workoutSpecificationId;
            SetNumber = setNumber;
            Weight = weight;
            Reps = reps;
        }
    }
}
