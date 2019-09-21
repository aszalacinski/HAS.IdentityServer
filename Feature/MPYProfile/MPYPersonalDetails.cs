using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYPersonalDetails : ValueObject<MPYPersonalDetails>
    {
        public string UserId { get; private set; }
        public string Email { get; private set; }
        public string ScreenName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public MPYLocationDetails Location { get; private set; }

        public MPYPersonalDetails(string userId, string email, string screenName, string firstName, string lastName, MPYLocationDetails location)
        {
            this.UserId = userId;
            this.Email = email;
            this.ScreenName = screenName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Location = location;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { UserId, Email, ScreenName, FirstName, LastName, Location };
        }

        public static MPYPersonalDetails Create(string userId, string email, string screenName, string firstName, string lastName, MPYLocationDetails location)
        {
            return new MPYPersonalDetails(userId, email, screenName, firstName, lastName, location);
        }
    }
}
