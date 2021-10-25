using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Trading.Core.Interfaces;

namespace Trading.Core.Models.Processors
{
    public class JSEStockManager : IStockManager
    {
        public JSEStockManager(string Url, IWebDriver webDriver)
        {
            this.Url = Url;
            WebDriver = webDriver;
            processHelper = new ProcessHelper(webDriver);
        }

        public string Url { get; set; }
        public IWebDriver WebDriver { get; }

        private readonly ProcessHelper processHelper;
        public List<Stock> GetStocksFromJson(string filepath)
        {
            try
            {
                List<Stock> ListOfStocks = JsonConvert.DeserializeObject<List<Stock>>(File.ReadAllText(filepath));
                ListOfStocks = UpdateStocksWithSubLink(ListOfStocks);
                ListOfStocks = GetCompanyProfile(ListOfStocks);
                return ListOfStocks;
            }
            catch { return new List<Stock>(); }
    
        }
        public List<Stock> GetAllAvailableStocks()
        {
            processHelper.GoToUrl(Url, 180);
       
            processHelper.Wait(30);
            var tableRows = WebDriver.FindElement(By.XPath("/html/body/div[2]/main/div/div[2]/section/div[2]/section/div[2]/div/div[2]")).FindElements(By.CssSelector("tr")).Where(x => !string.IsNullOrEmpty(x.Text));
            List<Stock> ListOfStocks = GetStocksFromWebElements(tableRows);
            ListOfStocks = ListOfStocks.Select(x =>
            {
                x.Symbol = x.Name.Split("-").FirstOrDefault().Trim() ?? string.Empty;
                x.Name = x.Name.Substring(x.Name.IndexOf('-') + 1) ?? string.Empty;
                return x;

            }).ToList();
            int index = 0, stockcount = ListOfStocks.Count();
            foreach (var element in ListOfStocks)
            {
                try
                {
                    if (index == 0) processHelper.ClickPolicyAgreements(By.XPath("/html/body/div[6]/div/div/div[2]/button[2]"));

                    SearchTheInputBox(element);
                    GetStockResultPages(element);
                    processHelper.Wait(40);
                    processHelper.GoToUrl(Url);
                    processHelper.Wait(90);
                    index++;
                }
                catch (Exception)
                {
                    processHelper.GoToUrl(Url);
                    index++;
                    continue;
                }

            }
            var updatedStocks = UpdateStocksWithSubLink(ListOfStocks);
            updatedStocks = GetCompanyProfile(updatedStocks);
            return updatedStocks;
        }

        private void SearchTheInputBox(Stock stock)
        {
            try
            {
                processHelper.Wait(180);
                processHelper.ClearInputText(By.Id("edit-keys--2"));
                processHelper.SendKeysToInput(By.Id("edit-keys--2"), stock.Symbol);
                processHelper.Wait(30);
                processHelper.ClickElement(By.Id("edit-submit--2"));
                processHelper.Wait(270);
            }catch
            {
                return;
            }
        }

        private void GetStockResultPages(Stock stock)
        {
            try
            {
                processHelper.ClickPolicyAgreements(By.XPath("/html/body/div[7]/div/div/div[2]/button[2]"));
                var firstSearchResult = WebDriver.FindElements(By.ClassName("search-result__type")).Where(x => !string.IsNullOrEmpty(x.Text)).Select(x => x).FirstOrDefault();
                var firstElement = firstSearchResult.Text;
                var resultlinks = WebDriver.FindElements(By.ClassName("search-result__title"))
                    .Select(x => x.FindElement(By.CssSelector("a")))
                    .Where(x => x != null)
                    .ToList();
                var resultCount = resultlinks.Count();
                if (!string.IsNullOrEmpty(firstElement) && firstElement == "Profile" && resultCount >= 2)
                {
                    stock.ProfileUrl = resultlinks.ElementAtOrDefault(0).GetAttribute("href");
                    stock.InstrumentUrl = resultlinks.ElementAtOrDefault(1).GetAttribute("href");
                }
                else if (resultCount >= 2)
                {
                    stock.ProfileUrl = resultlinks.ElementAtOrDefault(1).GetAttribute("href");
                    stock.InstrumentUrl = resultlinks.ElementAtOrDefault(0).GetAttribute("href");
                }
            }
            catch
            {
              
            }
        }




        private List<Commodities> GetCommoditiesFromWebElements(IEnumerable<IWebElement> tableRows)
        {
            bool first = true;
            var ListOfCommodities = new List<Commodities>();
            var dataTable = tableRows.Where(x => !string.IsNullOrWhiteSpace(x.Text)).Select(x => x.Text).ToList();
            var tableLen = dataTable.Count();
            for (int i = 0; i < tableLen; i++)
            {
                try
                {
                    var element = dataTable.ElementAt(i);
                    var commodity = new Commodities
                    {
                        CreatedDateTime = DateTime.Now
                    };

                    //add check to see if text is null or empty
                    var textFields = element.Split(new char[] { '\r', '\n', ' ' }) ?? new string[] { };
                    if (first)
                    {
                        first = false;
                        if (WebDriver.FindElements(By.XPath("/html/body/div[6]/div/div/div[2]/button[2]")).FirstOrDefault() is IWebElement ConfirmAgreePolicies)
                        {
                            processHelper.Wait(30);
                            ConfirmAgreePolicies.Click();
                        }
                        continue;
                    }
                    var LengthOfElementsFromSplit = textFields.Count();
                    if (LengthOfElementsFromSplit >= 6)
                    {
                        textFields = textFields.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        LengthOfElementsFromSplit = textFields.Count();
                        commodity.Name = string.Join(" ", textFields.Where((value, index) => index >= 0 && index < LengthOfElementsFromSplit - 5));
                        var stockvalue = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 5).Replace(",", "").Trim();
                        commodity.Price = double.Parse(stockvalue, CultureInfo.InvariantCulture);
                        commodity.ChangePercent = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 4);
                        commodity.Volume = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 3);
                        commodity.High = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 2);
                        commodity.Low = textFields.LastOrDefault();
                        ListOfCommodities.Add(commodity);
                    }
                }
                catch
                {
                    continue;
                }

            }
            return ListOfCommodities;
        }
        private List<Stock> GetStocksFromWebElements(IEnumerable<IWebElement> tableRows)
        {
            bool first = true;
            var ListOfStocks = new List<Stock>();
            var dataTable = tableRows.Where(x => !string.IsNullOrWhiteSpace(x.Text)).Select(x => x.Text).ToList();
            var tableLen = dataTable.Count();
            for (int i = 0; i < tableLen; i++)
            {
                try
                {
                    var element = dataTable.ElementAt(i);
                    var stock = new Stock
                    {
                        CreatedDateTime = DateTime.Now
                    };

                    //add check to see if text is null or empty
                    var textFields = element.Split(new char[] { '\r', '\n', ' ' }) ?? new string[] { };
                    if (first)
                    {
                        first = false;
                        if (WebDriver.FindElements(By.XPath("/html/body/div[6]/div/div/div[2]/button[2]")).FirstOrDefault() is IWebElement ConfirmAgreePolicies)
                        {
                            processHelper.Wait(30);
                            ConfirmAgreePolicies.Click();
                        }
                        continue;
                    }
                    var LengthOfElementsFromSplit = textFields.Count();
                    if (LengthOfElementsFromSplit >= 6)
                    {
                        textFields = textFields.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        LengthOfElementsFromSplit = textFields.Count();
                        stock.Name = string.Join(" ", textFields.Where((value, index) => index >= 0 && index < LengthOfElementsFromSplit - 5));
                        var stockvalue = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 5).Replace(",", "").Trim();
                        stock.Price = double.Parse(stockvalue, CultureInfo.InvariantCulture);
                        stock.ChangePercent = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 4);
                        stock.Volume = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 3);
                        stock.High = textFields.ElementAtOrDefault(LengthOfElementsFromSplit - 2);
                        stock.Low = textFields.LastOrDefault();
                        ListOfStocks.Add(stock);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return ListOfStocks;
        }
        private List<Stock> GetCompanyProfile(List<Stock> stocks)
        {

            foreach (var stock in stocks)
            {
                try
                {
                    if (string.IsNullOrEmpty(stock.ProfileUrl)) continue;
                    processHelper.GoToUrl(stock.ProfileUrl);
                    processHelper.ClickPolicyAgreements(By.Id("popup-buttons"));
                    stock.CompanyRegistrationNumber = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[1]/div[2]/div/div[2]/span[2]/div"));
                    stock.CompanyOverview = processHelper.GetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[2]/div/div/div"));
                    stock.ContanctInfo = processHelper.GetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[3]/div/div[1]"));
                    stock.PostalAddress = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[3]/div/div[2]/div"));
                    stock.PhysicalAddress = processHelper.GetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[3]/div/div[3]/div"));
                    stock.RegulatoryDocuments = processHelper.GetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]"));
                }
                catch
                {
                    continue;
                }

            }
     
            return stocks;
        }
        private List<Stock> UpdateStocksWithSubLink(List<Stock> list)
        {
            var stocksWithSublinks = list.Where(x => !string.IsNullOrEmpty(x.InstrumentUrl) && !string.IsNullOrEmpty(x.ProfileUrl)).Where(x => x.InstrumentUrl.Contains("instruments") || x.ProfileUrl.Contains("instruments") && x.SectorPeers == null);
            foreach (var stock in stocksWithSublinks)
            {
                try
                {
                    var path = "";
                    if (stock.InstrumentUrl.Contains("instruments")) { path = stock.InstrumentUrl; }
                    else if (stock.ProfileUrl.Contains("instruments")) { path = stock.ProfileUrl; }
                    else { continue; }
                    processHelper.GoToUrl(path);
                    processHelper.ClickPolicyAgreements(By.Id("popup-buttons"));
                    stock.ProfileUrl = processHelper.TryGetAttribute(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[1]/div[1]/div/a"), "href") ?? stock.ProfileUrl;
                    var priceoverview = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[2]/div[2]/div/div[1]/div[1]/div[2]/div/div/span/div[2]")).Split("\r\n");

                    stock.Price = double.Parse(string.IsNullOrWhiteSpace(priceoverview.ElementAtOrDefault(1)) ? "0" : priceoverview.ElementAt(1).Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                    stock.Low = priceoverview.ElementAtOrDefault(3) == null ? stock.Low : priceoverview.ElementAtOrDefault(3).Replace(",", "").Trim();
                    stock.High = priceoverview.ElementAtOrDefault(5) == null ? stock.High : priceoverview.ElementAtOrDefault(5).Replace(",", "").Trim();
                    stock.Volume = string.IsNullOrEmpty(priceoverview.ElementAtOrDefault(7)) ? stock.Volume : priceoverview.ElementAt(7).Replace(",", "").Trim();
                    var priceHistory = WebDriver.FindElement(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[2]/div[1]/ul/li[2]/a"));
                    priceHistory.Click();
                    stock.Sector = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[1]/div[2]/div/div[1]/span[2]"));
                    stock.Industry = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[1]/div[2]/div/div[2]/span[2]"));
                    stock.ListingDate = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[1]/div[2]/div/div[3]/span[2]/div/time"));
                    stock.CreatedDateTime = DateTime.Now;
                    var sectorPeers = WebDriver.FindElement(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div[2]/div[1]/ul/li[4]/a/div/div"));
                    sectorPeers.Click();
                    var listofpeers = WebDriver.FindElements(By.CssSelector("tr"))
                        .Select(x => x.Text)
                        .Select(x => x.Split("\r\n"))
                        .Where(x => x.Length >= 3)
                        .Select(x => new StockPeer()
                        {
                            Code = x.ElementAt(0).Split(" ").ElementAt(0).Trim(),
                            Name = string.Join(" ", x.ElementAt(0).Split(" ").Where((value, index) => index > 0 && index < (x.ElementAt(0).Split(" ").Length - 1))),
                            Price = double.Parse(x.ElementAt(0).Split(" ").LastOrDefault().Trim().Replace(",", ""), CultureInfo.InvariantCulture),
                            ChangePercent = x.ElementAt(1).Trim(),
                            Change = double.Parse(x.ElementAt(2).Trim().Replace(",", ""), CultureInfo.InvariantCulture)
                        }).ToList();
                    stock.SectorPeers = listofpeers;
   
                }
                catch
                {
                    continue;

                }
            }

            return list;

        }
        private void WriteToFile(string json, string path)
        {
            File.WriteAllText(path, json);
        }
        private Stock UpdateCompanyDetails(Stock stock)
        {

            stock.Symbol = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[1]/div[2]/div/div/span[2]"));
            stock.CompanyOverview = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[2]/div/div/div"));
            stock.ContanctInfo = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[3]/div/div[1]"));
            stock.PostalAddress = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[3]/div/div[2]/div"));
            stock.PhysicalAddress = processHelper.TryGetElementText(By.XPath("/html/body/div[2]/main/div/div/section/div[2]/div/div/div/div[2]/div[1]/div/div[3]/div/div[3]/div"));
            stock.RegulatoryDocuments = processHelper.TryGetElementText(By.ClassName("view"));
            return stock;
        }

        public List<Commodities> GetCommodities()
        {
            try
            {
                processHelper.GoToUrl(Url, 90);
                processHelper.ClickPolicyAgreements(By.Id("popup-buttons"));
                processHelper.ClickElement(By.XPath("/html/body/div[2]/main/div/div[2]/section/div[2]/section/div[1]/div[3]/button/i"));
                var element = WebDriver.FindElements(By.ClassName("tab-title")).Where(x => x.Text.Contains("Commodities")).FirstOrDefault();
                var commodities = new List<Commodities>();
                if (element is IWebElement commoditiesTab)
                {
                    commoditiesTab.Click();
                    var tableRows = WebDriver.FindElement(By.XPath("/html/body/div[2]/main/div/div[2]/section/div[2]/section/div[2]/div/div[2]")).FindElements(By.CssSelector("tr")).Where(x => !string.IsNullOrEmpty(x.Text));
                    commodities = GetCommoditiesFromWebElements(tableRows);
                }
                return commodities;
            }
            catch
            {
                return new List<Commodities>();
            }
   
        }
        public Dictionary<string, List<TopMovers>> GetAllTopMoversMetrics()
        {
            var dict = new Dictionary<string, List<TopMovers>>();
            try
            {
                processHelper.GoToUrl(Url, 90);
                processHelper.ClickPolicyAgreements(By.Id("popup-buttons"));
                dict.Add("percent", GetTopMoversByPercents());
                dict.Add("price", GetTopMoversByPriceChanges());
                dict.Add("volume", GetTopMoversByVolumes());
                return dict;
            }
            catch { return dict; }
        }
        private List<TopMovers> GetMovers(string type)
        {
            try
            {
               
                var deltas = WebDriver.FindElement(By.Id($"diptych-first")).FindElements(By.ClassName("delta"));
                var results = deltas
                    .Where(x => !string.IsNullOrEmpty(x.Text))
                    .Select(x => x.Text);
                var links = deltas
                    .Select(x => x.FindElement(By.CssSelector("a")))
                    .Select(x => x.GetAttribute("href"));

                var movers = new List<TopMovers>();
                int index = 0;
                foreach (var item in results)
                {
                    var result = item.Split(new char[] { ' ', '\n', '\t', '\r' }).Where(x => !string.IsNullOrEmpty(x)).ToList();

                    var topmover = new TopMovers()
                    {
                        Symbol = string.Join(" ", result.Where((value, index) => index < result.Count() - 1)),
                        ChangeFactor = double.Parse(result.LastOrDefault() == null ? "0" : result.LastOrDefault().Replace("%", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture),
                        Change = double.Parse(result.ElementAtOrDefault(result.Count() - 2) == null ? "0" : result.ElementAt(result.Count() - 2).Replace("%", "").Replace(",", "").Trim()),
                        ChangeFactorType = type,
                        Url = links.ElementAtOrDefault(index) ?? string.Empty,
                        CreatedDate = DateTime.Now
                    };
                    movers.Add(topmover);
                };

                return movers;

            }
            catch
            {
                return new List<TopMovers>();
            }
  
        }
        private List<TopMovers> GetTopMoversByPercents()
        {
            try
            {
                var movers = GetMovers("percent");
                if (movers != null) return movers;
            }
            catch { }
            return new List<TopMovers>();
        }

        private List<TopMovers> GetTopMoversByPriceChanges()
        {
            try
            {
                processHelper.TryClickElement(processHelper.GetElement(By.XPath("/html/body/div[2]/section[2]/div/div[1]/div/section/div[1]/button/i")));
                var elementToClick = WebDriver.FindElements(By.XPath("/html/body/div[2]/section[2]/div/div[1]/div/section/div[1]/ul/li")).Where(x => x.Text.Contains("change")).FirstOrDefault();
                if (elementToClick is IWebElement element)
                {
                    element.Click();
                    var movers = GetMovers("price");
                    return movers;
                }

            }
            catch
            {
            }
            return new List<TopMovers>();
        }

        private List<TopMovers> GetTopMoversByVolumes()
        {
            try
            {
                processHelper.TryClickElement(processHelper.GetElement(By.XPath("/html/body/div[2]/section[2]/div/div[1]/div/section/div[1]/button/i")));
                var elementToClick = WebDriver.FindElements(By.XPath("/html/body/div[2]/section[2]/div/div[1]/div/section/div[1]/ul/li")).Where(x => x.Text.Contains("volume")).FirstOrDefault();
                if (elementToClick is IWebElement element)
                {
                    element.Click();
                    var movers = GetMovers("volume");
                    return movers;
                }
            }
            catch
            {
           
            }
            return new List<TopMovers>();
        }
    }
}
