using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public string Message { get; set; } = "";
    public string Avatar { get; set; } = "";
    public DateTime Timestamp { get; set; }
    public string? ImageUrl { get; set; } = "";
    
    [NotMapped]
    public long TimestampUnix => new DateTimeOffset(Timestamp).ToUnixTimeMilliseconds(); // Not mapped to DB

}