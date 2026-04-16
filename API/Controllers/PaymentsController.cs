using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

        // GET: api/payments
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _paymentService.GetAllPaymentsAsync());

        // GET: api/payments/by-ticket/5
        [HttpGet("by-ticket/{ticketId}")]
        public async Task<IActionResult> GetByTicket(int ticketId)
        {
            var item = await _paymentService.GetPaymentByTicketAsync(ticketId);
            return item == null ? NotFound() : Ok(item);
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _paymentService.GetPaymentByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/payments
        [HttpPost]
        public async Task<IActionResult> Create(Payment payment)
        {
            await _paymentService.CreatePaymentAsync(payment);
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }

        // PATCH: api/payments/5/status — cập nhật trạng thái thanh toán
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PaymentStatus status)
        {
            try { await _paymentService.UpdatePaymentStatusAsync(id, status); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }
    }
}
