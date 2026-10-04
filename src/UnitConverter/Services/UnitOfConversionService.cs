using UnitConverter.Models;

namespace UnitConverter.Services;

public class UnitOfConversionService : IConversionService
{
    public decimal Convert(decimal value, string conversionType)
    {
        var result = conversionType switch
        {
            ConversionTypes.MilesToKilometers => new UnitOf.Length().FromMiles((double)value).ToKilometers(),
            ConversionTypes.KilometersToMiles => new UnitOf.Length().FromKilometers((double)value).ToMiles(),
            ConversionTypes.FahrenheitToCelsius => new UnitOf.Temperature().FromFahrenheit((double)value).ToCelsius(),
            ConversionTypes.CelsiusToFahrenheit => new UnitOf.Temperature().FromCelsius((double)value).ToFahrenheit(),
            ConversionTypes.PoundsToKilograms => new UnitOf.Mass().FromPounds((double)value).ToKilograms(),
            ConversionTypes.KilogramsToPounds => new UnitOf.Mass().FromKilograms((double)value).ToPounds(),
            ConversionTypes.GallonsToLiters => new UnitOf.Volume().FromGallonsUS((double)value).ToLiters(),
            ConversionTypes.AcresToHectares => new UnitOf.Area().FromAcres((double)value).ToHectares(),
            _ => throw new ArgumentException(
                $"Unsupported conversion type: {conversionType}",
                nameof(conversionType))
        };

        return decimal.Truncate((decimal)result * 100m) / 100m;
    }
    }

