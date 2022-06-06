using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace VZTest.Models.Test
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public string? UserId { get; set; }
        public bool Opened { get; set; }
        public bool Public { get; set; }
        public byte[]? PasswordHash { get; set; }
        public DateTime CreatedTime { get; set; }
        [NotMapped]
        public IEnumerable<Question> Questions { get; set; }
    }
}
