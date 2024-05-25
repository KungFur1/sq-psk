:: For tracking migration commands, invoke from AuthService directory
dotnet ef migrations add "InitialCreate" -o Data/Migrations
dotnet ef database update
dotnet ef migrations add "ChangeUserModel"