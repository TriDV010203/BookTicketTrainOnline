using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
            => await _unitOfWork.Schedules.GetAllAsync();

        public async Task<IEnumerable<Schedule>> GetSchedulesByRouteAsync(int routeId)
        {
            var schedules = await _unitOfWork.Schedules.GetAllAsync();
            return schedules.Where(s => s.RouteId == routeId);
        }

        public async Task<Schedule?> GetScheduleByIdAsync(int id)
            => await _unitOfWork.Schedules.GetByIdAsync(id);

        public async Task CreateScheduleAsync(Schedule schedule)
        {
            await _unitOfWork.Schedules.AddAsync(schedule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateScheduleAsync(int id, Schedule schedule)
        {
            var existing = await _unitOfWork.Schedules.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy lịch trình này");
            existing.TrainId       = schedule.TrainId;
            existing.RouteId       = schedule.RouteId;
            existing.DepartureTime = schedule.DepartureTime;
            existing.ArrivalTime   = schedule.ArrivalTime;
            _unitOfWork.Schedules.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
            if (schedule != null)
            {
                _unitOfWork.Schedules.Delete(schedule);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
