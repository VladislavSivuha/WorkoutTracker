using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetStreakWeeksAsync(int userId);
        Task<int> GetTotalWorkoutsAsync(int userId);
        Task<List<ExerciseTonnageTop5>> GetTopExercisesByTonnageAsync(int userId, int top);
        Task<List<UserTonnageTop3>> GetTopUsersByTonnageAsync(int top);
        Task<List<CurrentMonthCalendar>> GetMonthCalendarAsync(int userId, int year, int month);
    }
}
