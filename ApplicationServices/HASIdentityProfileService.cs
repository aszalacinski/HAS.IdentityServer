using HAS.IdentityServer.Model;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static HAS.IdentityServer.GetProfileByEmailAddress;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDb.IdentityUser;


namespace HAS.IdentityServer
{
    public class HASIdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMediator _mediator;

        public HASIdentityProfileService(
            UserManager<IdentityUser> userManager, 
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IMediator mediator)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _mediator = mediator;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            // get user
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            // get mpy profile details
            var profile = await _mediator.Send(new GetProfileByEmailAddressQuery(user.Email.NormalizedValue));

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
            claims.Add(new Claim(JwtClaimTypes.Role, "student"));

            if (profile.IsInstructor())
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "instructor"));
            }

            if(IsAdminEmailAddress(user.Email.NormalizedValue))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
            }

            if(IsSuperAdminEmail(user.Email.NormalizedValue))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
                claims.Add(new Claim(JwtClaimTypes.Role, "superadmin"));
            }

            if(PersonalEmails.Any(x => x.ToUpper() == user.Email.NormalizedValue.ToUpper()))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
                claims.Add(new Claim(JwtClaimTypes.Role, "superadmin"));
            }

            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email.NormalizedValue));

            context.IssuedClaims = claims;
        }

        private List<string> SuperAdminEmailAddresses = new List<string> { "aarron.szalacinski@happyappsoftware.com", "tammy.naylor@happyappsoftware.com" };
        private List<string> AdminEmailDomains = new List<string> { "happyappsoftware.com" };

        private List<string> PersonalEmails = new List<string> { "aszalacinski@outlook.com", "tamara.l.naylor@gmail.com" };

        private bool IsAdminEmailAddress(string email)
        {
            var domain = email.Split('@')[1];

            return AdminEmailDomains.Any(x => x.ToUpper() == domain.ToUpper());
        }

        private bool IsSuperAdminEmail(string email)
        {
            return SuperAdminEmailAddresses.Any(x => x.ToUpper() == email.ToUpper());
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
