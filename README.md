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

### Prerequisites

- .NET 10 SDK
- SQLite is included through the application database file

### Run Locally

1. Clone the repository.
2. Open the solution file: `c# project.sln`.
3. Update the connection string in `FeedbackSystem/appsettings.json` if needed.
4. Set `AdminAccess:AccessKey` locally before using the admin login page. Do not commit the real key to the repository; keep it in your local config or environment.
5. Restore and run the project:

```bash
dotnet restore
dotnet build
dotnet run --project FeedbackSystem/FeedbackSystem.csproj
```

6. Open the app in your browser using the URL shown in the terminal.

The app starts on the default Feedback index page.

## Notes

- The database is created and migrated automatically on startup.
- Feedback data is stored in `feedbacksystem.db`.
- The default route points to the feedback list so the application is ready to use immediately.
- Admin features require the local `AdminAccess:AccessKey` value to match the key you enter on the admin login page.

## Admin Access

- Admin login page: `/Feedback/AdminLogin`
- Admin dashboard: `/Feedback/Admin`
- Set `AdminAccess:AccessKey` in `FeedbackSystem/appsettings.json` before logging in.
- After running the app, open the local URL shown in the terminal, then navigate to the admin login page.
