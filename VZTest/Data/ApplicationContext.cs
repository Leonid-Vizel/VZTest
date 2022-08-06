using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;

namespace VZTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { Database.EnsureCreated(); }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<CorrectAnswer> CorrectAnswers { get; set; }
        public DbSet<UserStar> UserStars { get; set; }
        //TPH for CorrectAnswer
        public DbSet<CorrectTextAnswer> CorrectTextAnswers { get; set; }
        public DbSet<CorrectIntAnswer> CorrectIntAnswers { get; set; }
        public DbSet<CorrectDoubleAnswer> CorrectDoubleAnswers { get; set; }
        public DbSet<CorrectDateAnswer> CorrectDateAnswers { get; set; }
        public DbSet<CorrectCheckAnswer> CorrectCheckAnswers { get; set; }
    }
}
