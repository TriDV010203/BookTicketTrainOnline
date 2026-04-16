using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatTypesController : ControllerBase
    {
        private readonly ISeatTypeService _seatTypeService;
        public SeatTypesController(ISeatTypeService seatTypeService) => _seatTypeService = seatTypeService;

        // GET: api/seattypes
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _seatTypeService.GetAllSeatTypesAsync());

        // GET: api/seattypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _seatTypeService.GetSeatTypeByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/seattypes
        [HttpPost]
        public async Task<IActionResult> Create(SeatType seatType)
        {
            await _seatTypeService.CreateSeatTypeAsync(seatType);
            return CreatedAtAction(nameof(GetById), new { id = seatType.Id }, seatType);
        }

        // PUT: api/seattypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SeatType seatType)
        {
            if (id != seatType.Id) return BadRequest();
            try { await _seatTypeService.UpdateSeatTypeAsync(id, seatType); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }

        // DELETE: api/seattypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _seatTypeService.DeleteSeatTypeAsync(id);
            return NoContent();
        }
    }
}
