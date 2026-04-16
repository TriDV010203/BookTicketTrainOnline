using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class SeatTypeService : ISeatTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SeatTypeService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<SeatType>> GetAllSeatTypesAsync()
            => await _unitOfWork.SeatTypes.GetAllAsync();

        public async Task<SeatType?> GetSeatTypeByIdAsync(int id)
            => await _unitOfWork.SeatTypes.GetByIdAsync(id);

        public async Task CreateSeatTypeAsync(SeatType seatType)
        {
            await _unitOfWork.SeatTypes.AddAsync(seatType);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateSeatTypeAsync(int id, SeatType seatType)
        {
            var existing = await _unitOfWork.SeatTypes.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy loại ghế này");
            existing.Name = seatType.Name;
            existing.Description = seatType.Description;
            _unitOfWork.SeatTypes.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteSeatTypeAsync(int id)
        {
            var seatType = await _unitOfWork.SeatTypes.GetByIdAsync(id);
            if (seatType != null)
            {
                _unitOfWork.SeatTypes.Delete(seatType);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
