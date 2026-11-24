using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly PgsqlDbContext _context;
        public StatisticsService(PgsqlDbContext context)
        {
            _context = context;
        }

        private static (int Year, int Week) PreviousWeek(int year, int week)
        {
            if (week > 1) return (year, week - 1);

            // Переход на последнюю неделю предыдущего года
            var lastWeekOfPrevYear = ISOWeek.GetWeeksInYear(year - 1);
            return (year - 1, lastWeekOfPrevYear);
        }
        public async Task<int> GetStreakWeeksAsync(int userId)
        {
            var workoutDates = await _context.WorkoutHeader
                .Where(w => w.UserId == userId && w.FinishedAt != null)
                .Select(w => w.StartedAt)
                .ToListAsync();

            if (workoutDates.Count == 0)
                return 0;

            var workoutWeeks = workoutDates
                .Select(d => (Year: ISOWeek.GetYear(d), Week: ISOWeek.GetWeekOfYear(d)))
                .ToHashSet();

            var today = DateTime.UtcNow;
            var currentYear = ISOWeek.GetYear(today);
            var currentWeek = ISOWeek.GetWeekOfYear(today);

            int streak = 0;
            var year = currentYear;
            var week = currentWeek;

            if (!workoutWeeks.Contains((year, week)))
                (year, week) = PreviousWeek(year, week);

            while (workoutWeeks.Contains((year, week)))
            {
                streak++;
                (year, week) = PreviousWeek(year, week);
            }

            return streak;
        }
        public async Task<int> GetTotalWorkoutsAsync(int userId)
        {
            return await _context.WorkoutHeader
                .CountAsync(w => w.UserId == userId && w.FinishedAt != null);
        }
        public async Task<List<ExerciseTonnageTop5>> GetTopExercisesByTonnageAsync(int userId, int top)
        {
            return await _context.WorkoutSet
                .Where(s => s.WorkoutSpecification.WorkoutHeader.UserId == userId
                            && s.WorkoutSpecification.WorkoutHeader.FinishedAt != null)
                .GroupBy(s => new
                {
                    s.WorkoutSpecification.ExerciseId,
                    s.WorkoutSpecification.Exercise.Name
                })
                .Select(g => new ExerciseTonnageTop5
                {
                    ExerciseId = g.Key.ExerciseId,
                    ExerciseName = g.Key.Name,
                    Tonnage = g.Sum(s => s.Weight * s.Reps)
                })
                .OrderByDescending(d => d.Tonnage)
                .Take(top)
                .ToListAsync();
        }
        public async Task<List<UserTonnageTop3>> GetTopUsersByTonnageAsync(int top)
        {
            return await _context.WorkoutSet
                .Where(s => s.WorkoutSpecification.WorkoutHeader.FinishedAt != null)
                .GroupBy(s => new
                {
                    s.WorkoutSpecification.WorkoutHeader.UserId,
                    s.WorkoutSpecification.WorkoutHeader.User.Login
                })
                .Select(g => new UserTonnageTop3
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.Login,
                    Tonnage = g.Sum(s => s.Weight * s.Reps)
                })
                .OrderByDescending(d => d.Tonnage)
                .Take(top)
                .ToListAsync();
        }
        public async Task<List<CurrentMonthCalendar>> GetMonthCalendarAsync(int userId, int year, int month)
        {
            var allDates = await _context.WorkoutHeader
                .Where(w => w.UserId == userId && w.FinishedAt != null)
                .Select(w => w.StartedAt)
                .ToListAsync();

            var workoutDates = allDates
                .Where(d => d.Year == year && d.Month == month)
                .Select(d => d.Day)
                .ToHashSet();

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var firstDay = new DateTime(year, month, 1);
            var today = DateTime.UtcNow;

            var result = new List<CurrentMonthCalendar>();

            int offset = ((int)firstDay.DayOfWeek + 6) % 7;
            for (int i = 0; i < offset; i++)
                result.Add(new CurrentMonthCalendar { Day = 0 });

            for (int d = 1; d <= daysInMonth; d++)
            {
                result.Add(new CurrentMonthCalendar
                {
                    Day = d,
                    HasWorkout = workoutDates.Contains(d),
                    IsToday = (today.Year == year && today.Month == month && today.Day == d)
                });
            }

            while (result.Count % 7 != 0)
                result.Add(new CurrentMonthCalendar { Day = 0 });

            return result;
        }
    }
}
