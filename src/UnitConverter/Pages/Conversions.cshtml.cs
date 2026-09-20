using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string ConversionType { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;

    public void OnGet()
    {
        ViewData["Title"] = "Conversions";

        if (string.IsNullOrEmpty(ConversionType))
        {
            ConversionType = "MilesToKilometers";
            ViewData["ConversionType"] = "Miles to Kilometers";
        }

        if (string.IsNullOrEmpty(Input))
        {
            Input = "3.1415";
        }

        double value;
        try
        {
            value = Convert.ToDouble(Input);
        }
        catch (FormatException)
        {
            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }

        double? result;
        try
        {
            result = ConversionType switch
            {
                "MilesToKilometers" => new UnitOf.Length().FromMiles(value).ToKilometers(),
                "KilometersToMiles" => new UnitOf.Length().FromKilometers(value).ToMiles(),
                "FahrenheitToCelsius" => new UnitOf.Temperature().FromFahrenheit(value).ToCelsius(),
                "CelsiusToFahrenheit" => new UnitOf.Temperature().FromCelsius(value).ToFahrenheit(),
                "PoundsToKilograms" => new UnitOf.Mass().FromPounds(value).ToKilograms(),
                "KilogramsToPounds" => new UnitOf.Mass().FromKilograms(value).ToPounds(),
                "GallonsToLiters" => new UnitOf.Volume().FromGallonsUS(value).ToLiters(),
                "AcresToHectares" => new UnitOf.Area().FromAcres(value).ToHectares(),
                _ => (double?)null
            };
        }
        catch (ArgumentException)
        {
            ViewData["ErrorMessage"] = "The conversion could not be completed.";
            return;
        }

        if (result is null)
        {
            ViewData["ErrorMessage"] = "Unknown conversion type.";
            return;
        }

        Output = result.Value.ToString();
    }
}
