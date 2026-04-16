using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Enums;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
            => await _unitOfWork.Tickets.GetAllAsync();

        public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId)
            => await _unitOfWork.Tickets.GetTicketsByUserAsync(userId);

        public async Task<Ticket?> GetTicketDetailsAsync(int id)
            => await _unitOfWork.Tickets.GetTicketDetailsAsync(id);

        public async Task CreateTicketAsync(Ticket ticket)
        {
            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateTicketStatusAsync(int id, TicketStatus status)
        {
            var existing = await _unitOfWork.Tickets.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy vé này");
            existing.Status = status;
            _unitOfWork.Tickets.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket != null)
            {
                _unitOfWork.Tickets.Delete(ticket);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
