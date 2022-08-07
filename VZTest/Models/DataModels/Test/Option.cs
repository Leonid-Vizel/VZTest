using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test;

public class Option
{
    [Key]
    public int Id { get; set; }
    public int QuestionId { get; set; }
    [Required(ErrorMessage = "Укажите наименование опции")]
    public string Title { get; set; }

    public Option()
    {
        Title = string.Empty;
    }

    public Option(string title)
    {
        Title = title;
    }
}
