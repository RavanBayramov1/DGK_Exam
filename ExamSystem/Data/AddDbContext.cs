using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExamSystem.Data;

public class AddDbContext : DbContext
{
    public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();
        modelBuilder.Entity<Exam>()
            .HasOne(e => e.User)
            .WithMany(u => u.Exams)
            .HasForeignKey(e => e.UserId);
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Exam)
            .WithMany(e => e.Questions)
            .HasForeignKey(q => q.ExamId);
        modelBuilder.Entity<Result>()
           .HasOne(r => r.Exam)
           .WithMany(e => e.Results)
           .HasForeignKey(r => r.ExamId);
        // User → Result (student)
        modelBuilder.Entity<Result>()
            .HasOne(r => r.Student)
            .WithMany(u => u.Results)
            .HasForeignKey(r => r.StudentId);
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId);
        // Result → Answer
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Result)
            .WithMany(r => r.Answers)
            .HasForeignKey(a => a.ResultId);
    }
}
