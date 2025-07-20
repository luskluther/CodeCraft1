using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CodeCraftAPI.Data;
using CodeCraftAPI.Models.DTOs;

namespace CodeCraftAPI.Services
{
    public class ProgressService : IProgressService
    {
        private readonly AppDbContext _context;
        
        public ProgressService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<ProgressItemDto>> GetUserProgress(Guid userId)
        {
            var progress = await _context.UserProgress
                .Include(p => p.Topic)
                .Where(p => p.UserId == userId)
                .Select(p => new ProgressItemDto
                {
                    TopicName = p.Topic.Name,
                    IsCompleted = p.IsCompleted
                })
                .ToListAsync();
                
            return progress;
        }
        
        public async Task UpdateProgress(Guid userId, string topicName, bool isCompleted)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Name == topicName);
            if (topic == null)
                throw new Exception("Topic not found");
                
            var progress = await _context.UserProgress
                .FirstOrDefaultAsync(p => p.UserId == userId && p.TopicId == topic.Id);
                
            if (progress == null)
            {
                progress = new Models.Entities.UserProgress
                {
                    UserId = userId,
                    TopicId = topic.Id,
                    IsCompleted = isCompleted,
                    CompletedAt = isCompleted ? DateTime.UtcNow : null
                };
                _context.UserProgress.Add(progress);
            }
            else
            {
                progress.IsCompleted = isCompleted;
                progress.CompletedAt = isCompleted ? DateTime.UtcNow : null;
            }
            
            await _context.SaveChangesAsync();
        }
    }
}