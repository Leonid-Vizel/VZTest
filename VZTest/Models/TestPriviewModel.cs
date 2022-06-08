﻿using VZTest.Models.Test;

namespace VZTest.Models
{
    public class TestPriviewModel
    {
        public Test.Test Test { get; set; }
        public int TotalAttempts { get; set; }
        public bool NotFound { get; set; }
        public bool Forbidden { get; set; }
        public IEnumerable<Attempt> UserAttempts { get; set; }
    }
}