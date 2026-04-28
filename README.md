# Feedback System

A .NET MVC web application for collecting, browsing, and managing customer feedback. The app uses SQLite for persistence and includes a simple admin flow for reviewing, editing, and deleting submissions.

## Features

- Submit feedback with a name, email, rating, and comments
- Browse feedback with search and rating filtering
- View average rating from submitted entries
- Admin login protected by a session-based access key
- Edit and delete feedback entries from the admin area

## Tech Stack

- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- Bootstrap

## Getting Started

1. Install the .NET 10 SDK.
2. Open the solution file: `c# project.sln`.
3. Update the connection string and admin access key in `FeedbackSystem/appsettings.json` if needed.
4. Run the application:

```bash
dotnet run --project FeedbackSystem/FeedbackSystem.csproj
```

The app starts on the default Feedback index page.

## Notes

- The database is created and migrated automatically on startup.
- Feedback data is stored in `feedbacksystem.db`.
- The default route points to the feedback list so the application is ready to use immediately.
