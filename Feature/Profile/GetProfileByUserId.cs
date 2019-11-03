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
    public class GetProfileByUserId
    {
        public class GetProfileByUserIdQuery : IRequest<GetProfileByUserIdResult>
        {
            public string UserId { get; private set; }

            public GetProfileByUserIdQuery(string userId) => UserId = userId;
        }

        public class GetProfileByUserIdResult
        {
            public string Id { get; private set; }
            public DateTime LastUpdate { get; private set; }
            public PersonalDetails PersonalDetails { get; private set; }
            public GetProfileByUserIdAppDetailsResult AppDetails { get; private set; }
        }

        public class GetProfileByUserIdAppDetailsResult
        {
            public string AccountType { get; private set; }
            public DateTime LastLogin { get; private set; }
            public DateTime JoinDate { get; private set; }
            public IEnumerable<SubscriptionDetails> Subscriptions { get; private set; }
            public InstructorDetails InstructorDetails { get; private set; }
        }

        public class GetProfileByUserIdQueryHandler : IRequestHandler<GetProfileByUserIdQuery, GetProfileByUserIdResult>
        {
            private readonly ProfileContext _db;
            private readonly MapperConfiguration _mapperConfiguration;

            public GetProfileByUserIdQueryHandler(ProfileContext db)
            {
                _db = db;
                _mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ProfileProfile>();
                    cfg.CreateMap<ProfileDAO, GetProfileByUserIdResult>()
                        .ForMember(m => m.AppDetails, opt => opt.MapFrom(src => src.AppDetails))
                        .ForMember(m => m.PersonalDetails, opt => opt.MapFrom(src => src.PersonalDetails));
                    cfg.CreateMap<AppDetailsDAO, GetProfileByUserIdAppDetailsResult>()
                        .ForMember(m => m.AccountType, opt => opt.MapFrom(source => Enum.GetName(typeof(AccountType), source.AccountType)));
                });
            }

            public async Task<GetProfileByUserIdResult> Handle(GetProfileByUserIdQuery query, CancellationToken cancellationToken)
            {
                var mapper = new Mapper(_mapperConfiguration);

                var projection = Builders<ProfileDAO>.Projection.Expression(x => mapper.Map<GetProfileByUserIdResult>(x));

                var profile = await _db.Profile
                                        .Find(x => x.PersonalDetails.UserId == query.UserId)
                                        .Project(projection)
                                        .FirstOrDefaultAsync();

                return profile;
            }
        }
    }
}
