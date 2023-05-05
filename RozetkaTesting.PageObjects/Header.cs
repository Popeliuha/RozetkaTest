using OpenQA.Selenium;

namespace RozetkaTesting.PageObjects
{
    public class Header : BasePage
    {
        public Header(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement btnCart => webDriver.FindElement(By.XPath("//button[@rzopencart]"));
        private IWebElement txtSearch => webDriver.FindElement(By.XPath("//input[@name='search']"));
        private IWebElement btnSearch => webDriver.FindElement(By.XPath("//button[contains(@class, 'green')]"));
       
        public void OpenCart() => btnCart.Click();
        public void ClickSearch() => btnSearch.Click();
        public void SetSearchText(string text)=> txtSearch.SendKeys(text);
    }
}
