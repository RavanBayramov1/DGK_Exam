using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExamSystem.Data;

public class AddDbContext : DbContext
{
    public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Əlaqələr
        modelBuilder.Entity<AppUser>()
            .HasOne(u => u.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(u => u.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.TaughtGroups)
            .WithMany(g => g.Teachers)
            .UsingEntity(j => j.ToTable("TeacherGroups"));

        modelBuilder.Entity<Group>()
            .HasMany(g => g.Subjects)
            .WithMany(s => s.Groups)
            .UsingEntity(j => j.ToTable("GroupSubjects"));

        modelBuilder.Entity<ExamQuestion>().HasKey(eq => eq.Id);
        modelBuilder.Entity<ExamQuestion>()
            .HasIndex(eq => new { eq.ExamId, eq.QuestionId }).IsUnique();
        modelBuilder.Entity<ExamQuestion>()
            .HasOne(eq => eq.Exam).WithMany(e => e.ExamQuestions)
            .HasForeignKey(eq => eq.ExamId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ExamQuestion>()
            .HasOne(eq => eq.Question).WithMany(q => q.ExamQuestions)
            .HasForeignKey(eq => eq.QuestionId).OnDelete(DeleteBehavior.Cascade);

        // Soft Delete
        modelBuilder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Group>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Subject>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Question>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Exam>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ExamResult>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ExamQuestion>().HasQueryFilter(x => !x.IsDeleted);
    }
}


