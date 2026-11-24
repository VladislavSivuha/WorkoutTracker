using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly PgsqlDbContext _context;

        public TemplateService(PgsqlDbContext context)
        {
            _context = context;
        }

        public async Task<List<TemplateHeader>> GetByUserAsync(int userId)
        {
            return await _context.TemplateHeader
                .Where(t => t.UserId == userId)
                .Include(t => t.Specification)
                .ThenInclude(s => s.Exercise)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }
        public async Task<TemplateHeader> CreateAsync(int userId, string name, string description)
        {
            var header = new TemplateHeader(userId, name, description ?? string.Empty);
            _context.TemplateHeader.Add(header);
            await _context.SaveChangesAsync();
            return header;
        }
        public async Task<TemplateHeader?> GetByIdAsync(int id)
        {
            return await _context.TemplateHeader
                .Include(t => t.Specification)
                .ThenInclude(t => t.Exercise)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task DeleteAsync(int id)
        {
            var header = await _context.TemplateHeader
                .Include(t => t.Specification)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (header == null) return;

            if (header.Specification.Any())
                _context.TemplateSpecification.RemoveRange(header.Specification);

            _context.TemplateHeader.Remove(header);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id, string name, string description)
        {
            var header = await _context.TemplateHeader.FindAsync(id);
            if (header == null)
                return;

            header.Name = name;
            header.Description = description ?? string.Empty;
            await _context.SaveChangesAsync();
        }
        public async Task<List<Exercise>> GetAvailableExercisesAsync(int templateHeaderId, int userId)
        {
            var header = await _context.TemplateHeader
                .Include(t => t.Specification)
                .FirstOrDefaultAsync(t => t.Id == templateHeaderId);

            if (header == null || header.UserId != userId)
                return new List<Exercise>();

            var usedIds = header.Specification.Select(s => s.ExerciseId).ToHashSet();

            return await _context.Exercise
                .Where(e => !usedIds.Contains(e.Id))
                .OrderBy(e => e.Name)
                .ToListAsync();
        }
        public async Task AddExerciseAsync(int templateHeaderId, int exerciseId, int userId)
        {
            var header = await _context.TemplateHeader
                .Include(t => t.Specification)
                .FirstOrDefaultAsync(t => t.Id == templateHeaderId);

            if (header == null || header.UserId != userId)
                return;

            if (!await _context.Exercise.AnyAsync(e => e.Id == exerciseId))
                return;

            if (header.Specification.Any(s => s.ExerciseId == exerciseId))
                return;

            var nextPosition = header.Specification.Any()
                ? header.Specification.Max(s => s.Position) + 1
                : 0;

            var spec = new TemplateSpecification(templateHeaderId, exerciseId, nextPosition);
            _context.TemplateSpecification.Add(spec);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveExerciseAsync(int templateHeaderId, int exerciseId, int userId)
        {
            var header = await _context.TemplateHeader
                .Include(t => t.Specification)
                .FirstOrDefaultAsync(t => t.Id == templateHeaderId);

            if (header == null || header.UserId != userId)
                return;

            var spec = header.Specification.FirstOrDefault(s => s.ExerciseId == exerciseId);
            if (spec == null) return;

            _context.TemplateSpecification.Remove(spec);
            await _context.SaveChangesAsync();
        }
    }
}
