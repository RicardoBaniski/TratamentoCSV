using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TratamentoCSV
{
    class Daily
    {
        public string City { get; set; }
        public string ProvinceState { get; set; }
        public string CountryRegion { get; set; }
        public string LastUpdate { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public int Total { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public int Active { get; set; }
    }
}
