using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TratamentoCSV
{
    class Daily
    {
        public string ProvinceState { get; set; }
        public string CountryRegion { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
    }
}
