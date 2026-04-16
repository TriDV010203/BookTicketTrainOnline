using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Enums;
using BookingTrain.Domain.Interfaces;

namespace BookingTrain.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
            => await _unitOfWork.Payments.GetAllAsync();

        public async Task<Payment?> GetPaymentByIdAsync(int id)
            => await _unitOfWork.Payments.GetByIdAsync(id);

        public async Task<Payment?> GetPaymentByTicketAsync(int ticketId)
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            return payments.FirstOrDefault(p => p.TicketId == ticketId);
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePaymentStatusAsync(int id, PaymentStatus status)
        {
            var existing = await _unitOfWork.Payments.GetByIdAsync(id)
                           ?? throw new Exception("Không tìm thấy thanh toán này");
            existing.Status = status;
            _unitOfWork.Payments.Update(existing);
            await _unitOfWork.CompleteAsync();
        }
    }
}
