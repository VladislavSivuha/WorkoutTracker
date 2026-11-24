using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace WorkoutTracker.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly PgsqlDbContext _context;

        public WorkoutService(PgsqlDbContext context)
        {
            _context = context;
        }


        public async Task<WorkoutHeader?> GetActiveWorkoutAsync(int userId)
        {
            return await _context.WorkoutHeader
                .Include(w => w.Specification)
                    .ThenInclude(s => s.Exercise)
                .Include(w => w.Specification)
                    .ThenInclude(s => s.Sets)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.FinishedAt == null);
        }
        public async Task<List<TemplateHeader>> GetUserTemplatesAsync(int userId)
        {
            return await _context.TemplateHeader
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }
        public async Task<WorkoutHeader> StartWorkoutAsync(int userId, int templateHeaderId)
        {
            var existing = await _context.WorkoutHeader
                .FirstOrDefaultAsync(w => w.UserId == userId && w.FinishedAt == null);
            if (existing != null)
                throw new InvalidOperationException("У вас уже есть активная тренировка. Завершите её перед началом новой.");

            var template = await _context.TemplateHeader
                .Include(t => t.Specification)
                    .ThenInclude(s => s.Exercise)
                .FirstOrDefaultAsync(t => t.Id == templateHeaderId && t.UserId == userId);

            if (template == null)
                throw new InvalidOperationException("Шаблон не найден.");
            if (template.Specification.Count == 0)
                throw new InvalidOperationException("Шаблон пуст. Добавьте упражнения в шаблон перед началом тренировки.");

            var workout = new WorkoutHeader(userId, template.Name);

            foreach (var spec in template.Specification.OrderBy(s => s.Position))
            {
                workout.Specification.Add(new WorkoutSpecification
                {
                    ExerciseId = spec.ExerciseId,
                    Position = spec.Position
                });
            }

            _context.WorkoutHeader.Add(workout);
            await _context.SaveChangesAsync();

            return workout;
        }
        public async Task AddSetAsync(int workoutSpecificationId, decimal weight, int reps)
        {
            var spec = await _context.WorkoutSpecification
                .FirstOrDefaultAsync(s => s.Id == workoutSpecificationId);

            if (spec == null)
                throw new InvalidOperationException("Упражнение в тренировке не найдено.");

            var maxSetNumber = await _context.WorkoutSet
                .Where(s => s.WorkoutSpecificationId == workoutSpecificationId)
                .Select(s => (int?)s.SetNumber)
                .MaxAsync() ?? 0;

            var set = new WorkoutSet(workoutSpecificationId, maxSetNumber + 1, weight, reps);
            _context.WorkoutSet.Add(set);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveSetAsync(int setId)
        {
            var set = await _context.WorkoutSet.FirstOrDefaultAsync(s => s.Id == setId);
            if (set == null) return;

            _context.WorkoutSet.Remove(set);
            await _context.SaveChangesAsync();
        }
        public async Task AddExerciseAsync(int workoutId, int exerciseId)
        {
            var workout = await _context.WorkoutHeader
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.FinishedAt == null);

            if (workout == null)
                throw new InvalidOperationException("Активная тренировка не найдена.");

            if (!await _context.Exercise.AnyAsync(e => e.Id == exerciseId))
                throw new InvalidOperationException("Упражнение не найдено.");

            var alreadyExists = await _context.WorkoutSpecification
                .AnyAsync(s => s.WorkoutHeaderId == workoutId && s.ExerciseId == exerciseId);

            if (alreadyExists)
                throw new InvalidOperationException("Это упражнение уже есть в тренировке.");

            var maxPosition = await _context.WorkoutSpecification
                .Where(s => s.WorkoutHeaderId == workoutId)
                .Select(s => (int?)s.Position)
                .MaxAsync() ?? -1;

            var spec = new WorkoutSpecification(workoutId, exerciseId, maxPosition + 1);
            _context.WorkoutSpecification.Add(spec);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveExerciseAsync(int workoutSpecificationId)
        {
            var spec = await _context.WorkoutSpecification
                .Include(s => s.Sets)
                .FirstOrDefaultAsync(s => s.Id == workoutSpecificationId);

            if (spec == null) return;

            var workoutId = spec.WorkoutHeaderId;

            if (spec.Sets.Any())
                _context.WorkoutSet.RemoveRange(spec.Sets);

            _context.WorkoutSpecification.Remove(spec);
            await _context.SaveChangesAsync();

            var remaining = await _context.WorkoutSpecification
                .Where(s => s.WorkoutHeaderId == workoutId)
                .OrderBy(s => s.Position)
                .ToListAsync();

            for (int i = 0; i < remaining.Count; i++)
                remaining[i].Position = i;

            await _context.SaveChangesAsync();
        }
        public async Task FinishWorkoutAsync(int workoutId)
        {
            var workout = await _context.WorkoutHeader
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.FinishedAt == null);

            if (workout == null)
                throw new InvalidOperationException("Активная тренировка не найдена.");

            workout.FinishedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
