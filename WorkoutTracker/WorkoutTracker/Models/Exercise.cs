using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
	public class Exercise
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Должно быть заполнено")]
		public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public List<TemplateSpecification> TemplateSpecifications { get; set; } = new();

        public Exercise()
		{

		}

		public Exercise(string name, string description, string imageUrl)
		{
			Name = name;
			Description = description;
			ImageUrl = imageUrl;
		}
	}
}
