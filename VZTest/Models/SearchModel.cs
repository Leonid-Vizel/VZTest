namespace VZTest.Models
{
    public class SearchModel
    {
        public int SearchId { get; set; }
        public bool PasswordNeeded { get; set; }
        public string? PasswordHash { get; set; }
    }
}
