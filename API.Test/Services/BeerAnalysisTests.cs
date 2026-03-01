namespace API.Test.Services;

using API.Models;
using API.Services;
using System.Collections.Generic;

[TestClass]
public class ProductAnalysisTests
{[TestMethod]
    public void GetCheapestAndMostExpensive_ReturnsOrderedByPricePerLiter()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A",
                MakeArticle(1, "6 x 0,5L", 6.00m, "(2,00 €/Liter)"),
                MakeArticle(2, "6 x 0,5L", 7.50m, "(2,50 €/Liter)")),
            MakeProduct(2, "Brand C", "Beer C",
                MakeArticle(1, "6 x 0,5L", 9.00m, "(2,90 €/Liter)"),
                MakeArticle(2, "6 x 0,5L", 9.00m, "(3,00 €/Liter)")),
        };

        var result = ProductAnalysis.GetCheapestAndMostExpensive(products);

        Assert.AreEqual(1, result.Cheapest.ProductId);
        Assert.AreEqual(1, result.Cheapest.ArticleId);
        Assert.AreEqual(2.00m, result.Cheapest.PricePerLiter);
        Assert.AreEqual(2, result.MostExpensive.ProductId);
        Assert.AreEqual(2, result.MostExpensive.ArticleId);
        Assert.AreEqual(3.00m, result.MostExpensive.PricePerLiter);
    }

    [TestMethod]
    public void GetCheapestAndMostExpensive_WithSingleArticle_ReturnsSameArticleForBoth()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A", MakeArticle(1, "6 x 0,5L", 6.00m, "(2,00 €/Liter)")),
        };

        var result = ProductAnalysis.GetCheapestAndMostExpensive(products);

        Assert.AreEqual(result.Cheapest.ProductId, result.MostExpensive.ProductId);
        Assert.AreEqual(result.Cheapest.PricePerLiter, result.MostExpensive.PricePerLiter);
    }

    [TestMethod]
    public void GetCheapestAndMostExpensive_FiltersUnparseable()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A", MakeArticle(1, "6 x 0,5L", 6.00m, "liter=2 Euro")),
        };
        Assert.ThrowsExactly<NoProductsException>(
            () => ProductAnalysis.GetCheapestAndMostExpensive(products));
    }

    [TestMethod]
    public void GetCheapestAndMostExpensive_ThrowsOnNoProduct()
    {
        Assert.ThrowsExactly<NoProductsException>(
            () => ProductAnalysis.GetCheapestAndMostExpensive([]));
    }

    [TestMethod]
    public void GetMostBottles_ReturnsProductWithHighestBottleCount()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A",
                MakeArticle(1, "6 x 0,5L",  6.00m),
                MakeArticle(2, "20 x 0,5L", 18.00m)),
            MakeProduct(2, "Brand B", "Beer B",
                MakeArticle(3, "12 x 0,5L", 10.00m)),
        };

        var result = ProductAnalysis.GetMostBottles(products);

        Assert.AreEqual(1, result.ProductId);
        Assert.AreEqual(20, result.BottleCount);
    }

    [TestMethod]
    public void GetMostBottles_ThrowsOnNoProducts()
    {
        Assert.ThrowsExactly<NoProductsException>(
            () => ProductAnalysis.GetMostBottles([]));
    }


    [TestMethod]
    public void GetMostBottles_FiltersUnparseable()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A", MakeArticle(1, "20 Flaschen a 0.5l",  6.00m)),
        };

        Assert.ThrowsExactly<NoProductsException>(
            () => ProductAnalysis.GetMostBottles(products));
    }

    [TestMethod]
    public void GetBeersAtPrice_ReturnsArticlesMatchingTargetPrice()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A", MakeArticle(1, "6 x 0,5L", 17.99m, "(2,00 €/Liter)")),
            MakeProduct(2, "Brand B", "Beer B", MakeArticle(2, "6 x 0,5L", 10.00m, "(2,00 €/Liter)")),
        };

        var result = ProductAnalysis.GetBeersAtPrice(products, 17.99m);

        Assert.HasCount(1, result);
        Assert.AreEqual(1, result[0].ProductId);
        Assert.AreEqual(17.99m, result[0].Price);
    }

    [TestMethod]
    public void GetBeersAtPrice_ReturnsSortedByPricePerLiterAscending()
    {
        // Article 1: 6 x 0.5L → 6.00 €/L (expensive), Article 2: 12 x 0.5L → 3.00 €/L (cheap)
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A", MakeArticle(1, "6 x 0,5L",  17.99m, "(6,00 €/Liter)")),
            MakeProduct(2, "Brand B", "Beer B", MakeArticle(2, "12 x 0,5L", 17.99m, "(3,00 €/Liter)")),
        };

        var result = ProductAnalysis.GetBeersAtPrice(products, 17.99m);

        Assert.HasCount(2, result);
        Assert.AreEqual(2, result[0].ProductId, "Cheapest per litre should be first");
        Assert.IsLessThan(result[1].PricePerLiter!.Value, result[0].PricePerLiter!.Value);
    }

    [TestMethod]
    public void GetBeersAtPrice_IncludesAllMatchingArticlesAcrossProducts()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A",
                MakeArticle(1, "6 x 0,5L", 17.99m, "(3,00 €/Liter)"),
                MakeArticle(2, "6 x 0,5L", 10.00m, "(3,00 €/Liter)")),
            MakeProduct(2, "Brand B", "Beer B",
                MakeArticle(3, "6 x 0,5L", 17.99m, "(3,00 €/Liter)")),
        };

        var result = ProductAnalysis.GetBeersAtPrice(products, 17.99m);

        Assert.HasCount(2, result);
    }

    [TestMethod]
    public void GetBeersAtPrice_ReturnsEmptyList_WhenNoPriceMatches()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A", MakeArticle(1, "6 x 0,5L", 10.00m)),
        };

        var result = ProductAnalysis.GetBeersAtPrice(products, 17.99m);

        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void GetAll_ReturnsCombinedResults()
    {
        var products = new List<Product>
        {
            MakeProduct(1, "Brand A", "Beer A",
                MakeArticle(1, "6 x 0,5L", 6.00m, "(2,00 €/Liter)"),
                MakeArticle(2, "6 x 0,5L", 17.99m, "(2,00 €/Liter)")),
        };
        var result = ProductAnalysis.GetAllResults(products);

        Assert.IsNotNull(result.CheapestAndMostExpensive);
        Assert.IsNotNull(result.BeersAt1799);
        Assert.IsNotNull(result.MostBottles);
        Assert.HasCount(1, result.BeersAt1799);
    }

    private static Product MakeProduct(int id, string brand, string name, params Article[] articles) =>
    new(id, brand, name, null, articles);

    private static Article MakeArticle(int id, string shortDescription, decimal price, string pricePerUnitText = "") =>
        new(id, shortDescription, price, Unit.Liter, pricePerUnitText, "");
}
