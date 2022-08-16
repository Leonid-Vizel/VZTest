﻿using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test;

public abstract class CorrectAnswer
{
    [Key]
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Discriminator { get; set; }
}