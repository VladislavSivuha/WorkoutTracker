using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;
        private readonly PgsqlDbContext _context;

        public WorkoutController(IWorkoutService workoutService, PgsqlDbContext context)
        {
            _workoutService = workoutService;
            _context = context;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        public async Task<IActionResult> Workout()
        {
            var active = await _workoutService.GetActiveWorkoutAsync(CurrentUserId);
            ViewBag.Templates = await _workoutService.GetUserTemplatesAsync(CurrentUserId);

            var allExercises = await _context.Exercise.OrderBy(e => e.Name).ToListAsync();

            if (active != null)
            {
                var usedIds = active.Specification.Select(s => s.ExerciseId).ToHashSet();
                allExercises = allExercises.Where(e => !usedIds.Contains(e.Id)).ToList();
            }

            ViewBag.AllExercises = allExercises;
            return View(active);
        }
        [HttpPost]
        public async Task<IActionResult> Start(int templateHeaderId)
        {
            try
            {
                await _workoutService.StartWorkoutAsync(CurrentUserId, templateHeaderId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Workout));
        }
        [HttpPost]
        public async Task<IActionResult> AddSet(int workoutExerciseId, decimal weight, int reps)
        {
            try
            {
                await _workoutService.AddSetAsync(workoutExerciseId, weight, reps);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Workout));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveSet(int setId)
        {
            await _workoutService.RemoveSetAsync(setId);
            return RedirectToAction(nameof(Workout));
        }
        [HttpPost]
        public async Task<IActionResult> AddExercise(int workoutId, int exerciseId)
        {
            try
            {
                await _workoutService.AddExerciseAsync(workoutId, exerciseId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Workout));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveExercise(int workoutExerciseId)
        {
            await _workoutService.RemoveExerciseAsync(workoutExerciseId);
            return RedirectToAction(nameof(Workout));
        }
        [HttpPost]
        public async Task<IActionResult> Finish(int workoutId)
        {
            try
            {
                await _workoutService.FinishWorkoutAsync(workoutId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Workout));
        }
    }
}
