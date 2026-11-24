using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly IExerciseService _exerciseService; 
        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<IActionResult> Exercise()
        {
            return View(await _exerciseService.GetAsync());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteExecriseById(int id)
        {
            await _exerciseService.DeleteAsync(id);
            return RedirectToAction("Exercise");
        }
        [HttpGet]
        public async Task<IActionResult> EditExercise(int id)
        {
            return View(await _exerciseService.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> EditExercise(Exercise exercise)
        {
            await _exerciseService.UpdateAsync(exercise.Id, exercise);
            return RedirectToAction("Exercise");
        }
        [HttpPost]
        public async Task<IActionResult> Create(Exercise exercise)
        {
            await _exerciseService.CreateAsync(new Exercise("", "", ""));
            return RedirectToAction("Exercise");
        }
    }
}
