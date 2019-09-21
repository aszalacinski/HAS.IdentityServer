using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDb.IdentityUser;


namespace HAS.IdentityServer
{
    public class HASIdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMPYProfileService _mpyProfileSvc;

        public HASIdentityProfileService(
            UserManager<IdentityUser> userManager, 
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory, 
            IMPYProfileService mpyProfileSvc)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _mpyProfileSvc = mpyProfileSvc;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            // get user
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            // get mpy profile details
            var profile = await _mpyProfileSvc.GetProfileByEmailAddress(user.Email.NormalizedValue);
            
            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));

            if(profile.IsInstructor())
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "instructor"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "student"));
            }

            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email.NormalizedValue));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
