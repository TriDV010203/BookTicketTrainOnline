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
            // Khởi tạo các Repo
            Tickets = new TicketRepository(_context);
            Trains = new GenericRepository<Train>(_context);
            Stations = new GenericRepository<Station>(_context);
        }

        public ITicketRepository Tickets { get; private set; }
        public IGenericRepository<Train> Trains { get; private set; }
        public IGenericRepository<Station> Stations { get; private set; }

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