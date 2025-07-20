using System.ComponentModel.DataAnnotations;

namespace CodeCraftAPI.Models.DTOs
{
    public class UpdateProgressDto
    {
        [Required]
        public string TopicName { get; set; } = string.Empty;
        
        [Required]
        public bool IsCompleted { get; set; }
    }
}