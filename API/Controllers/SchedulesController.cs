using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public SchedulesController(IScheduleService scheduleService) => _scheduleService = scheduleService;

        // GET: api/schedules
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _scheduleService.GetAllSchedulesAsync());

        // GET: api/schedules/by-route/5
        [HttpGet("by-route/{routeId}")]
        public async Task<IActionResult> GetByRoute(int routeId)
            => Ok(await _scheduleService.GetSchedulesByRouteAsync(routeId));

        // GET: api/schedules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _scheduleService.GetScheduleByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // POST: api/schedules
        [HttpPost]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            await _scheduleService.CreateScheduleAsync(schedule);
            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
        }

        // PUT: api/schedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Schedule schedule)
        {
            if (id != schedule.Id) return BadRequest();
            try { await _scheduleService.UpdateScheduleAsync(id, schedule); }
            catch (Exception ex) { return NotFound(ex.Message); }
            return NoContent();
        }

        // DELETE: api/schedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }
    }
}
