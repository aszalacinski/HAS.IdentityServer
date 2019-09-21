using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public interface IMPYProfileService
    {
        Task<MPYProfile> GetProfileByUserId(string userId);
        Task<MPYProfile> GetProfileByEmailAddress(string emailAddress);

    }
}
