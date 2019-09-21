using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYInstructorDetails : ValueObject<MPYInstructorDetails>
    {
        public DateTime? StartDate { get; private set; }

        protected MPYInstructorDetails() { }

        public MPYInstructorDetails(DateTime? startDate)
        {
            this.StartDate = startDate;
        }
        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { StartDate };
        }

        public static MPYInstructorDetails Create(DateTime? startDate)
        {
            return new MPYInstructorDetails(startDate);
        }
    }
}
