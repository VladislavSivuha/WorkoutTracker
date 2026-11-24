using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services
{
	public class ExerciseService : IExerciseService
	{
		private readonly PgsqlDbContext _context;
        public ExerciseService(PgsqlDbContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(Exercise exercise)
        {
            _context.Exercise.Add(exercise);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var e = await _context.Exercise.FindAsync(id);
            
            if (e == null)
                return;

            _context.Exercise.Remove(e);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id, Exercise exercise)
        {
            var e = await _context.Exercise.FindAsync(id);
            
            if (e == null)
                return;

            e.Name = exercise.Name;
            e.Description = exercise.Description;
            e.ImageUrl = exercise.ImageUrl;

            await _context.SaveChangesAsync();
        }
        public async Task<List<Exercise>> GetAsync()
        {
            return await _context.Exercise.OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercise.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
