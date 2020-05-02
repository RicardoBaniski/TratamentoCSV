using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TratamentoCSV
{
    class Daily
    {
        private string city;
        private string provinceState;
        private string countryRegion;
        private DateTime lastUpdate;
        private string lat;
        private string long_;
        private int confirmed;
        private int deaths;
        private int recovered;
        private int active;
        private string archive;

        public string City { get => city; set => city = value; }
        public string ProvinceState { get => provinceState; set => provinceState = value; }
        public string CountryRegion { get => countryRegion; set => countryRegion = value; }
        public DateTime LastUpdate { get => lastUpdate; set => lastUpdate = value; }
        public string Lat { get => lat; set => lat = value; }
        public string Long_ { get => long_; set => long_ = value; }
        public int Confirmed { get => confirmed; set => confirmed = value; }
        public int Deaths { get => deaths; set => deaths = value; }
        public int Recovered { get => recovered; set => recovered = value; }
        public int Active { get => active; set => active = value; }
        public string Archive { get => archive; set => archive = value; }
    }
}
