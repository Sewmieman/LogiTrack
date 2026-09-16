LogiTrack - Full Backend + Frontend

Included:
- backend/: .NET 10 LogiTrack API, Application, Domain, Infrastructure, tests, migrations
- frontend/: Angular LogiTrack client

Recent fixes:
- Angular routes for login, register, dashboard, customers, drivers, deliveries, payments, vehicles
- Registration now shows a successful-registration confirmation and sends the user to Login instead of logging them in automatically
- Password checklist updates while typing: 8+ chars, uppercase, lowercase, number, special character
- ASP.NET Core Identity enforces the same password requirements

Backend setup:
1. Configure PostgreSQL connection in backend/src/LogiTrack.Api/appsettings.json or user secrets/environment variables.
2. Make sure PostgreSQL is running and the logitrack database exists.
3. From backend/src/LogiTrack.Api run:
   dotnet ef database update --project ..\\LogiTrack.Infrastructure --startup-project .
4. Run:
   dotnet run

Frontend setup:
1. Open the frontend folder.
2. Install packages:
   npm.cmd install
3. Start Angular:
   npx.cmd ng serve
4. Open http://localhost:4200

Do not commit real database passwords or other secrets to GitHub.
