namespace API.Models;

using System.Text.Json.Serialization;

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
);

public enum Unit
{
    Liter = 1
}
