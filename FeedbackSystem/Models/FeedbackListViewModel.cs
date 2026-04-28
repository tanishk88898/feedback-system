namespace FeedbackSystem.Models;

public class FeedbackListViewModel
{
    public IReadOnlyList<FeedbackEntry> FeedbackEntries { get; set; } = [];

    public string? SearchTerm { get; set; }

    public int? RatingFilter { get; set; }

    public double AverageRating { get; set; }
}