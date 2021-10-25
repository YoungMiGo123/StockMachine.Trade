using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trading.Core.Interfaces;

namespace Trading.Core.Models
{
    public class LunoAuthenticator : IAuth
    {
        public LunoAuthenticator(IWebDriver webDriver)
        {
            WebDriver = webDriver;
   
            ProcessHelper = new ProcessHelper(WebDriver);
        }

        public IWebDriver WebDriver { get; }

        public ProcessHelper ProcessHelper { get; }

        public bool LoginToConfirmation(string Url)
        {
            WebDriver.Navigate().GoToUrl(Url);
            ProcessHelper.Wait(60);
            var webElement = WebDriver.FindElements(By.ClassName("ng-star-inserted")).FirstOrDefault(x => x.Text.ToLower().Contains("google"));
            ProcessHelper.Wait(60);
            webElement.Click();
            ProcessHelper.Wait(30);
            if (ProcessHelper.DoesElementExist(By.Id("mat-input-0")))
            {
                ProcessHelper.Wait(60);
                return true;
            }
            return false;

        }
    }
}
