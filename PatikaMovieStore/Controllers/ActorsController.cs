using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.DtoLayer.ActorDtos;

namespace PatikaMovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActor()
        {
            var actors = await _actorService.GetAllActorAsync();
            return Ok(actors);
        }

        [HttpGet("GetAllActorWithMovieNames")]
        public async Task<IActionResult> GetAllActorWithMovieNames()
        {
            var actors = await _actorService.GetAllActorWithMovieNameAsync();
            return Ok(actors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActorById(int id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActor(CreateActorDto createActorDto)
        {
            await _actorService.CreateActorAsync(createActorDto);
            return Ok("Actor created successfully !");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateActor(UpdateActorDto updateActorDto)
        {
            await _actorService.UpdateActorAsync(updateActorDto);
            return Ok("Actor updated successfully !");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            await _actorService.DeleteActorAsync(id);
            return Ok("Actor deleted successfully !");
        }
    }
}
