using System.Threading.Tasks;
using CodeCraftAPI.Models.DTOs;

namespace CodeCraftAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto dto);
        Task<AuthResponseDto> Login(LoginDto dto);
    }
}