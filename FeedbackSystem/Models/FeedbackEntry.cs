using System.ComponentModel.DataAnnotations;

namespace FeedbackSystem.Models;

public class FeedbackEntry
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [Required]
    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;

    // Stored in UTC and shown in local time on the UI.
    public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;
}