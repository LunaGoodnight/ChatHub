namespace ChatApp.Models;

public class ChatMessageDto
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public string Message { get; set; } = "";
    public string Avatar { get; set; } = "";
    public long Timestamp { get; set; }
    public string? ImageUrl { get; set; } = "";
    
    public DateTime TimeString  { get; set; }
}