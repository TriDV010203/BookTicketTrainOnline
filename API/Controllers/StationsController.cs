using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;
        public StationsController(IStationService stationService) => _stationService = stationService;

        // GET: api/stations
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _stationService.GetAllStationsAsync());

        // GET: api/stations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _stationService.GetStationByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/stations
        [HttpPost]
        public async Task<IActionResult> Create(Station station)
        {
            await _stationService.CreateStationAsync(station);
            return CreatedAtAction(nameof(GetById), new { id = station.Id }, station);
        }

        // PUT: api/stations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Station station)
        {
            if (id != station.Id) return BadRequest();
            try { await _stationService.UpdateStationAsync(id, station); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }

        // DELETE: api/stations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _stationService.DeleteStationAsync(id);
            return NoContent();
        }
    }
}
