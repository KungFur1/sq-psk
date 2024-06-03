
namespace RecipesService.LoggingServices;

public class FileRequestLogger : IRequestLogger
{
    private readonly string _filePath;

    public FileRequestLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void LogRequest(string message)
    {
        File.AppendAllText(_filePath, $"FILE LOGGER: {message}\n");
    }
}
