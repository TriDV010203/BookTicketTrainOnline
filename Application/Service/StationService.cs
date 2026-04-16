using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StationService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Station>> GetAllStationsAsync()
            => await _unitOfWork.Stations.GetAllAsync();

        public async Task<Station?> GetStationByIdAsync(int id)
            => await _unitOfWork.Stations.GetByIdAsync(id);

        public async Task CreateStationAsync(Station station)
        {
            await _unitOfWork.Stations.AddAsync(station);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateStationAsync(int id, Station station)
        {
            var existing = await _unitOfWork.Stations.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy ga tàu này");
            existing.Name     = station.Name;
            existing.Location = station.Location;
            _unitOfWork.Stations.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteStationAsync(int id)
        {
            var station = await _unitOfWork.Stations.GetByIdAsync(id);
            if (station != null)
            {
                _unitOfWork.Stations.Delete(station);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
