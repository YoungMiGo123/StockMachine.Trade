using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using Trading.Core.Models.Processors;
using System.Collections.Generic;
using Trading.Core.Models.Http;
using Trading.Core.Models;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using System;

namespace Automation.Trade
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("-incognito");
                chromeOptions.AddArguments("disable-infobars");
                chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");

                IWebDriver webdriver = new ChromeDriver(chromeOptions);


                var url = "http://www.jse.co.za/";
                var path = @$"..\..\..\AllStocks_Withsublinks.json";
                var httpmanager = new HttpManager();
                JSEStockManager stockManager = new JSEStockManager(url, webdriver);

                var allStocks = stockManager.GetStocksFromJson(path);
                foreach (var i in allStocks)
                {
                    var jsonStock = JsonConvert.SerializeObject(i);
                    var stock = await httpmanager.PostAsync<Stock>("https://localhost:44397/api/AssetManagement/AddStock", jsonStock);
                }

                var commodities = stockManager.GetCommodities();
                var json = JsonConvert.SerializeObject(commodities);
                await httpmanager.PostAsync<List<Commodities>>("https://localhost:44397/api/AssetManagement/AddCommodities", json);

                var topmovers = stockManager.GetAllTopMoversMetrics();
                foreach (var topm in topmovers)
                {
                    json = JsonConvert.SerializeObject(topm.Value);
                    var obj = await httpmanager.PostAsync<List<TopMovers>>("https://localhost:44397/api/AssetManagement/AddTopMovers", json);
                }
            }
            catch(Exception e)
            {

            }
        

        }
    }
}
