using System;

namespace CodeCraftAPI.Models.Entities
{
    public class UserProgress
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public int TopicId { get; set; }
        public Topic Topic { get; set; } = null!;
        
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}