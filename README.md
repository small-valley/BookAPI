## Development Commands

-   Run backend services

    `docker-compose up`

-   Restart backend service

    `docker-compose restart backend`

## Migration Commands

-   Add a migration file

    `dotnet ef migrations add <Migration Name> --project Book_EF/Book_EF.csproj --startup-project Book_API/Book_API.csproj`

-   Remove migrations

    `dotnet ef migrations remove --project Book_EF/Book_EF.csproj --startup-project Book_API/Book_API.csproj`

-   Run migration

    `dotnet ef database update --project Book_API/Book_API.csproj`

-   Drop Database

    `dotnet ef database drop --project Book_API/Book_API.csproj`

## File Format Commands

-   Apply format

    `dotnet format`
