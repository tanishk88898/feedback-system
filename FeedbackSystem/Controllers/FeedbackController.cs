using FeedbackSystem.Data;
using FeedbackSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Controllers;

public class FeedbackController(ApplicationDbContext dbContext, IConfiguration configuration) : Controller
{
    private const string AdminSessionKey = "IsAdmin";

    // Public list view: users can browse submitted feedback with filters.
    public async Task<IActionResult> Index(string? searchTerm, int? ratingFilter)
    {
        IQueryable<FeedbackEntry> query = dbContext.FeedbackEntries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(f =>
                f.Name.Contains(searchTerm) ||
                f.Email.Contains(searchTerm) ||
                f.Comments.Contains(searchTerm));
        }

        if (ratingFilter is >= 1 and <= 5)
        {
            query = query.Where(f => f.Rating == ratingFilter);
        }

        List<FeedbackEntry> entries = await query
            .OrderByDescending(f => f.SubmittedAtUtc)
            .ToListAsync();

        FeedbackListViewModel viewModel = new()
        {
            FeedbackEntries = entries,
            SearchTerm = searchTerm,
            RatingFilter = ratingFilter,
            AverageRating = entries.Count == 0 ? 0 : entries.Average(f => f.Rating)
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new FeedbackEntry());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FeedbackEntry feedbackEntry)
    {
        if (!ModelState.IsValid)
        {
            return View(feedbackEntry);
        }

        feedbackEntry.SubmittedAtUtc = DateTime.UtcNow;

        dbContext.FeedbackEntries.Add(feedbackEntry);
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Feedback submitted successfully.";
        return RedirectToAction(nameof(Index));
    }

    // Simple admin page for management operations.
    public async Task<IActionResult> Admin(string? searchTerm, int? ratingFilter)
    {
        if (!IsAdminUser())
        {
            TempData["ErrorMessage"] = "Admin access required.";
            return RedirectToAction(nameof(AdminLogin));
        }

        return View("Index", await BuildListViewModel(searchTerm, ratingFilter));
    }

    [HttpGet]
    public IActionResult AdminLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdminLogin(string accessKey)
    {
        string configuredKey = configuration["AdminAccess:AccessKey"] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            ModelState.AddModelError(string.Empty, "Admin key is not configured.");
            return View();
        }

        if (!string.Equals(accessKey, configuredKey, StringComparison.Ordinal))
        {
            ModelState.AddModelError(string.Empty, "Invalid admin access key.");
            return View();
        }

        HttpContext.Session.SetString(AdminSessionKey, "true");
        TempData["SuccessMessage"] = "Admin mode enabled.";
        return RedirectToAction(nameof(Admin));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdminLogout()
    {
        HttpContext.Session.Remove(AdminSessionKey);
        TempData["SuccessMessage"] = "Admin mode disabled.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAdminUser())
        {
            TempData["ErrorMessage"] = "Admin access required.";
            return RedirectToAction(nameof(AdminLogin));
        }

        FeedbackEntry? feedbackEntry = await dbContext.FeedbackEntries.FindAsync(id);
        if (feedbackEntry is null)
        {
            return NotFound();
        }

        return View(feedbackEntry);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FeedbackEntry feedbackEntry)
    {
        if (!IsAdminUser())
        {
            TempData["ErrorMessage"] = "Admin access required.";
            return RedirectToAction(nameof(AdminLogin));
        }

        if (id != feedbackEntry.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(feedbackEntry);
        }

        FeedbackEntry? existing = await dbContext.FeedbackEntries.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = feedbackEntry.Name;
        existing.Email = feedbackEntry.Email;
        existing.Rating = feedbackEntry.Rating;
        existing.Comments = feedbackEntry.Comments;

        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Feedback updated successfully.";
        return RedirectToAction(nameof(Admin));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdminUser())
        {
            TempData["ErrorMessage"] = "Admin access required.";
            return RedirectToAction(nameof(AdminLogin));
        }

        FeedbackEntry? feedbackEntry = await dbContext.FeedbackEntries
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

        if (feedbackEntry is null)
        {
            return NotFound();
        }

        return View(feedbackEntry);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!IsAdminUser())
        {
            TempData["ErrorMessage"] = "Admin access required.";
            return RedirectToAction(nameof(AdminLogin));
        }

        FeedbackEntry? feedbackEntry = await dbContext.FeedbackEntries.FindAsync(id);
        if (feedbackEntry is null)
        {
            return NotFound();
        }

        dbContext.FeedbackEntries.Remove(feedbackEntry);
        await dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "Feedback deleted successfully.";
        return RedirectToAction(nameof(Admin));
    }

    private async Task<FeedbackListViewModel> BuildListViewModel(string? searchTerm, int? ratingFilter)
    {
        IQueryable<FeedbackEntry> query = dbContext.FeedbackEntries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(f =>
                f.Name.Contains(searchTerm) ||
                f.Email.Contains(searchTerm) ||
                f.Comments.Contains(searchTerm));
        }

        if (ratingFilter is >= 1 and <= 5)
        {
            query = query.Where(f => f.Rating == ratingFilter);
        }

        List<FeedbackEntry> entries = await query
            .OrderByDescending(f => f.SubmittedAtUtc)
            .ToListAsync();

        return new FeedbackListViewModel
        {
            FeedbackEntries = entries,
            SearchTerm = searchTerm,
            RatingFilter = ratingFilter,
            AverageRating = entries.Count == 0 ? 0 : entries.Average(f => f.Rating)
        };
    }

    private bool IsAdminUser()
    {
        return string.Equals(HttpContext.Session.GetString(AdminSessionKey), "true", StringComparison.Ordinal);
    }
}