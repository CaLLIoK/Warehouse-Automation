using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRoom
{
    public class StatisticContext : DbContext
    {
        public StatisticContext() : base("DefaultConnection") { }
        public DbSet<Statistic> Statistics { get; set; }
    }
}
