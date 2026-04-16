using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using TrainRoute = BookingTrain.Domain.Entities.Route;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;
        public RoutesController(IRouteService routeService) => _routeService = routeService;

        // GET: api/routes
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _routeService.GetAllRoutesAsync());

        // GET: api/routes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _routeService.GetRouteByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/routes
        [HttpPost]
        public async Task<IActionResult> Create(TrainRoute route)
        {
            await _routeService.CreateRouteAsync(route);
            return CreatedAtAction(nameof(GetById), new { id = route.Id }, route);
        }

        // PUT: api/routes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TrainRoute route)
        {
            if (id != route.Id) return BadRequest();
            try { await _routeService.UpdateRouteAsync(id, route); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }

        // DELETE: api/routes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _routeService.DeleteRouteAsync(id);
            return NoContent();
        }
    }
}
