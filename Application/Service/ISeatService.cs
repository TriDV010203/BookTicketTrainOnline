using BookingTrain.Domain.Entities;

namespace BookingTrain.Application.Service
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync();
        Task<IEnumerable<Seat>> GetSeatsByTrainAsync(int trainId);
        Task<Seat?> GetSeatByIdAsync(int id);
        Task CreateSeatAsync(Seat seat);
        Task UpdateSeatAsync(int id, Seat seat);
        Task DeleteSeatAsync(int id);
    }
}
