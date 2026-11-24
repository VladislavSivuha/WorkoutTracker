using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces
{
    public interface IWorkoutService
    {
        Task<WorkoutHeader?> GetActiveWorkoutAsync(int userId);
        Task<List<TemplateHeader>> GetUserTemplatesAsync(int userId);
        Task<WorkoutHeader> StartWorkoutAsync(int userId, int templateHeaderId);
        Task AddSetAsync(int workoutSpecificationId, decimal weight, int reps);
        Task RemoveSetAsync(int setId);
        Task AddExerciseAsync(int workoutId, int exerciseId);
        Task RemoveExerciseAsync(int workoutSpecificationId);
        Task FinishWorkoutAsync(int workoutId);
    }
}
