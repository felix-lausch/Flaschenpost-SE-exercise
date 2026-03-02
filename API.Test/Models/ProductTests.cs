namespace API.Test.Models;

using API.Models;
using System.Globalization;

[TestClass]
public class ProductTests
{
    [TestMethod]
    [DataRow("20 x 0,5L (Glas)", 20)]
    [DataRow("6 x 0,5L", 6)]
    [DataRow("12 x 0,33L", 12)]
    [DataRow("1 x 5L", 1)]
    [DataRow("20 Gläser", null)]
    [DataRow("5L Fass", null)]
    public void GetBottleCount_ReturnsExpected(string input, int? expected)
    {
        var article = new Article(0, input, 0, Unit.Liter, "", "");
        var result = article.GetBottleCount();

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("(2,10 €/Liter)", "2.10")]
    [DataRow("(1,00 €/Liter)", "1.00")]
    [DataRow("(10,99 €/Liter)", "10.99")]
    [DataRow("20 Gläser", null)]
    [DataRow("", null)]
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
