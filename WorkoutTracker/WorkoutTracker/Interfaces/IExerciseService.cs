using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces
{
	public interface IExerciseService
	{
        Task<List<Exercise>> GetAsync();
        Task<Exercise?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Exercise exercise);
        Task CreateAsync(Exercise exercise);
    }
}
