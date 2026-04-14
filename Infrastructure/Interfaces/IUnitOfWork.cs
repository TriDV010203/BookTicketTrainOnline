using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Entities;

namespace BookingTrain.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITicketRepository Tickets { get; }
        IGenericRepository<Train> Trains { get; }
        IGenericRepository<Station> Stations { get; }
        Task<int> CompleteAsync();
    }
}
