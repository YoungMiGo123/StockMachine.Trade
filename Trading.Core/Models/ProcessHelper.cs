using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trading.Core.Models
{
    public class ProcessHelper
    {
        public ProcessHelper(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        public IWebDriver WebDriver { get; }
        public IEnumerable<string> FindAllLinks(By by, string ContainsText = null)
        {
            var links = WebDriver.FindElements(by).Select(x => x.GetAttribute("href"));
            if (!string.IsNullOrEmpty(ContainsText))
            {
                links = links.Where(x => x.Contains(ContainsText));
            }
            return links;
        }
        public string GetFirstLinkFromWebElement(By by)
        {
            var firstlink = WebDriver.FindElement(by).GetAttribute("href");
            return firstlink;

        }
        public string GetAttribute(By By, string Attribute)
        {
           var attribute =  WebDriver.FindElement(By).GetAttribute(Attribute);
           return attribute;
        }
        public string TryGetAttribute(By By, string Attribute)
        {
            try
            {
                var attribute = WebDriver.FindElement(By).GetAttribute(Attribute);
                return attribute;
            }
            catch
            {
                return string.Empty;
            }
        }
        public void Wait(int seconds)
        {
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }
        public IWebElement GetElement(By by)
        {
            return WebDriver.FindElement(by);
        }
        public void GoToUrl(string Url, int WaitTime = 0)
        {

            WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            Wait(WaitTime);
            WebDriver.Navigate().GoToUrl(Url);
        }
        public void TryClickElement(IWebElement element)
        {
            int retrycount = 0;
            while (true)
            {
                try
                {
                    Wait(60);
                    element.Click();
                    Wait(60);
                    break;
                }
                catch
                {
                    if (retrycount == 5) break;
                    retrycount++;
                }
            }
        }
        public bool DoesElementExist(By By)
        {
            return WebDriver.FindElements(By).Count >= 1;
        }
        public string TryGetElementText(By by)
        {
            try
            {
                if (DoesElementExist(by))
                {
                    Wait(30);
                    return GetElementText(by);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }

        }

        public void ClearInputText(By by)
        {
            WebDriver.FindElement(by).Clear();
        }
        public void SendKeysToInput(By by, string name)
        {

            WebDriver.FindElement(by).SendKeys(name);
        }
        public string GetAttribute(IWebElement webElement, string attribute = "href")
        {
           return  webElement.GetAttribute(attribute);
        }

        public string GetElementText(By By)
        {
            return WebDriver.FindElement(By).Text;
        }
        public void ClickElement(By by)
        {
            WebDriver.FindElement(by).Click();
        }
        public void ClickPolicyAgreements(By By)
        {

            if (WebDriver.FindElements(By).FirstOrDefault() is IWebElement ConfirmAgreePolicies)
            {
                Wait(30);
                ConfirmAgreePolicies.Click();
            }
        }
    }
}
