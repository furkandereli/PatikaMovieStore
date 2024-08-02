using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.DtoLayer.AuthDtos;

namespace PatikaMovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginDto loginDto)
        {
            var token = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (token == null)
                return Unauthorized("Invalid credentials");

            var response = new ResponseTokenDto
            {
                Token = token
            };

            return Ok(response);
        }
    }
}
