using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SeatService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
            => await _unitOfWork.Seats.GetAllAsync();

        public async Task<IEnumerable<Seat>> GetSeatsByTrainAsync(int trainId)
        {
            var seats = await _unitOfWork.Seats.GetAllAsync();
            return seats.Where(s => s.TrainId == trainId);
        }

        public async Task<Seat?> GetSeatByIdAsync(int id)
            => await _unitOfWork.Seats.GetByIdAsync(id);

        public async Task CreateSeatAsync(Seat seat)
        {
            await _unitOfWork.Seats.AddAsync(seat);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateSeatAsync(int id, Seat seat)
        {
            var existing = await _unitOfWork.Seats.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy ghế này");
            existing.SeatNumber  = seat.SeatNumber;
            existing.CoachNumber = seat.CoachNumber;
            existing.TrainId     = seat.TrainId;
            existing.SeatTypeId  = seat.SeatTypeId;
            _unitOfWork.Seats.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteSeatAsync(int id)
        {
            var seat = await _unitOfWork.Seats.GetByIdAsync(id);
            if (seat != null)
            {
                _unitOfWork.Seats.Delete(seat);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
