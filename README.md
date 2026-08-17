# interview-question-087

Full-stack take-home assignment: a post + comments page ("IT 08-1") where the
current user ("Blend 285") can add a comment and see it appended below.

- **Backend**: C# / .NET 10, clean architecture (Domain/Application/Infrastructure/Api), EF Core + SQLite
- **Frontend**: Angular 21, standalone components

## Quick start

```bash
# Backend (needs .NET 10 SDK)
cd backend
dotnet restore
dotnet run --project Example.SocialFeed.Api   # serves API + Swagger at /swagger

# Frontend (needs Node 22+), in a second terminal
cd frontend
npm install
npm start                                      # http://localhost:4200
```

See [`backend/README.md`](backend/README.md) and [`frontend/README.md`](frontend/README.md) for full details, endpoints, and test commands.

## Database design

Two tables, one-to-many:

- **Posts**: `Id`, `AuthorName`, `Content`, `ImageUrl`, `CreatedAt`
- **Comments**: `Id`, `PostId` (FK, cascade delete), `AuthorName`, `Text`, `CreatedAt`

The current user ("Blend 285") is resolved server-side (`ICurrentUserProvider`), not supplied by the client. Avatar initials are derived from `AuthorName` at the DTO layer, not stored.
