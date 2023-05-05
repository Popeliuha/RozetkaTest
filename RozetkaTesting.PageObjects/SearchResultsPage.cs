using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RozetkaTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RozetkaTesting.PageObjects
{
    public class SearchResultsPage : BasePage
    {
        public SearchResultsPage(IWebDriver driver) : base(driver)
        {
        }
        public Header Header = new Header(webDriver);

        private string filterByName(string name) => $"//div[@data-filter-name='{name}']/div";

        private IWebElement chbFilterByNameAndValue(string name, string value) =>
            webDriver.FindElement(By.XPath($"{filterByName(name)}//a[@data-id='{value}']"));
        private IWebElement txtPriceMin
            => webDriver.FindElement(By.XPath($"{filterByName("price")}//input[@formcontrolname = 'min']"));
        private IWebElement txtPriceMax
            => webDriver.FindElement(By.XPath($"{filterByName("price")}//input[@formcontrolname = 'max']"));
        private IWebElement btnPriceSubmit
            => webDriver.FindElement(By.XPath($"{filterByName("price")}//button[@type='submit']"));
        private List<IWebElement> searchResults => webDriver.FindElements(By.XPath("//rz-catalog-tile")).ToList();

        public void CheckFilterByNameAndValue(string name, string value)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => chbFilterByNameAndValue(name, value).Displayed);
            chbFilterByNameAndValue(name, value).Click();
        }

        public void SetFilterByPrice(double priceMin, double priceMax)
        {
            ScrollDownByPixels(300);
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => txtPriceMin.Displayed);
            txtPriceMin.Clear();
            txtPriceMax.Clear();
            txtPriceMin.SendKeys(priceMin.ToString());
            txtPriceMax.SendKeys(priceMax.ToString());
            btnPriceSubmit.Click();
        }

        public Dictionary<string, double> GetSearchResultDetails()
        {
            ScrollDownByPixels(0);
            Dictionary<string, double> productsDetails = new Dictionary<string, double>();

            foreach (var product in searchResults)
            {
                string title = product.FindElement(By.XPath(".//a[contains(@class, 'heading')]")).GetAttribute("title");
                string price = product.FindElement(By.XPath(".//div[contains(@class, 'prices')]//span[contains(@class, 'value')]")).Text.Replace(" ", "").Replace("₴","");
                productsDetails.Add(title, double.Parse(price));
            }

            return productsDetails;
        }

        public List<double> GetSearchResultPrices()
        {
            ScrollDownByPixels(0);
            List<double> productsPrices = new List<double>();

            foreach (var product in searchResults)
            {
                string price = product.FindElement(By.XPath(".//div[contains(@class, 'prices')]//span[contains(@class, 'value')]")).Text.Replace(" ", "").Replace("₴", "");
                productsPrices.Add(double.Parse(price));
            }

            return productsPrices;
        }

        public Product GetProductDetailsById(int id)
        {
            ScrollDownByPixels(50);
            Product product = new Product();
            product.Name = searchResults[id].FindElement(By.XPath(".//a[contains(@class, 'heading')]")).GetAttribute("title");
            product.Price = double.Parse(searchResults[id].FindElement(By.XPath(".//div[contains(@class, 'prices')]//span[contains(@class, 'value')]")).Text.Replace(" ", "").Replace("₴", ""));

            return product;
        }

        public void AddFirstItemToCart(int id)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => webDriver.FindElements(By.TagName("app-buy-button")).Where(x=>x.Displayed));
            var elements = webDriver.FindElements(By.TagName("app-buy-button"));
            if (elements.Count < id)
                throw new Exception("Count of products is less than required id");

            elements[id].Click();
        }

        public void SelectDropdownValueToOrderByPrice()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => webDriver.FindElement(By.TagName("select")).Displayed);
            IWebElement select = webDriver.FindElement(By.TagName("select"));
            SelectElement selectElement = new SelectElement(select);
            selectElement.SelectByValue("1: cheap");
        }
    }
}
