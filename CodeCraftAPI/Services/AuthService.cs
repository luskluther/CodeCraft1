using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CodeCraftAPI.Data;
using CodeCraftAPI.Models.DTOs;
using CodeCraftAPI.Models.Entities;

namespace CodeCraftAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            // Check if user exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email || u.Username == dto.Username))
                throw new Exception("User already exists");
                
            // Create user
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Users.Add(user);
            
            // Initialize progress for all topics
            var topics = await _context.Topics.ToListAsync();
            foreach (var topic in topics)
            {
                _context.UserProgress.Add(new UserProgress
                {
                    UserId = user.Id,
                    TopicId = topic.Id,
                    IsCompleted = false
                });
            }
            
            await _context.SaveChangesAsync();
            
            // Load progress for response
            user.Progress = await _context.UserProgress
                .Include(p => p.Topic)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            
            // Generate token
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token,
                User = MapToUserDto(user)
            };
        }
        
        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Progress)
                .ThenInclude(p => p.Topic)
                .FirstOrDefaultAsync(u => u.Email == dto.EmailOrUsername || u.Username == dto.EmailOrUsername);
                
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");
                
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token,
                User = MapToUserDto(user)
            };
        }
        
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        private UserDto MapToUserDto(User user)
        {
            var completedCount = user.Progress?.Count(p => p.IsCompleted) ?? 0;
            var totalCount = user.Progress?.Count ?? 0;
            
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.Username,
                Avatar = user.Avatar,
                Progress = user.Progress?.Select(p => new ProgressItemDto
                {
                    TopicName = p.Topic.Name,
                    IsCompleted = p.IsCompleted
                }).ToList() ?? new List<ProgressItemDto>(),
                OverallProgressPercentage = totalCount > 0 ? (completedCount * 100 / totalCount) : 0
            };
        }
    }
}