using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V110.DOM;
using OpenQA.Selenium.Support.UI;
using RozetkaTesting.Models;

namespace RozetkaTesting.PageObjects
{
    public  class CartPage : BasePage
    {
        public CartPage(IWebDriver driver) : base(driver)
        {
        }

        private string cartProductPriceXPath = "//*[contains(@class,'cart-product__price')]";
        private List<IWebElement> actualProducts => webDriver.FindElements(By.TagName("rz-cart-product")).ToList();
        private IWebElement txtTotalPrice => webDriver.FindElement(By.XPath("//div[@class='cart-receipt__sum-price']"));
        
        public bool IsCartEmpty()
        {
            var elements = webDriver.FindElements(By.XPath("//*[@class='cart-dummy__heading']")).Where(x => x.Displayed).ToList();
            return elements.Count != 0;
        }

        public void IsBtnDeleteEnabled()
        {
            for (int i = 0; i < actualProducts.Count; i++)
            {
                var button = webDriver.FindElement(By.XPath($"//button[@aria-controls='cartProductActions{i}']"));
                button.Click();
                if (!webDriver.FindElement(By.TagName("rz-trash-icon")).Enabled)
                    throw new Exception("Button delete is disabled");
            }
        }

        public double GetTotalPrice()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => txtTotalPrice.Displayed);
            return double.Parse(txtTotalPrice.Text.Replace(" ", "").Replace("₴", ""));
        }

        public double GetProductsPricesSum()
        {
            double sum = 0;
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            foreach (IWebElement actualProduct in actualProducts)
            {
                wait.Until(x => actualProduct.FindElement(By.XPath($".{cartProductPriceXPath}")).Displayed);
                sum += double.Parse(actualProduct.FindElement(By.XPath($".{cartProductPriceXPath}")).Text.Replace(" ", "").Replace("₴", ""));
            }

            return sum;
        }

        public List<Product> GetCartProductsDetails()
        {
            List<Product> products = new List<Product>();

            foreach (IWebElement actualProduct in actualProducts)
            {
                Product product = new Product();
                product.Name = actualProduct.FindElement(By.XPath(".//*[@class = 'cart-product__title']")).GetAttribute("title");
                product.Price = double.Parse(actualProduct.FindElement(By.XPath($".{cartProductPriceXPath}")).Text.Replace(" ", "").Replace("₴", ""));
                products.Add(product);
            }

            return products;
        }
    }
}
