using System.Text.Json;
using UnitConverter.Models;

namespace UnitConverter.Services;

public class JsonLogReader : ILogReader
{
    private readonly string _logDirectory;
    private readonly ILogger<JsonLogReader> _logger;

    public JsonLogReader(IWebHostEnvironment environment, ILogger<JsonLogReader> logger)
    {
        _logDirectory = Path.Combine(environment.ContentRootPath, "Logs");
        _logger = logger;
    }

    public IEnumerable<LogEntry> Read()
    {
        List<LogEntry> logs = new List<LogEntry>();
        string[] logFiles;

        try
        {
            logFiles = Directory.GetFiles(_logDirectory, "*.json");
        }
        catch (DirectoryNotFoundException e)
        {
            _logger.LogWarning(e, "Log directory {LogDirectory} was not found", _logDirectory);
            return logs;
        }

        foreach (string logFile in logFiles)
        {
            FileStream stream;

            try
            {
                stream = new FileStream(
                    logFile,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite | FileShare.Delete);
            }
            catch (IOException e)
            {
                _logger.LogError(e, "Could not open log file {LogFile}", logFile);
                continue;
            }

            using StreamReader reader = new StreamReader(stream);

            try
            {
                LoadLogs(reader, logFile, logs);
            }
            catch (IOException e)
            {
                _logger.LogError(e, "Could not read log file {LogFile}", logFile);
            }
        }

        return logs;
    }

    private void LoadLogs(StreamReader reader, string logFile, List<LogEntry> logs)
    {
        while (reader.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            LogEntry? entry;

            try
            {
                entry = JsonSerializer.Deserialize<LogEntry>(line);
            }
            catch (JsonException e)
            {
                _logger.LogWarning(e, "Skipped malformed log line in {LogFile}", logFile);
                continue;
            }

            if (entry == null)
            {
                continue;
            }

            if (string.IsNullOrEmpty(entry.Level))
            {
                entry.Level = "Information";
            }

            logs.Add(entry);
        }
    }
}
