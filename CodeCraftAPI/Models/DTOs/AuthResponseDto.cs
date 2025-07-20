namespace CodeCraftAPI.Models.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }
}