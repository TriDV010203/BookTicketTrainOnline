using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Entities;

namespace BookingTrain.Application.Service
{
    public interface ITrainService
    {
        Task<IEnumerable<Train>> GetAllTrainsAsync();
        Task<Train?> GetTrainByIdAsync(int id);
        Task CreateTrainAsync(Train train);
        Task UpdateTrainAsync(int id,Train train);
        Task DeleteTrainAsync(int id);
    }
}
