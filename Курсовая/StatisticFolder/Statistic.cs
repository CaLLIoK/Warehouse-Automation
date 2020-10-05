using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRoom
{
    [Table("SparePartOrder")]
    public class Statistic
    {
        [Key]
        public int OrderNumber { get; set; }
        public string UserLogin { get; set; }
        public string SparePartN {get;set;}
        public string CarModelName { get; set; }
        public int SparePartCount { get; set; }
        public double GeneralSum { get; set; }

        public Statistic() { }

        public Statistic(string userLogin, string sparePartN, string carModelname, int sparePartCount, double generalSum)
        {
            UserLogin = userLogin;
            SparePartN = sparePartN;
            CarModelName = carModelname;
            SparePartCount = sparePartCount;
            GeneralSum = generalSum;
        }
    }
}
