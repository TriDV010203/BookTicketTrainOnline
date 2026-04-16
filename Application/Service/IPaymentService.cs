using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Enums;

namespace BookingTrain.Application.Service
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<Payment?> GetPaymentByTicketAsync(int ticketId);
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentStatusAsync(int id, PaymentStatus status);
    }
}
