using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public ConversionModel Conversion { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string ConversionType
    {
        get => Conversion.ConversionType;
        set => Conversion.ConversionType = value;
    }

    [BindProperty(SupportsGet = true)]
    public string Input
    {
        get => Conversion.Input;
        set => Conversion.Input = value;
    }

    public string Output
    {
        get => Conversion.Output;
        set => Conversion.Output = value;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Conversions";

        if (string.IsNullOrEmpty(Conversion.ConversionType))
        {
            Conversion.ConversionType = ConversionTypes.MilesToKilometers;
            ViewData["ConversionType"] = ConversionTypes.All[ConversionTypes.MilesToKilometers];
        }

        if (string.IsNullOrEmpty(Conversion.Input))
        {
            Conversion.Input = "3.1415";
        }

        double value;
        try
        {
            value = Convert.ToDouble(Conversion.Input);
        }
        catch (FormatException)
        {
            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }

        double? result;
        try
        {
            result = Conversion.ConversionType switch
            {
                ConversionTypes.MilesToKilometers => new UnitOf.Length().FromMiles(value).ToKilometers(),
                ConversionTypes.KilometersToMiles => new UnitOf.Length().FromKilometers(value).ToMiles(),
                ConversionTypes.FahrenheitToCelsius => new UnitOf.Temperature().FromFahrenheit(value).ToCelsius(),
                ConversionTypes.CelsiusToFahrenheit => new UnitOf.Temperature().FromCelsius(value).ToFahrenheit(),
                ConversionTypes.PoundsToKilograms => new UnitOf.Mass().FromPounds(value).ToKilograms(),
                ConversionTypes.KilogramsToPounds => new UnitOf.Mass().FromKilograms(value).ToPounds(),
                ConversionTypes.GallonsToLiters => new UnitOf.Volume().FromGallonsUS(value).ToLiters(),
                ConversionTypes.AcresToHectares => new UnitOf.Area().FromAcres(value).ToHectares(),
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

        Conversion.Output = result.Value.ToString();
    }
}
