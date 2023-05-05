using Microsoft.Extensions.Configuration;
using NUnit.Allure.Core;
using NUnit.Framework;
using RozetkaTesting.Models;
using RozetkaTesting.PageObjects;
using System.Configuration;

namespace RozetkaTesting.Tests
{
    [AllureNUnit]
    public class Tests : BaseTest
    {
        [Test]
        public void FilterTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appconfig.json").Build();
            double minPrice = double.Parse(config["minPrice"]); 
            double maxPrice = double.Parse(config["maxPrice"]);

            Assert.AreEqual(websiteUrl, driver.Url, "Wrong url opened.");

            InitPage initPage = new InitPage(driver);
            initPage.SwitchLanguageToUA();
            initPage.ClickOnCategoryByName(config["category1"]);

            CategoryPage categoryPage = new CategoryPage(driver);
            categoryPage.ClickSubCategoryByName(config["subcategory1"]);

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);
            searchResultsPage.CheckFilterByNameAndValue("seller", "Rozetka");
            searchResultsPage.ScrollDownByPixels(400);
            searchResultsPage.CheckFilterByNameAndValue("platforma", "Playstation 5");
            searchResultsPage.SetFilterByPrice(minPrice, maxPrice);

            var searchResultDetails = searchResultsPage.GetSearchResultDetails();
            Assert.IsNotNull(searchResultDetails);

            foreach (var item in searchResultDetails)
            {
                Assert.LessOrEqual(item.Value, maxPrice, $"Product {item.Key} price {item.Value} is not less than maximum price");
                Assert.GreaterOrEqual(item.Value, minPrice, $"Product {item.Key} price {item.Value} is not less than maximum price");
            }
        }
        
        [Test]
        public void CartTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appconfig.json").Build();
            Assert.AreEqual(websiteUrl, driver.Url, "Wrong url opened.");

            InitPage initPage = new InitPage(driver);
            initPage.SwitchLanguageToUA();
            initPage.ClickOnCategoryByName(config["category1"]);

            CategoryPage categoryPage = new CategoryPage(driver);
            categoryPage.ClickSubCategoryByName(config["subcategory1"]);

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);
            List<Product> productDetails = new List<Product>();
            productDetails.Add(searchResultsPage.GetProductDetailsById(0));
            searchResultsPage.AddFirstItemToCart(0);

            driver.Navigate().GoToUrl(websiteUrl);
            initPage = new InitPage(driver);
            initPage.ClickOnCategoryByName(config["category2"]);

            categoryPage = new CategoryPage(driver);
            categoryPage.ClickSubCategoryByName(config["subcategory2"]);

            searchResultsPage = new SearchResultsPage(driver);
            productDetails.Add(searchResultsPage.GetProductDetailsById(0));
            searchResultsPage.AddFirstItemToCart(0);
            searchResultsPage.Header.OpenCart();

            CartPage cartPage = new CartPage(driver);
            var cartProductsDetails = cartPage.GetCartProductsDetails();

            foreach (var product in cartProductsDetails)
            {
                Assert.That(productDetails.Any(x => x.Name.Equals(product.Name)));
                Assert.That(productDetails.Where(x => x.Name.Equals(product.Name))
                    .Any(x => x.Price.Equals(product.Price)));
                Assert.That(productDetails.Where(x => x.Name.Equals(product.Name))
                    .Any(x => x.Quantity.Equals(product.Quantity)));
            }

            double totalPrice = cartPage.GetTotalPrice();
            double productsPriceSum = cartPage.GetProductsPricesSum();
            Assert.AreEqual(productsPriceSum, totalPrice, "Total price is not equal to sum of product prices");

            cartPage.IsBtnDeleteEnabled();
        }

        [Test]
        public void SearchTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appconfig.json").Build();
            string searchWord = config["searchWord"];
            Assert.AreEqual(websiteUrl, driver.Url, "Wrong url opened.");

            InitPage initPage = new InitPage(driver);
            initPage.SwitchLanguageToUA();
            initPage.Header.SetSearchText(searchWord);
            initPage.Header.ClickSearch();

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);
            var details = searchResultsPage.GetSearchResultDetails();
            foreach (var item in details)
            {
                StringAssert.Contains(searchWord.ToLower(), item.Key.ToLower());
            }
        }

        [Test]
        public void OrderByPriceTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appconfig.json").Build();
            InitPage initPage = new InitPage(driver);
            initPage.SwitchLanguageToUA();
            initPage.ClickOnCategoryByName(config["category1"]);

            CategoryPage categoryPage = new CategoryPage(driver);
            categoryPage.ClickSubCategoryByName(config["subcategory1"]);

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);
            searchResultsPage.SelectDropdownValueToOrderByPrice();
            List<double> pricesList = searchResultsPage.GetSearchResultPrices();
            List<double> orderedPricesList = pricesList.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(pricesList, orderedPricesList);

            Assert.Fail("Тому шо ви так сказали шо тестик має впасти :)");
        }
    }
}
