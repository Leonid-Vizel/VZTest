using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test;

public class UserStar
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int TestId { get; set; }
}