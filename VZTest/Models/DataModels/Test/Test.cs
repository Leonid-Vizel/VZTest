using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VZTest.Models.ViewModels.Test;

namespace VZTest.Models.DataModels.Test;

public class Test
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Укажите название теста")]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string UserId { get; set; } = null!;
    [Required(ErrorMessage = "Укажите максимальное количество попыток")]
    public int MaxAttempts { get; set; }
    public bool Opened { get; set; }
    public bool Public { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? EditedTime { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool Shuffle { get; set; }
}