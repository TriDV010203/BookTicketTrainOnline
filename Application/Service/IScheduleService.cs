using BookingTrain.Domain.Entities;

namespace BookingTrain.Application.Service
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
        Task<IEnumerable<Schedule>> GetSchedulesByRouteAsync(int routeId);
        Task<Schedule?> GetScheduleByIdAsync(int id);
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(int id, Schedule schedule);
        Task DeleteScheduleAsync(int id);
    }
}
