using AutoMapper;
using HAS.IdentityServer.Data;
using HAS.IdentityServer.Model;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static HAS.IdentityServer.Data.ProfileContext;

namespace HAS.IdentityServer
{
    public class GetProfileByEmailAddress
    {
        public class GetProfileByEmailAddressQuery : IRequest<Model.Profile> 
        { 
            public string EmailAddress { get; private set; }

            public GetProfileByEmailAddressQuery(string emailAddress) => EmailAddress = emailAddress;
        }
        
        public class GetProfileByEmailAddressQueryHandler : IRequestHandler<GetProfileByEmailAddressQuery, Model.Profile>
        {
            private readonly ProfileContext _db;
            private readonly MapperConfiguration _mapperConfiguration;

            public GetProfileByEmailAddressQueryHandler(ProfileContext db)
            {
                _db = db;
                _mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ProfileProfile>();
                });
            }

            public async Task<Model.Profile> Handle(GetProfileByEmailAddressQuery query, CancellationToken cancellationToken)
            {
                var mapper = new Mapper(_mapperConfiguration);

                var projection = Builders<ProfileDAO>.Projection.Expression(x => mapper.Map<Model.Profile>(x));

                var profile = await _db.Profile
                                        .Find(x => x.PersonalDetails.Email.ToUpper() == query.EmailAddress.ToUpper())
                                        .Project(projection)
                                        .FirstOrDefaultAsync();

                return profile;
            }
        }
    }
}
