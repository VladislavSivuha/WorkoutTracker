using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WorkoutTracker.Models
{
    public class TemplateHeader
    {

        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        [Required(ErrorMessage = "Должно быть заполнено")]
        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<TemplateSpecification> Specification { get; set; } = new();

        public TemplateHeader()
        {

        }

        public TemplateHeader(int userId, string name, string description)
        {
            UserId = userId;
            Name = name;
            Description = description;
        }
    }
}
