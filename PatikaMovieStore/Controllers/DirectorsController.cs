using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.DtoLayer.DirectorDtos;

namespace PatikaMovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDirector()
        {
            var directors = await _directorService.GetAllDirectorAsync();
            return Ok(directors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectorById(int id)
        {
            var director = await _directorService.GetDirectorByIdAsync(id);
            return Ok(director);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirector(CreateDirectorDto createDirectorDto)
        {
            await _directorService.CreateDirectorAsync(createDirectorDto);
            return Ok("Director created successfully !");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDirector(UpdateDirectorDto updateDirectorDto)
        {
            await _directorService.UpdateDirectorAsync(updateDirectorDto);
            return Ok("Director updated successfully !");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            await _directorService.DeleteDirectorAsync(id);
            return Ok("Director deleted successfully !");
        }
    }
}
