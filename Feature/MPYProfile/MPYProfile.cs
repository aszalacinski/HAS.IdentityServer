using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYProfile : Entity<string>
    {
        private DateTime LastUpdate { get; set; }
        private MPYPersonalDetails PersonalDetails { get; set; }
        private MPYAppContext AppContext { get; set; }

        public MPYProfile(string id, DateTime lastUpdate, MPYPersonalDetails personalDetails, MPYAppContext appContext)
            : base(id)
        {
            LastUpdate = lastUpdate;
            PersonalDetails = personalDetails;
            AppContext = appContext;
        }

        public bool IsInstructor()
        {
            return AppContext.AccounType.Equals(AccountType.INSTRUCTOR);
        }

        public static MPYProfile Create(string id, DateTime lastUpdate, MPYPersonalDetails personalDetails, MPYAppContext appContext)
        {
            return new MPYProfile(id, lastUpdate, personalDetails, appContext);
        }
    }
}
