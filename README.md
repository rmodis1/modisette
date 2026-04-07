# Modisette.com

Professional website and lightweight content management platform for a university professor, built with ASP.NET Core Razor Pages.

Live site: https://vickimodisette.com

## Overview

Modisette.com combines a polished public-facing site with an authenticated admin experience for managing course content, uploaded files, and incoming contact submissions. The project was built to support real deployment and maintenance concerns, including background email processing, configuration validation, Docker-based deployment, and database-provider flexibility between local and hosted environments.

## Highlights

- Public pages for profile, research, teaching content, and contact flows
- Authenticated admin dashboard for managing courses, content, and contact submissions
- Queue-backed background email notifications for a fast contact form experience
- Configuration-driven support for SQLite locally and PostgreSQL in deployment
- Cookie-based admin authentication with fail-fast configuration validation
- Docker and Render deployment workflow with custom domain support

## Gallery

![Project Screenshot](Images/modisette.com.png)
![Project Screenshot](Images/About.png)
![Project Screenshot](Images/UserCourseContent.png)
![Project Screenshot](Images/AdminContactDashboard.png)
![Project Screenshot](Images/AdminCoursesDisplay.png)
![Project Screenshot](Images/AdminEditCourse.png)

## Architecture Notes

- Razor Pages UI backed by a service-oriented application layer
- EF Core data access across courses, documents, and contact records
- Background email queue via hosted service to keep requests responsive
- Startup-time options validation for critical application settings
- Design-time DbContext factory support for EF tooling and migrations

## Tech Stack

- ASP.NET Core Razor Pages on .NET 8
- C#
- Entity Framework Core
- SQLite for local development
- PostgreSQL for deployed environments
- Resend for transactional email delivery
- Bootstrap, custom CSS, and JavaScript
- Docker and Render for deployment

## Local Development

1. Clone the repository.
2. Restore packages with `dotnet restore`.
3. Configure the required application secrets.
4. Apply the database with `dotnet ef database update`.
5. Start the app with `dotnet run`.

Required settings for local development:

- `AdminAuth:Username`
- `AdminAuth:Password` or `AdminAuth:PasswordHash`
- `Database:Provider`
- `ConnectionStrings:SiteContext` or `ConnectionStrings:Postgres`
- `EmailConfiguration:From`
- `EmailConfiguration:ResendApiKey`
- `SiteEmailAddress:Name`
- `SiteEmailAddress:Address`

Checked-in configuration is placeholder-only. Real values should come from user secrets or environment variables.

## Deployment Notes

- Production deployment targets Render with Docker
- Production data can run on PostgreSQL while local development stays on SQLite
- Contact notifications use Resend instead of direct SMTP for more reliable hosted delivery
- ASP.NET Core data-protection keys can be persisted to a mounted disk for stable auth and antiforgery behavior across redeploys

## Quality Checks

```sh
dotnet build --configuration Release
dotnet test modisette.sln --configuration Release
```

## Repository Notes

Additional implementation notes for the PostgreSQL rollout are documented in [docs/postgres-migration.md](docs/postgres-migration.md).

## Contact

For project inquiries, contact [modisetteryan@gmail.com](mailto:modisetteryan@gmail.com).
