using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitConverter.Models;
using UnitConverter.Services;

namespace UnitConverter.Pages;

public class LogsModel : PageModel
{
    private readonly ILogger<LogsModel> _logger;
    private readonly ILogReader _logReader;

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Level { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ConversionType { get; set; }

    public List<LogEntry> Entries { get; set; } = new List<LogEntry>();

    public LogsModel(ILogger<LogsModel> logger, ILogReader logReader)
    {
        _logger = logger;
        _logReader = logReader;
    }

    public IActionResult OnGet()
    {
        IEnumerable<LogEntry> allEntries;

        try
        {
            allEntries = _logReader.Read();
        }
        catch (IOException e)
        {
            _logger.LogError(e, "Unable to load logs");
            return RedirectToPage("/Error");
        }

        foreach (LogEntry entry in allEntries)
        {
            if (MatchesFilters(entry))
            {
                Entries.Add(entry);
            }
        }

        return Page();
    }

    private bool MatchesFilters(LogEntry entry)
    {
        if (!string.IsNullOrEmpty(Level) &&
            !string.Equals(entry.Level, Level, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(ConversionType) &&
            !string.Equals(entry.ConversionType, ConversionType, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(Search))
        {
            bool inMessage = entry.Message != null &&
                             entry.Message.Contains(Search, StringComparison.OrdinalIgnoreCase);
            bool inException = entry.Exception != null &&
                               entry.Exception.Contains(Search, StringComparison.OrdinalIgnoreCase);

            if (!inMessage && !inException)
            {
                return false;
            }
        }

        return true;
    }
}
