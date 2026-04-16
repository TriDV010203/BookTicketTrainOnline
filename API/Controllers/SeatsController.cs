using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        public SeatsController(ISeatService seatService) => _seatService = seatService;

        // GET: api/seats
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _seatService.GetAllSeatsAsync());

        // GET: api/seats/by-train/5
        [HttpGet("by-train/{trainId}")]
        public async Task<IActionResult> GetByTrain(int trainId)
            => Ok(await _seatService.GetSeatsByTrainAsync(trainId));

        // GET: api/seats/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _seatService.GetSeatByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/seats
        [HttpPost]
        public async Task<IActionResult> Create(Seat seat)
        {
            await _seatService.CreateSeatAsync(seat);
            return CreatedAtAction(nameof(GetById), new { id = seat.Id }, seat);
        }

        // PUT: api/seats/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Seat seat)
        {
            if (id != seat.Id) return BadRequest();
            try { await _seatService.UpdateSeatAsync(id, seat); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }

        // DELETE: api/seats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _seatService.DeleteSeatAsync(id);
            return NoContent();
        }
    }
}
