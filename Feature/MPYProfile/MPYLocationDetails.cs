using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYLocationDetails : ValueObject<MPYLocationDetails>
    {
        public string Country { get; private set; }
        public string City { get; private set; }
        public string StateProvince { get; private set; }

        public MPYLocationDetails(string country, string city, string stateProvince)
        {
            this.Country = country;
            this.City = city;
            this.StateProvince = stateProvince;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { Country, City, StateProvince };
        }

        public static MPYLocationDetails Create(string country, string city, string stateProvince)
        {
            return new MPYLocationDetails(country, city, stateProvince);
        }
    }
}
