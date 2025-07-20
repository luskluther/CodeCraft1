using System;
using System.Collections.Generic;

namespace CodeCraftAPI.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public List<ProgressItemDto> Progress { get; set; } = new List<ProgressItemDto>();
        public int OverallProgressPercentage { get; set; }
    }
}