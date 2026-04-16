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
        IGenericRepository<User> Users { get; }
        IGenericRepository<Station> Stations { get; }
        IGenericRepository<SeatType> SeatTypes { get; }
        IGenericRepository<Seat> Seats { get; }
        IGenericRepository<Route> Routes { get; }
        IGenericRepository<Schedule> Schedules { get; }
        IGenericRepository<Payment> Payments { get; }
        Task<int> CompleteAsync();
    }
}
