## GamesApi

- Install the .NET 8 SDK. Run `dotnet restore`, `dotnet build`, and `dotnet run`.
- Configure the Bearer scheme with a trusted issuer, audience, and signing key. For local development, `dotnet user-jwts create --role player --claim subscription=platinum` issues a test token. Use the URL printed by the app.
- `GET /playergames` requires the admin role. `GET /mygames` requires the player role. Supported subscription claims are silver and platinum. The sample game assignments are in memory.
- Verified with synthetic signed tokens: unauthenticated 401, role and unknown subscription 403, admin access, an empty assignment, and the platinum catalog.
