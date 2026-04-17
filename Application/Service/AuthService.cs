using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingTrain.Application.DTOs;
using BookingTrain.Domain.Entities;
using BookingTrain.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookingTrain.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            if (allUsers.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Email này đã được sử dụng.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Name     = request.Name,
                Email    = request.Email,
                Password = hashedPassword,
                Role     = "Customer"
            };

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();

            return GenerateToken(newUser);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var user = allUsers.FirstOrDefault(u =>
                u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isValidPassword)
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");

            return GenerateToken(user);
        }

        private AuthResponse GenerateToken(User user)
        {
            var jwtSettings  = _configuration.GetSection("JwtSettings");
            var secretKey    = jwtSettings["SecretKey"]!;
            var issuer       = jwtSettings["Issuer"]!;
            var audience     = jwtSettings["Audience"]!;
            var expMinutes   = int.Parse(jwtSettings["ExpirationMinutes"]!);

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name,               user.Name),
                new Claim(ClaimTypes.Role,               user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(expMinutes);

            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            expiresAt,
                signingCredentials: creds
            );

            return new AuthResponse
            {
                Token     = new JwtSecurityTokenHandler().WriteToken(token),
                Name      = user.Name,
                Email     = user.Email,
                Role      = user.Role,
                ExpiresAt = expiresAt
            };
        }
    }
}
