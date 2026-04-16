using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Enums;

namespace BookingTrain.Application.Service
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId);
        Task<Ticket?> GetTicketDetailsAsync(int id);
        Task CreateTicketAsync(Ticket ticket);
        Task UpdateTicketStatusAsync(int id, TicketStatus status);
        Task DeleteTicketAsync(int id);
    }
}
