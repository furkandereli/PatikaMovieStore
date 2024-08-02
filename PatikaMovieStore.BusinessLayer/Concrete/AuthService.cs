using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Token;
using PatikaMovieStore.DataAccessLayer.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly MovieStoreContext _context;
        private readonly JwtConfig _jwtConfig;
        private readonly IMapper _mapper;

        public AuthService(MovieStoreContext context, IOptions<JwtConfig> jwtConfig, IMapper mapper)
        {
            _context = context;
            _jwtConfig = jwtConfig.Value;
            _mapper = mapper;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == email && c.Password == password);

            if (customer == null)
                return null;

            // JWT Token creation
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Role, customer.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
