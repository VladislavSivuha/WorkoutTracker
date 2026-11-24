using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Services;

namespace WorkoutTracker.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        public async Task<IActionResult> Statistics()
        {
            ViewBag.StreakWeeks = await _statisticsService.GetStreakWeeksAsync(CurrentUserId);
            ViewBag.TotalWorkouts = await _statisticsService.GetTotalWorkoutsAsync(CurrentUserId);
            ViewBag.TopExercises = await _statisticsService.GetTopExercisesByTonnageAsync(CurrentUserId, 5);
            ViewBag.TopUsers = await _statisticsService.GetTopUsersByTonnageAsync(3);

            var now = DateTime.UtcNow;
            ViewBag.Calendar = await _statisticsService.GetMonthCalendarAsync(CurrentUserId, now.Year, now.Month);
            ViewBag.CalendarMonthName = now.ToString("MMMM yyyy", new System.Globalization.CultureInfo("ru-RU")).ToUpper();

            return View();
        }
    }
}
