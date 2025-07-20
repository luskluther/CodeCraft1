using System.Collections.Generic;

namespace CodeCraftAPI.Models.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Intro { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? DiagramData { get; set; }
        public ICollection<UserProgress> UserProgress { get; set; } = new List<UserProgress>();
    }
}