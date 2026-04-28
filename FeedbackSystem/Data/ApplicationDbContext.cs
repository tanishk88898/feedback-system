using FeedbackSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<FeedbackEntry> FeedbackEntries => Set<FeedbackEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FeedbackEntry>()
            .Property(f => f.SubmittedAtUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<FeedbackEntry>().HasData(
            new FeedbackEntry
            {
                Id = 1,
                Name = "Aarav Sharma",
                Email = "aarav.sharma@example.com",
                Rating = 5,
                Comments = "Great experience. The UI is smooth and easy to use.",
                SubmittedAtUtc = new DateTime(2026, 4, 20, 9, 30, 0, DateTimeKind.Utc)
            },
            new FeedbackEntry
            {
                Id = 2,
                Name = "Priya Nair",
                Email = "priya.nair@example.com",
                Rating = 4,
                Comments = "Form validation works well, and submission is fast.",
                SubmittedAtUtc = new DateTime(2026, 4, 21, 14, 10, 0, DateTimeKind.Utc)
            },
            new FeedbackEntry
            {
                Id = 3,
                Name = "Rohit Verma",
                Email = "rohit.verma@example.com",
                Rating = 3,
                Comments = "Overall good. Would love a dark mode option in future.",
                SubmittedAtUtc = new DateTime(2026, 4, 22, 18, 45, 0, DateTimeKind.Utc)
            });
    }
}