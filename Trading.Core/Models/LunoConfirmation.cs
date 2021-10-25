using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trading.Core.Interfaces;

namespace Trading.Core.Models
{
    class LunoConfirmation : IConfirm
    {
        public LunoConfirmation(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        public IWebDriver WebDriver { get; }

        public bool Confirm(string passcode)
        {
            try
            {
                WebDriver.FindElements(By.ClassName("input-label ng-tns-c109-0 ng-star-inserted")).First(x => x.Text.Contains("4-digit code")).Click();
                WebDriver.FindElements(By.ClassName("input-label ng-tns-c109-0 ng-star-inserted")).First(x => x.Text.Contains("4-digit code")).SendKeys(passcode);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
