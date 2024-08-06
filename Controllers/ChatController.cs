using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatContext _context;

        public ChatController(ChatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetMessages(int pageNumber = 1, int pageSize = 40)
        {
            var messages = await _context.ChatMessages
                .OrderByDescending(m => m.Timestamp) // Ensure messages are sorted by timestamp in descending order
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    User = m.User,
                    Message = m.Message,
                    Avatar = m.Avatar,
                    Timestamp = new DateTimeOffset(m.Timestamp).ToUnixTimeMilliseconds(),
                    TimeString = m.Timestamp,
                    ImageUrl = m.ImageUrl
                })
                .ToListAsync();

            return messages;
        }
    }
}