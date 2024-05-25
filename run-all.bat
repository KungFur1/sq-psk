REM Starting all processes:
:: cd /D "%~dp0" changes to the directory where this file is located
REM 1. Starting all docker containers
cd /D "%~dp0"
docker compose up -d
REM 2. Starting RecipesService
cd /D "%~dp0"
cd ./Backend/RecipesService
start "Recipes Service" cmd /K dotnet watch
REM 3. Starting RecipesSearchService
cd /D "%~dp0"
cd ./Backend/RecipesSearchService
start "Recipes Search Service" cmd /K dotnet watch
REM 4. Starting AuthService
cd /D "%~dp0"
cd ./Backend/AuthService
start "Auth Service" cmd /K dotnet watch
REM 5. Starting ImagesService
cd /D "%~dp0"
cd ./Backend/ImagesService
start "Images Service" cmd /K dotnet watch