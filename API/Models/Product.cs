namespace API.Models;

using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

public record Product(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("brandName")] string BrandName,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("descriptionText")] string? DescriptionText,
    [property: JsonPropertyName("articles")] Article[] Articles
);

public partial record Article(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("shortDescription")] string ShortDescription,
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("unit")] Unit Unit,
    [property: JsonPropertyName("pricePerUnitText")] string PricePerUnitText,
    [property: JsonPropertyName("image")] string Image
)
{
    public int? GetBottleCount()
    {
        var match = BottleCount().Match(ShortDescription);
        return match.Success ? int.Parse(match.Groups[1].Value) : null;
    }

    public decimal? GetPricePerLiter()
    {
        var match = PricePerLiter().Match(PricePerUnitText);
        return match.Success ? decimal.Parse(match.Groups[1].Value, CultureInfo.GetCultureInfo("de-DE")) : null;
    }

    // Extracts the bottle count at the start of the string (e.g. "20 x 0,5L (Glas)" → 20)
    [GeneratedRegex(@"^(\d+)\s*x")]
    private static partial Regex BottleCount();

    // Extracts the price per liter inside parentheses (e.g. "(2,10 €/Liter)" → 2,10)
    [GeneratedRegex(@"\(([0-9,]+)\s*€/Liter\)")]
    private static partial Regex PricePerLiter();
}

public enum Unit
{
    Liter = 1
}
