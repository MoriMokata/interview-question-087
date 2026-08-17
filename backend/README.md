# Backend — Example Social Feed API

.NET 10 Web API implementing the "IT 08-1" post & comments page: a post by
"Change can" with a comment thread where the current user "Blend 285" can
add a comment (pressing Enter in the UI).

## Architecture

Clean architecture, 4 layers + tests:

```
Example.SocialFeed.Domain          Entities (Post, Comment), exceptions
Example.SocialFeed.Application     DTOs, service interfaces, business logic
Example.SocialFeed.Infrastructure  EF Core (SQLite), repositories, seed data
Example.SocialFeed.Api             Controllers, DI wiring, Swagger, CORS
Example.SocialFeed.Tests           xUnit + Moq + FluentAssertions, EF InMemory
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Run

```bash
cd backend
dotnet restore
dotnet run --project Example.SocialFeed.Api
```

The API starts on the URL printed in the console (see
`Example.SocialFeed.Api/Properties/launchSettings.json`) and serves Swagger
UI at `/swagger`. A SQLite database file (`socialfeed.db`) is created
automatically on first run and seeded with the post/comment shown in the
original mockup.

By default the API allows CORS requests from `http://localhost:4200`
(the Angular dev server). Override via the `AngularDevClientOrigin` setting
in `appsettings.json` if needed.

## Test

```bash
cd backend
dotnet test
```

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/posts/{postId}` | Get a post with its comments |
| GET | `/api/posts/{postId}/comments` | List comments for a post |
| POST | `/api/posts/{postId}/comments` | Add a comment as the current user (`{ "text": "..." }`) |
