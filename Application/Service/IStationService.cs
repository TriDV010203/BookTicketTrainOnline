using BookingTrain.Domain.Entities;

namespace BookingTrain.Application.Service
{
    public interface IStationService
    {
        Task<IEnumerable<Station>> GetAllStationsAsync();
        Task<Station?> GetStationByIdAsync(int id);
        Task CreateStationAsync(Station station);
        Task UpdateStationAsync(int id, Station station);
        Task DeleteStationAsync(int id);
    }
}
