using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class TrainService : ITrainService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Train>> GetAllTrainsAsync()
        {
            return await _unitOfWork.Trains.GetAllAsync();
        }
        public async Task<Train?> GetTrainByIdAsync(int id)
        {
            return await _unitOfWork.Trains.GetByIdAsync(id);
        }
        public async Task CreateTrainAsync(Train train)
        {
            await _unitOfWork.Trains.AddAsync(train);
            await _unitOfWork.CompleteAsync();
        }
        public async Task UpdateTrainAsync(int id ,Train train)
        {
            var existingTrain = await _unitOfWork.Trains.GetByIdAsync(id);
            if (existingTrain == null)
                throw new Exception("Không tìm thấy tàu này");
            existingTrain.Name = train.Name;
            existingTrain.Type = train.Type;
            existingTrain.Capacity = train.Capacity;

            _unitOfWork.Trains.Update(train);
            await _unitOfWork.CompleteAsync();
        }
        public async Task DeleteTrainAsync(int id)
        {
            var train = await _unitOfWork.Trains.GetByIdAsync(id);
            if (train != null)
            {
                _unitOfWork.Trains.Delete(train);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
