using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RozetkaTesting
{
    public class BaseTest
    {
        public IWebDriver driver;
        protected string websiteUrl = "https://rozetka.com.ua/ua/";

        [SetUp]
        public void SetUpTest()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--window-size=1920,1080");
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(websiteUrl);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }
    }
}