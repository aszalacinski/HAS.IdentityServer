using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYSubscriptionDetails : ValueObject<MPYSubscriptionDetails>
    {
        public string InstructorId { get; private set; }
        
        protected MPYSubscriptionDetails() { }

        public MPYSubscriptionDetails(string instructorId) => this.InstructorId = instructorId;

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { InstructorId };
        }

        public static MPYSubscriptionDetails Create(string instructorId)
        {
            return new MPYSubscriptionDetails(instructorId);
        }
    }
}
