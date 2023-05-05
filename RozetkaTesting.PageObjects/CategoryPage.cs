using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaTesting.PageObjects
{
    public class CategoryPage : BasePage
    {
        public CategoryPage(IWebDriver driver) : base(driver)
        {
        }

        IWebElement btnSubCategoryByName(string name) => webDriver.FindElement(By.XPath($"//a[@title='{name}'][contains(@class, 'heading')]"));

        public void ClickSubCategoryByName(string name)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => btnSubCategoryByName(name).Displayed);
            btnSubCategoryByName(name).Click();
        }
    }
}
