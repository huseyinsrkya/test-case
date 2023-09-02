using Microsoft.AspNetCore.Identity;

namespace NoteAPP.Entity;

public class NoteEntity
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime AddDate { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
}