using BookingTrain.Domain.Interfaces;
using BookingTrain.Domain.Entities;
using BookingTrain.Infrastructure.Persistence;

namespace BookingTrain.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Tickets   = new TicketRepository(_context);
            Users     = new GenericRepository<User>(_context);
            Trains    = new GenericRepository<Train>(_context);
            Stations  = new GenericRepository<Station>(_context);
            SeatTypes = new GenericRepository<SeatType>(_context);
            Seats     = new GenericRepository<Seat>(_context);
            Routes    = new GenericRepository<Route>(_context);
            Schedules = new GenericRepository<Schedule>(_context);
            Payments  = new GenericRepository<Payment>(_context);
        }

        public ITicketRepository             Tickets   { get; private set; }
        public IGenericRepository<User>      Users     { get; private set; }
        public IGenericRepository<Train>     Trains    { get; private set; }
        public IGenericRepository<Station>   Stations  { get; private set; }
        public IGenericRepository<SeatType>  SeatTypes { get; private set; }
        public IGenericRepository<Seat>      Seats     { get; private set; }
        public IGenericRepository<Route>     Routes    { get; private set; }
        public IGenericRepository<Schedule>  Schedules { get; private set; }
        public IGenericRepository<Payment>   Payments  { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
