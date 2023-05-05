using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozetkaTesting.PageObjects
{
    public class InitPage : BasePage
    {
        public InitPage(IWebDriver driver) : base(driver)
        {
        }

        public Header Header = new Header(webDriver);
        private IWebElement imgUkrLang => webDriver.FindElement(By.XPath("//img[@alt='ua']"));
        private List<IWebElement> btnCategoryByName(string name) => webDriver.FindElements(By.XPath($"//*[text()='{name}']")).ToList();
        
        public void SwitchLanguageToUA() => imgUkrLang.Click();
        public void ClickOnCategoryByName(string name)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(x => btnCategoryByName(name).Where(x => x.Displayed).FirstOrDefault());
            btnCategoryByName(name).Where(x => x.Displayed).FirstOrDefault().Click();
        }
    }
}
