:: For tracking project configuration commands, invoke from the root directory
:: initial backend setup
dotnet new sln
dotnet new webapi -o Backend/RecipesService
dotnet sln add Backend/RecipesService
dotnet new gitignore
:: search service setup
dotnet new webapi -o Backend/RecipesSearchService
dotnet sln add Backend/RecipesSearchService