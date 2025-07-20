using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeCraftAPI.Models.DTOs;
using CodeCraftAPI.Services;

namespace CodeCraftAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;
        
        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProgress()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var progress = await _progressService.GetUserProgress(Guid.Parse(userId!));
            return Ok(new { success = true, data = progress });
        }
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProgress(UpdateProgressDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _progressService.UpdateProgress(Guid.Parse(userId!), dto.TopicName, dto.IsCompleted);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}