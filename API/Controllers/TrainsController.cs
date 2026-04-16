using BookingTrain.Application.Service;
using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingTrain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly ITrainService trainService;
        public TrainsController(ITrainService trainService)
        {
            this.trainService = trainService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrains()
        {
            var trains = await trainService.GetAllTrainsAsync();
            return Ok(trains);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainById(int id)
        {
            var train = await trainService.GetTrainByIdAsync(id);
            if (train == null)
                return NotFound();
            return Ok(train);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrain(Train train)
        {
            await trainService.CreateTrainAsync(train);
            return CreatedAtAction(nameof(GetTrainById), new { id = train.Id }, train);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrain(int id)
        {
            await trainService.DeleteTrainAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrain(int id, Train train)
        {
            if (id != train.Id)
                return BadRequest();
            try
            {
                await trainService.UpdateTrainAsync(id, train);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
