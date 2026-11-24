using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Models
{
    public class TemplateSpecification
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public int TemplateHeaderId { get; set; }
        [ForeignKey(nameof(TemplateHeaderId))]
        public TemplateHeader TemplateHeader { get; set; }
        [Required(ErrorMessage = "Должно быть заполнено")]
        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; }
        public int Position { get; set; }
        
        public TemplateSpecification()
        {

        }
        public TemplateSpecification(int templateHeaderId, int exerciseId, int position)
        {
            TemplateHeaderId = templateHeaderId;
            ExerciseId = exerciseId;
            Position = position;
        }
    }
}
