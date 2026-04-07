# PostgreSQL Migration Plan

## Goal

Move production persistence from the local SQLite file to Supabase Postgres without breaking local development. The application code now supports both providers through configuration:

- `Database:Provider=sqlite` uses `ConnectionStrings:SiteContext`
- `Database:Provider=postgres` uses `ConnectionStrings:Postgres`

## Current State

- Local development defaults to SQLite.
- Existing EF Core migrations under `Migrations/` were generated against SQLite.
- Production target is Supabase Postgres.
- Uploaded files still live under `wwwroot/Uploads`, so storage cutover remains a separate task.

## Recommended Cutover Strategy

1. Provision the Supabase Postgres database and collect the SSL-required connection string.
2. Switch a local development environment to Postgres with `Database__Provider=postgres` and `ConnectionStrings__Postgres=...`.
3. Create a PostgreSQL baseline migration after deciding whether to keep the existing migration history or replace it with a fresh provider-neutral baseline.
4. Apply the Postgres migration to an empty Supabase database.
5. Export the current SQLite data and import it into Postgres.
6. Validate the app locally against Postgres before any deployment cutover.
7. Update the Render production environment to use the Postgres provider and connection string.
8. Deploy and verify CRUD, contact submissions, and admin login on the hosted site.

## Important Constraint

The current migrations include SQLite-specific annotations such as `Sqlite:Autoincrement`. That means the safest cutover is usually one of these approaches:

1. Create a fresh Postgres baseline migration once the model is stable.
2. Or maintain provider-specific migrations if you need both providers to evolve independently.

For this project, a fresh Postgres baseline is the lower-complexity path.

## Proposed Implementation Order

1. Keep SQLite as the default local provider.
2. Add Postgres provider support in application startup and design-time EF tooling.
3. Create the first Postgres baseline migration.
4. Add a small one-time data migration path from SQLite to Postgres.
5. Cut production over to Supabase.
6. Later, remove SQLite entirely if local file-based development is no longer useful.

## Validation Checklist

1. `dotnet build --configuration Release modisette.sln`
2. `dotnet test --configuration Release modisette.sln`
3. `dotnet ef dbcontext info` with SQLite settings
4. `dotnet ef dbcontext info` with Postgres settings
5. `dotnet ef database update` against a disposable Postgres database
6. Manual verification of contact form persistence, admin CRUD, and content reads