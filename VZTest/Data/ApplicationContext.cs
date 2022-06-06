using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VZTest.Models.Test;
using VZTest.Models.Test.Answers;
using VZTest.Models.Test.CorrectAnswers;

namespace VZTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<CorrectAnswer> CorrectAnswers { get; set; }
        //Implementing Table Per Hierarchy (TPH) EF Inheritance Pattern for Answer
        public DbSet<DateAnswer> DateAnswers { get; set; }
        public DbSet<IntAnswer> IntAnswers { get; set; }
        public DbSet<DoubleAnswer> DoubleAnswers { get; set; }
        public DbSet<TextAnswer> TextAnswers { get; set; }
        public DbSet<RadioAnswer> RadioAnswers { get; set; }
        public DbSet<CheckAnswer> CheckAnswers { get; set; }
        public DbSet<DateAnswerOptional> DateAnswersOptional { get; set; }
        public DbSet<IntAnswerOptional> IntAnswersOptional { get; set; }
        public DbSet<DoubleAnswerOptional> DoubleAnswersOptional { get; set; }
        public DbSet<TextAnswerOptional> TextAnswersOptional { get; set; }
        public DbSet<RadioAnswerOptional> RadioAnswersOptional { get; set; }
        public DbSet<CheckAnswerOptional> CheckAnswersOptional { get; set; }
        //TPH for CorrectAnswer
        public DbSet<CorrectTextAnswer> CorrectTextAnswers { get; set; }
        public DbSet<CorrectIntAnswer> CorrectIntAnswers { get; set; }
        public DbSet<CorrectDoubleAnswer> CorrectDoubleAnswers { get; set; }
        public DbSet<CorrectDateAnswer> CorrectDateAnswers { get; set; }
    }
}
