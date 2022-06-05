﻿using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.Answers
{
    public class DateAnswer : Answer
    {
        [Required(ErrorMessage = "Укажите ответ на данный вопрос")]
        public double Answer { get; set; }
    }
}
