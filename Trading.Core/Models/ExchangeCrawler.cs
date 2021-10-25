using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Trading.Core.Models
{
    public class ExchangeCrawler
    {
        public ExchangeCrawler(IWebDriver webDriver)
        {
            WebDriver = webDriver;
            processHelper = new ProcessHelper(webDriver);
        }

        private IWebDriver WebDriver { get; }

        private ProcessHelper processHelper;

        public Dictionary<string, Exchange> GetExchanges(string Url)
        {
            WebDriver.Navigate().GoToUrl(Url);
            processHelper.Wait(30);
            WebDriver.FindElement(By.Id("show_more")).Click();
            processHelper.Wait(30);
            var GetAllExchanges = WebDriver.FindElements(By.CssSelector("tr")).Select(x => x.Text);
            var list = new List<Exchange>();

            var AllExchangesRawWithoutheaders = GetAllExchanges.Where((value, index) => index > 0);
                            
            foreach (var exchange in AllExchangesRawWithoutheaders)
            {
                var data = exchange.Split("\r\n");
                if(data.Length >= 5)
                {
                    var joinedStr = data.ElementAtOrDefault(3).Split(" ");
                    var thexch = new Exchange()
                    {
                        Country = data.ElementAtOrDefault(0),
                        ExchangeName = data.ElementAtOrDefault(1),
                        ExchangeSymbol = data.ElementAtOrDefault(2),
                        MarketCap = joinedStr.ElementAtOrDefault(0) + " " + joinedStr.ElementAtOrDefault(1),
                        Hours = string.Join(" ", joinedStr.Where((value, index) => index > 1 && index < joinedStr.Length && !value.Contains("CLOSED")))
                    };
                    list.Add(thexch);
                }
               
            }

            var ExchangeTravellinks = WebDriver.FindElements(By.CssSelector("a")).
                Where(x => x.Text.Contains("Go")).
                Select(x => x.GetAttribute("href")).
                ToList();
            var dict = new Dictionary<string, Exchange>();
            int index = 0;
            foreach (var exchangeTravelLink in ExchangeTravellinks)
            {
      
                dict.Add(exchangeTravelLink, list[index]);
            }
            return dict;
        }

        public List<Exchange> GetExchangeSummary(Dictionary<string, Exchange> ExchangesWithPaths)
        {
            List<Exchange> exchanges = new List<Exchange>();
            var now = DateTime.Now.ToString();
            foreach (var exchangewithpath in ExchangesWithPaths)
            {
                var exchange = new Exchange();
                exchange.contactInfo = new ContactInfo();
                exchange.TradingSchedule = new TradingSchedule();
                exchange.ExchangeBreakDown = new ExchangeBreakDown();
                exchange.CreatedDate = DateTime.Now;
                processHelper.Wait(180);
                WebDriver.Navigate().GoToUrl(exchangewithpath.Key);
                processHelper.Wait(180);
                var country = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[3]/div/div[1]/a"));
                exchange.Country = country ?? exchange.Country;
                exchange.ExchangeName = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/h1/b"));
                exchange.TimeZone = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[5]/div/div[1]/a"));
                exchange.MicCode = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[1]/div/div[1]"));
                exchange.Overview = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[5]/div"));
                exchange.TradingSchedule.CoreTradingSessions = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[6]/div[2]/div/div[2]/p[3]"));
                exchange.TradingSchedule.ExtendedHours = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[6]/div[2]/div/div[2]/p[2]"));
                exchange.TradingSchedule.PreTradingSession = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[6]/div[2]/div/div[2]/p[2]"));
                exchange.History = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[11]/div[1]"));
                exchange.ExchangeBreakDown.SubSidiaryMarkets = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/p[1]")) + " " + processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/p[2]"));
                exchange.ExchangeBreakDown.ExchangeTimeZonesInfo = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[8]/div[1]"));
                exchange.ExchangeBreakDown.MarketCapitilization = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[8]/div[2]"));
                processHelper.Wait(180);
                processHelper.ClickElement(By.XPath("/html/body/div[1]/div/div[2]/main/div[1]/ul/li[4]/a"));
                processHelper.Wait(180);
                exchange.contactInfo.Address = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[1]/address"));
                exchange.contactInfo.MarketComplianceEmail = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[1]/p[1]"));
                exchange.contactInfo.ClientRelationshipServicesEmail = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[1]/p[2]"));
                exchange.contactInfo.ContactNumber = processHelper.TryGetElementText(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[1]/p[4]"));
                exchange.contactInfo.Coordinates = string.Empty;
                processHelper.Wait(180);
                var website = WebDriver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/main/div[2]/div[1]")).FindElements(By.CssSelector("a")).FirstOrDefault();
                exchange.contactInfo.Website = website != null ? website.GetAttribute("href") : string.Empty; 
                exchanges.Add(exchange);
              
                var json = JsonConvert.SerializeObject(exchanges.OrderBy(x => x.CreatedDate));
                File.WriteAllText(@$"C:\Users\marti\source\repos\StockMachine.Trade\StockMachine.Trade\wwwroot\AllExchanges.json", json);
            }
            return exchanges;
        }
    }
}
