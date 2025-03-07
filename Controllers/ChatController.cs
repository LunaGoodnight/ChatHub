using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers;

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
    public async Task<ActionResult<IEnumerable<ChatMessage>>> GetMessages(int pageNumber = 1, int pageSize = 40)
    {
        var messages = await _context.ChatMessages
            .OrderByDescending(m => m.Timestamp) // Ensure messages are sorted by timestamp in descending order
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return messages;
    }
}