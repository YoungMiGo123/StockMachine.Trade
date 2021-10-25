using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trading.Core.Models;

namespace StockMachine.Trade.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
   
   
        public IWebHostEnvironment HostingEnvironment { get; }
        public List<Exchange> exchanges { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
          
    
            HostingEnvironment = hostingEnvironment;
        }

        public void OnGet()
        {

        
        
        }
    }
}
