using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trading.Core.Interfaces;

namespace Trading.Core.Models
{
    public class LunoAccountManager : IAccount, IPurchase
    {
        public LunoAccountManager(IWebDriver webDriver)
        {
            WebDriver = webDriver;
            ProcessHelper = new ProcessHelper(WebDriver);
        }

        public IWebDriver WebDriver { get; }
        public ProcessHelper ProcessHelper { get; }

        public bool BuyCrypto(CurrencyType FromCurrencyTypeAccount, string Amount, CurrencyType ToCurrencyTypeAccount)
        {
            try
            {
                GoToWallet();
                WebDriver.FindElements(By.ClassName("account-info")).First(x => x.Text.Contains(FromCurrencyTypeAccount.ToString())).Click();
                WebDriver.FindElements(By.ClassName("qab-item ng-star-inserted")).First(x => x.Text.Contains("Buy")).Click();
                ProcessHelper.Wait(30);
                WebDriver.FindElements(By.ClassName("mat-list-text")).First(x => x.Text.Contains("Once-off")).Click();
                WebDriver.FindElements(By.ClassName("mat-list-text")).First(x => x.Text.Contains(ToCurrencyTypeAccount.ToString())).Click();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<string> GetAccounts()
        {
            GoToWallet();
            var accounts = WebDriver.FindElements(By.ClassName("accounts-list-grouping ng-star-inserted")).First().Text;
            return accounts.Split("\r\n");
        }

        public bool SellCrypto(CurrencyType CryptoType, string Amount, CurrencyType FromCurrencyType)
        {
            throw new NotImplementedException();
        }

        public bool SendCrypto(CurrencyType CryptoType, string SendToAccount, string Amount, string Note = null)
        {
            throw new NotImplementedException();
        }

        private void GoToWallet()
        {
            WebDriver.FindElements(By.ClassName("luno-desktop-nav__item")).First(x => x.Text.Trim().Contains("Wallets")).Click();
        }
    }
}
