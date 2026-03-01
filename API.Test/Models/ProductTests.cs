namespace API.Test.Models;

using API.Models;
using System.Globalization;

[TestClass]
public class ProductTests
{
    [TestMethod]
    [DataRow("20 x 0,5L (Glas)", 20)]
    [DataRow("20 Gläser", null)]
    public void GetBottleCount_ReturnsExpected(string input, int? expected)
    {
        var article = new Article(0, input, 0, Unit.Liter, "", "");
        var result = article.GetBottleCount();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("(2,10 €/Liter)", "2.10")]
    [DataRow("20 Gläser", null)]
    public void GetPricePerLiter_ReturnsExpected(string input, string? expectedStr)
    {
        var article = new Article(0, "", 0, Unit.Liter, input, "");
        var result = article.GetPricePerLiter();

        decimal? expected = expectedStr is not null
            ? decimal.Parse(expectedStr, CultureInfo.InvariantCulture)
            : null;

        Assert.AreEqual(expected, result);
    }
}
