:: For tracking migration commands, invoke from RecipesService directory
dotnet ef migrations add "InitialCreate" -o Data/Migrations
dotnet ef database update