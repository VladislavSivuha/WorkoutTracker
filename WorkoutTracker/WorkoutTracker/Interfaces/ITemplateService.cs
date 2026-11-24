using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces
{
    public interface ITemplateService
    {
        Task<List<TemplateHeader>> GetByUserAsync(int userId);
        Task<TemplateHeader> CreateAsync(int userId, string name, string description);
        Task<TemplateHeader?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, string name, string description);
        Task<List<Exercise>> GetAvailableExercisesAsync(int templateHeaderId, int userId);
        Task AddExerciseAsync(int templateHeaderId, int exerciseId, int userId);
        Task RemoveExerciseAsync(int templateHeaderId, int exerciseId, int userId);
    }
}
