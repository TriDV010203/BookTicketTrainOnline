using BookingTrain.Domain.Entities;

namespace BookingTrain.Application.Service
{
    public interface ISeatTypeService
    {
        Task<IEnumerable<SeatType>> GetAllSeatTypesAsync();
        Task<SeatType?> GetSeatTypeByIdAsync(int id);
        Task CreateSeatTypeAsync(SeatType seatType);
        Task UpdateSeatTypeAsync(int id, SeatType seatType);
        Task DeleteSeatTypeAsync(int id);
    }
}
