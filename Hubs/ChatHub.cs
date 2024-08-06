using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;


public class ChatHub : Hub
{
    private readonly ChatContext _context;

    public ChatHub(ChatContext context)
    {
        _context = context;
    }
    public async Task SendMessage(string user, string message, string avatar,string? imageUrl)
    {
        var chatMessage = new ChatMessage
        {
            User = user,
            Message = message,
            Avatar = avatar,
            Timestamp = DateTime.UtcNow, // Set the timestamp on the server
            ImageUrl = imageUrl // Add this line
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", user, message, avatar, chatMessage.TimestampUnix, imageUrl);
    }
}