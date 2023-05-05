using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RozetkaTesting.PageObjects
{
    public class BasePage
    {
        public static IWebDriver webDriver;
        

        public BasePage(IWebDriver driver)
        {
            webDriver = driver;
        }

        public void ScrollDownByPixels(int pixels)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
            js.ExecuteScript($"window.scrollTo(0, {pixels});");
            Thread.Sleep(1000);
        }
    }
}