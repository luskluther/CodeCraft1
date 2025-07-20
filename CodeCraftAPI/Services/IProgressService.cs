using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeCraftAPI.Models.DTOs;

namespace CodeCraftAPI.Services
{
    public interface IProgressService
    {
        Task<List<ProgressItemDto>> GetUserProgress(Guid userId);
        Task UpdateProgress(Guid userId, string topicName, bool isCompleted);
    }
}