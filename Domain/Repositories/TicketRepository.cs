using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;
using BookingTrain.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingTrain.Infrastructure.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Hàm lấy chi tiết vé bao gồm thông tin Người dùng, Chuyến tàu và Ghế
        public async Task<Ticket?> GetTicketDetailsAsync(int ticketId)
        {
            return await _context.Tickets
                .Include(t => t.User)           // Lấy thông tin khách hàng
                .Include(t => t.Seat)           // Lấy thông tin ghế
                .Include(t => t.Schedule)       // Lấy lịch trình
                    .ThenInclude(s => s.Route)  // Từ lịch trình lấy ra tuyến đường (Ga đi/Ga đến)
                        .ThenInclude(r => r.FromStation)
                .Include(t => t.Schedule)
                    .ThenInclude(s => s.Route)
                        .ThenInclude(r => r.ToStation)
                .Include(t => t.Payment)        // Lấy thông tin thanh toán
                .FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        // Hàm lấy danh sách tất cả vé của một người dùng cụ thể
        public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.Schedule)
                    .ThenInclude(s => s.Route)
                .OrderByDescending(t => t.Id) // Vé mới nhất lên đầu
                .ToListAsync();
        }
    }
}