using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.AuthDtos;
using PatikaMovieStore.DtoLayer.CustomerDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly MovieStoreContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(MovieStoreContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<TokenResponseDto> AuthenticateAsync(string email, string password)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == email && c.Password == password);
            if (customer == null)
                return null;

            var customerDto = _mapper.Map<CustomerDto>(customer);

            var token = GenerateToken(customerDto);

            return new TokenResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }

        private string GenerateToken(CustomerDto customerDto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customerDto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, customerDto.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
