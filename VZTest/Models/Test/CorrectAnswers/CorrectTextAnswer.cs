﻿using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.CorrectAnswers
{
    public class CorrectTextAnswer : CorrectAnswer
    {
        [Required(ErrorMessage = "Укажите ответ на вопрос!")]
        public string Correct;
    }
}
