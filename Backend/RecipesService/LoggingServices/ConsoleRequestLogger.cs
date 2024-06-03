
namespace RecipesService.LoggingServices;

public class ConsoleRequestLogger : IRequestLogger
{
    public void LogRequest(string message)
    {
        Console.WriteLine($"CONSOLE LOGGER: {message}");
    }
}
