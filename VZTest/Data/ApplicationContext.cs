using Microsoft.EntityFrameworkCore;
using VZTest.Models.Test;
using VZTest.Models.Test.Answers;

namespace VZTest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        //Implementing Table Per Hierarchy (TPH) EF Inheritance Pattern
        public DbSet<DateAnswer> DateAnswers { get; set; }
        public DbSet<IntAnswer> IntAnswers { get; set; }
        public DbSet<DoubleAnswer> DoubleAnswers { get; set; }
        public DbSet<TextAnswer> TextAnswers { get; set; }
        public DbSet<RadioAnswer> RadioAnswers { get; set; }
        public DbSet<DateAnswerOptional> DateAnswerOptionals { get; set; }
        public DbSet<IntAnswerOptional> IntAnswerOptionals { get; set; }
        public DbSet<DoubleAnswerOptional> DoubleAnswerOptionals { get; set; }
        public DbSet<TextAnswerOptional> TextAnswerOptionals { get; set; }
        public DbSet<RadioAnswerOptional> RadioAnswerOptionals { get; set; }
    }
}
