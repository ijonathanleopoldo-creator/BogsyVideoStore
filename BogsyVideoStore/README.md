# BogsyVideoStore 

Short notes for running this sample on another Windows machine.

Prerequisites
- .NET 8 runtime or SDK installed (run `dotnet --info`).
- A SQL Server instance accessible from the host. The default configuration uses LocalDB (Windows-only). Install SQL Server Express LocalDB for a straightforward dev experience.

What the app does on startup
- The app runs EF Core migrations automatically (Program.cs calls `Database.MigrateAsync()`).
- The DbInitializer seeds an admin account (email: `admin@bvs.com`, password: `Admin123!`) and sample customers/videos when the DB is empty.

Default configuration
- appsettings.json uses:
  `Server=(localdb)\\mssqllocaldb;Database=BogsyVideoStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`
  This works on Windows with LocalDB. It will fail on Linux/macOS unless you point it to a network SQL Server.

Quick start (Windows + LocalDB)
1. Install .NET 8 runtime (or SDK) and SQL Server Express LocalDB.
2. From the repo root run:
   `dotnet run --project BogsyVideoStore`
3. The database will be created and seeded automatically. Open the app in the browser using the URL shown in the console or via Visual Studio launch settings.

Using a different SQL Server or CI
- Set the connection string via an environment variable (recommended), e.g. in PowerShell:
  `$env:ConnectionStrings__DefaultConnection = 'Server=<host>,1433;Database=BogsyVideoStore;User Id=<user>;Password=<pw>;TrustServerCertificate=True'`
- Then run the project as above. Ensure the SQL Server account has permission to create/modify the database so migrations succeed.

Cross-machine / cross-platform suggestions 
- For quick cross-machine testing you can run SQL Server in Docker and point the app to that container.
- Alternatively, for a lightweight local sample that works on Linux/macOS, convert to SQLite for development (requires adding EF Core Sqlite package and small code changes). Contact me if you want this done.
