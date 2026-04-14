using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTrain.Domain.Entities;

namespace BookingTrain.Domain.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<Ticket?> GetTicketDetailsAsync(int ticketId);
        Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId);
    }
}
