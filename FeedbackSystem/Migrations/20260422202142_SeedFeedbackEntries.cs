using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FeedbackSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedFeedbackEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FeedbackEntries",
                columns: new[] { "Id", "Comments", "Email", "Name", "Rating", "SubmittedAtUtc" },
                values: new object[,]
                {
                    { 1, "Great experience. The UI is smooth and easy to use.", "aarav.sharma@example.com", "Aarav Sharma", 5, new DateTime(2026, 4, 20, 9, 30, 0, 0, DateTimeKind.Utc) },
                    { 2, "Form validation works well, and submission is fast.", "priya.nair@example.com", "Priya Nair", 4, new DateTime(2026, 4, 21, 14, 10, 0, 0, DateTimeKind.Utc) },
                    { 3, "Overall good. Would love a dark mode option in future.", "rohit.verma@example.com", "Rohit Verma", 3, new DateTime(2026, 4, 22, 18, 45, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeedbackEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FeedbackEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FeedbackEntries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
