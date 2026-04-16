using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RouteService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
            => await _unitOfWork.Routes.GetAllAsync();

        public async Task<Route?> GetRouteByIdAsync(int id)
            => await _unitOfWork.Routes.GetByIdAsync(id);

        public async Task CreateRouteAsync(Route route)
        {
            await _unitOfWork.Routes.AddAsync(route);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateRouteAsync(int id, Route route)
        {
            var existing = await _unitOfWork.Routes.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy tuyến đường này");
            existing.FromStationId = route.FromStationId;
            existing.ToStationId   = route.ToStationId;
            _unitOfWork.Routes.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRouteAsync(int id)
        {
            var route = await _unitOfWork.Routes.GetByIdAsync(id);
            if (route != null)
            {
                _unitOfWork.Routes.Delete(route);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
