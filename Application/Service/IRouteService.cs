using BookingTrain.Domain.Entities;

namespace BookingTrain.Application.Service
{
    public interface IRouteService
    {
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        Task<Route?> GetRouteByIdAsync(int id);
        Task CreateRouteAsync(Route route);
        Task UpdateRouteAsync(int id, Route route);
        Task DeleteRouteAsync(int id);
    }
}
