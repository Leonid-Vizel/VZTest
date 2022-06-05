﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public bool Opened { get; set; }
        public DateTime CreatedTime { get; set; }
        [NotMapped]
        public List<Question> Questions { get; set; }
    }
}
