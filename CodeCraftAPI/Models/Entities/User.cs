using System;
using System.Collections.Generic;

namespace CodeCraftAPI.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Avatar { get; set; } = "👤";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public ICollection<UserProgress> Progress { get; set; } = new List<UserProgress>();
    }
}