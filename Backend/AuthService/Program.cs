var builder = WebApplication.CreateBuilder(args);

// DI components

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();