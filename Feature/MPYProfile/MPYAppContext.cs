using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYAppContext : ValueObject<MPYAppContext>
    {
        public AccountType AccounType { get; private set; }
        public DateTime LastLogin { get; private set; }
        public DateTime JoinDate { get; private set; }
        protected IEnumerable<MPYSubscriptionDetails> Subscriptions { get; private set; }
        public MPYInstructorDetails InstructorDetails { get; private set; }

        public MPYAppContext(AccountType accountType, DateTime lastLogin, DateTime joinDate, IEnumerable<MPYSubscriptionDetails> subscriptions, MPYInstructorDetails instructorDetails)
        {
            this.AccounType = accountType;
            this.LastLogin = lastLogin;
            this.JoinDate = joinDate;
            this.Subscriptions = subscriptions;
            this.InstructorDetails = instructorDetails ?? null;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { AccounType, LastLogin, JoinDate, Subscriptions, InstructorDetails };
        }

        public static MPYAppContext Create(AccountType accountType, DateTime lastLogin, DateTime joinDate, IEnumerable<MPYSubscriptionDetails> subscriptions, MPYInstructorDetails instructorDetails)
        {
            return new MPYAppContext(accountType, lastLogin, joinDate, subscriptions, instructorDetails);
        }
    }
}
