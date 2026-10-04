using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnitConverter.Models;
using UnitConverter.Services;

namespace UnitConverter.Pages;

public class QuickConversionsModel : PageModel
{
    private readonly IConversionService _conversionService;

    public QuickConversionsModel(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    public IEnumerable<SelectListItem> PoundOptions =>
    [
        new("1 pound", "1"),
        new("5 pounds", "5"),
        new("10 pounds", "10"),
        new("25 pounds", "25"),
        new("50 pounds", "50")
    ];

    public string? Output { get; private set; }
    public string? ErrorMessage { get; private set; }

    public IActionResult OnGetMilesToKilometers(string input) =>
        PerformConversion(input, ConversionTypes.MilesToKilometers);

    public IActionResult OnGetKilometersToMiles(string input) =>
        PerformConversion(input, ConversionTypes.KilometersToMiles);

    public IActionResult OnGetFahrenheitToCelsius(string input) =>
        PerformConversion(input, ConversionTypes.FahrenheitToCelsius);

    public IActionResult OnGetCelsiusToFahrenheit(string input) =>
        PerformConversion(input, ConversionTypes.CelsiusToFahrenheit);

    public IActionResult OnGetPoundsToKilograms(string input) =>
        PerformConversion(input, ConversionTypes.PoundsToKilograms);

    public IActionResult OnGetKilogramsToPounds(string input) =>
        PerformConversion(input, ConversionTypes.KilogramsToPounds);

    public IActionResult OnGetGallonsToLiters(string input) =>
        PerformConversion(input, ConversionTypes.GallonsToLiters);

    public IActionResult OnGetAcresToHectares(string input) =>
        PerformConversion(input, ConversionTypes.AcresToHectares);

    private IActionResult PerformConversion(string input, string conversionType)
    {
        if (!decimal.TryParse(input, out var value))
        {
            ErrorMessage = "Please enter a valid number.";
            return Page();
        }

        decimal result;
        try
        {
            result = _conversionService.Convert(value, conversionType);
        }
        catch (ArgumentException)
        {
            ErrorMessage = "That conversion attempt was stupid, I will not attempt it.";
            return Page();
        }

        Output = result.ToString();
        return Page();
    }
}
