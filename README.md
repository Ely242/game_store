# Game Store API

A small ASP.NET Core minimal API for managing games and genres, with a simple frontend for demo usage.

## Stack

- ASP.NET Core minimal APIs
- Entity Framework Core
- SQLite
- Plain HTML, CSS, and JavaScript frontend

## Project Structure

- `GameStore.Api/` - backend API and database code
- `Frontend/` - simple browser client
- `GameStore.slnx` - solution file

## Run Locally

1. Start the API:

```powershell
cd GameStore.Api
dotnet run
```

2. Open the frontend:

- Open `Frontend/index.html` directly in a browser, or
- Serve the folder with a simple local server

The frontend calls the API at `http://localhost:5048`.

## Features

- View, create, update, and delete games
- View, create, update, and delete genres
- Automatic EF Core migrations on startup
- Initial genre seeding

## Notes

- The SQLite database file is excluded from git.
- CORS is enabled for local frontend development.