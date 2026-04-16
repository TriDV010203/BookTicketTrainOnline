using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketsController(ITicketService ticketService) => _ticketService = ticketService;

        // GET: api/tickets
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _ticketService.GetAllTicketsAsync());

        // GET: api/tickets/by-user/5
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
            => Ok(await _ticketService.GetTicketsByUserAsync(userId));

        // GET: api/tickets/5 — chi tiết đầy đủ (có Include)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _ticketService.GetTicketDetailsAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            await _ticketService.CreateTicketAsync(ticket);
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }

        // PATCH: api/tickets/5/status — cập nhật trạng thái vé
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatus status)
        {
            try { await _ticketService.UpdateTicketStatusAsync(id, status); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }

        // DELETE: api/tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return NoContent();
        }
    }
}
