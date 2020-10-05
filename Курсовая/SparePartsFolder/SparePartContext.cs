using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRoom
{
    public class SparePartContext : DbContext
    {
        public SparePartContext() : base("DefaultConnection") { }
        public DbSet<SparePart> SpareParts { get; set; }
    }
}
