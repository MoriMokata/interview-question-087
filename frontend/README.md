# Frontend — Example Social Feed

Angular 21 (standalone components) app for the "IT 08-1" post & comments
page. Renders a post, lets the current user ("Blend 285") type a comment and
press Enter to submit it.

## Prerequisites

- Node.js 22+ and npm
- The backend API running (see `../backend/README.md`)

## Setup

```bash
cd frontend
npm install
```

If your backend runs on a different URL than `https://localhost:7000/api`,
update `API_BASE_URL` in `src/app/core/config.ts`.

## Run

```bash
npm start
```

Opens the dev server at `http://localhost:4200`.

## Test

```bash
npm test
```

Runs the Vitest-based unit tests (12 specs across services and components).

## Build

```bash
npm run build
```

## Structure

```
src/app/
├── core/
│   ├── config.ts                 API base URL
│   ├── models/                   Post, Comment interfaces
│   └── services/                 PostService, CommentService (HttpClient)
├── feed/
│   ├── feed-page/                Page shell: header, post, comment list, input
│   ├── comment-item/             Renders a single existing comment
│   └── comment-input/            Text box, submits on Enter
└── app.ts                        Root component, renders <app-feed-page>
```
