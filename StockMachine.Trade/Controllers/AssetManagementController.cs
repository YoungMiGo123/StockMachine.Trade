using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trading.Core.Interfaces;
using Trading.Core.Models;

namespace StockMachine.Trade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetManagementController : Controller
    {
        public AssetManagementController(ITradingCoreRepository tradingCoreRepository, ILogger<AssetManagementController> logger)
        {
            TradingCoreRepository = tradingCoreRepository;
            Logger = logger;
        }

        public ITradingCoreRepository TradingCoreRepository { get; }
        public ILogger<AssetManagementController> Logger { get; }

        [HttpPost("AddStock")]
        public async Task<IActionResult> AddStockAsync([FromBody] Stock stock)
        {
            try
            {
                if (stock != null)
                {

                    TradingCoreRepository.AddEntity(stock);
                    if (await TradingCoreRepository.SaveAllChangesAsync())
                    {
                        return Ok(stock);
                    }

                }

            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");

            }
            return BadRequest("The API failed to add the stock");
        }

        [HttpPost("AddStocks")]
        public async Task<IActionResult> AddStocksAsync([FromBody] IEnumerable<Stock> stocks)
        {
            try
            {
                if (stocks != null)
                {
                    await TradingCoreRepository.AddStocks(stocks);
                    if (await TradingCoreRepository.SaveAllChangesAsync())
                    {
                        return Ok(stocks);
                    }

                }

            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");

            }
            return BadRequest("The API failed to add the stock");
        }
        [HttpPost("AddCommodities")]
        public async Task<IActionResult> AddCommoditiesAsync([FromBody] IEnumerable<Commodities> commodities)
        {
            try
            {
                if (commodities != null)
                {
                    await TradingCoreRepository.AddCommodities(commodities);
                    if (await TradingCoreRepository.SaveAllChangesAsync())
                    {
                        return Ok(commodities);
                    }

                }
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");

            }
            return BadRequest();
        }

        [HttpPost("AddTopMovers")]
        public async Task<IActionResult> AddTopMoversAsync([FromBody] IEnumerable<TopMovers> topMovers)
        {
            try
            {
                if (topMovers != null)
                {
                    await TradingCoreRepository.AddTopMovers(topMovers);
               
                    if (await TradingCoreRepository.SaveAllChangesAsync())
                    {
                        return Ok(topMovers);
                    }

                }
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");

            }
            return BadRequest();
        }


        [HttpPost("AddCommodity")]
        public async Task<IActionResult> AddCommodityAsync([FromBody] Commodities commodities)
        {
            try
            {
                if (commodities != null)
                {
                    TradingCoreRepository.AddEntity(commodities);
                    if (await TradingCoreRepository.SaveAllChangesAsync())
                    {
                        return Ok(commodities);
                    }

                }
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");

            }
            return BadRequest();
        }
        [HttpGet("GetStocks")]
        public IActionResult GetStocks()
        {
            try
            {
                var results = TradingCoreRepository.GetAllStocks();
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
        [HttpGet("GetCommodities")]
        public IActionResult GetCommodities()
        {
            try
            {
                var results = TradingCoreRepository.GetCommodities();
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
        [HttpPost("GetStockBy/{Id:Guid}")]
        public IActionResult GetStockBy([FromBody] Guid Id)
        {
            try
            {
                var results = TradingCoreRepository.GetStock(Id);
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
        [HttpPost("GetCommoditiesBy/{Id:Guid}")]
        public IActionResult GetCommoditiesBy([FromBody] Guid Id)
        {
            try
            {
                var results = TradingCoreRepository.GetCommodities(Id);
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
        [HttpGet("GetTopMovers")]
        public IActionResult GetTopMovers()
        {
            try
            {
                var results = TradingCoreRepository.GetTopMovers();
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
        [HttpGet("GetTopMoversBy/{Type}")]
        public IActionResult GetTopMovers([FromBody] string Type)
        {
            try
            {
                var results = TradingCoreRepository.GetTopMoversBy(Type);
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
        [HttpGet("GetIndustryPeformance")]
        public IActionResult GetIndustryPeformance()
        {
            try
            {
                var results = TradingCoreRepository.GetIndustryPeformance();
                return Ok(results);
            }
            catch (Exception e)
            {
                Logger.LogError($"The API failed to add the stock {e}");
                return BadRequest();
            }
        }
    }
}
