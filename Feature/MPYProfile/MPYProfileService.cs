using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYProfileService : IMPYProfileService
    {
        private readonly IMPYProfileRepository _repository;

        public MPYProfileService(IMPYProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<MPYProfile> GetProfileByEmailAddress(string emailAddress)
        {
            MPYProfile profile = await _repository.Find(x => x.PersonalDetails.Email.ToUpper() == emailAddress.ToUpper());

            if(profile != null)
            {
                return profile;
            }

            return null;
        }

        public async Task<MPYProfile> GetProfileByUserId(string userId)
        {
            MPYProfile profile = await _repository.Find(x => x.PersonalDetails.UserId == userId);

            if (profile != null)
            {
                return profile;
            }

            return null;
        }
    }
}
