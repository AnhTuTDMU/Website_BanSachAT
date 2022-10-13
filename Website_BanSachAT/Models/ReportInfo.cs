using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website_BanSachAT.Models
{
    public class ReportInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Sum { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<decimal> AVG { get; set; }
        public Nullable<decimal> MAX { get; set; }
        public Nullable<decimal> MIN { get; set; }


    }
}