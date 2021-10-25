using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models.Data
{
    public class TradingCoreDbContext : DbContext
    {
        public TradingCoreDbContext(DbContextOptions<TradingCoreDbContext> options) : base(options)
        {
        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Commodities> Commodities { get; set; }
        public DbSet<TopMovers> TopMovers { get; set; }
    }
}
