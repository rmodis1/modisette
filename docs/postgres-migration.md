# PostgreSQL Migration Plan

## Goal

Move production persistence from the local SQLite file to Supabase Postgres without breaking local development. The application code now supports both providers through configuration:

- `Database:Provider=sqlite` uses `ConnectionStrings:SiteContext`
- `Database:Provider=postgres` uses `ConnectionStrings:Postgres`

## Current State

- Local development defaults to SQLite.
- Existing EF Core migrations under `Migrations/` were generated against SQLite.
- Production target is Supabase Postgres.
- Initial connectivity should use the Supabase session pooler because it supports IPv4 for both local development and first deployment.
- Uploaded files still live under `wwwroot/Uploads`, so storage cutover remains a separate task.

## Recommended Cutover Strategy

1. Provision the Supabase Postgres database and collect the session pooler connection string for initial rollout.
2. Switch a local development environment to Postgres with `Database__Provider=postgres` and `ConnectionStrings__Postgres=...` using the session pooler.
3. Create a PostgreSQL baseline migration in the dedicated `Modisette.PostgresMigration` project so the existing SQLite migration history can stay intact.
4. Apply the Postgres migration to an empty Supabase database.
5. Export the current SQLite data and import it into Postgres.
6. Validate the app locally against Postgres before any deployment cutover.
7. Update the Render production environment to use the Postgres provider and session pooler connection string.
8. Deploy and verify CRUD, contact submissions, and admin login on the hosted site.
9. After initial deployment is stable, reevaluate direct Postgres connectivity from the hosting environment and switch from the session pooler to the direct connection string only if IPv6 connectivity is confirmed or an IPv4-capable direct option is available.

## Important Constraint

The current migrations include SQLite-specific annotations such as `Sqlite:Autoincrement`. That means the safest cutover is usually one of these approaches:

1. Create a fresh Postgres baseline migration once the model is stable.
2. Or maintain provider-specific migrations if you need both providers to evolve independently.

For this project, a fresh Postgres baseline in a separate migrations assembly is the lower-complexity path.

## Proposed Implementation Order

1. Keep SQLite as the default local provider.
2. Add Postgres provider support in application startup and design-time EF tooling.
3. Create the first Postgres baseline migration.
4. Add a small one-time data migration path from SQLite to Postgres.
5. Cut production over to Supabase using the session pooler.
6. After launch, test whether the production host can use Supabase's direct connection path safely and reliably.
7. Later, switch production from session pooler to direct connection if IPv6 support is verified and the operational tradeoff is worthwhile.
8. Later, remove SQLite entirely if local file-based development is no longer useful.

## Validation Checklist

1. `dotnet build --configuration Release modisette.sln`
2. `dotnet test --configuration Release modisette.sln`
3. `dotnet ef dbcontext info` with SQLite settings
4. `dotnet ef dbcontext info` with Postgres settings
5. `dotnet ef database update` against a disposable Postgres database
6. Manual verification of contact form persistence, admin CRUD, and content reads