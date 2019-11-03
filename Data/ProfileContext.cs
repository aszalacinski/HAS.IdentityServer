using HAS.IdentityServer.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace HAS.IdentityServer.Data
{
    public class ProfileContext
    {
        private readonly DbContext _db;
        private IMongoCollection<ProfileDAO> _profile;

        public IMongoCollection<ProfileDAO> Profile { get; }

        public ProfileContext(IConfiguration configuration)
        {
            _db = DbContext.Create(configuration["MongoDB:Profile:Database:Name"], configuration["MongoDB:Profile:ConnectionString"]);
            _profile = _db.Database.GetCollection<ProfileDAO>(configuration["MongoDB:Profile:Database:Collection"]);
            Profile = _profile;
        }

        [BsonIgnoreExtraElements]
        public class ProfileDAO
        {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
            [BsonElement("lupdate")]
            public DateTime LastUpdate { get; set; }

            [BsonElement("pdetails")]
            public PersonalDetailsDAO PersonalDetails { get; set; }

            [BsonElement("acdetails")]
            public AppDetailsDAO AppDetails { get; set; }
        }

        [BsonIgnoreExtraElements]
        public class PersonalDetailsDAO
        {
            [BsonElement("u_id")]
            public string UserId { get; set; }
            [BsonElement("email")]
            public string Email { get; set; }
            [BsonElement("sname")]
            public string ScreenName { get; set; }
            [BsonElement("fname")]
            public string FirstName { get; set; }
            [BsonElement("lname")]
            public string LastName { get; set; }
            [BsonElement("loc")]
            public LocationDetailsDAO Location { get; set; }

        }

        [BsonIgnoreExtraElements]
        public class LocationDetailsDAO
        {
            [BsonElement("ctry")]
            public string Country { get; set; }
            [BsonElement("city")]
            public string City { get; set; }
            [BsonElement("strp")]
            public string StateProvince { get; set; }
        }

        [BsonIgnoreExtraElements]
        public class AppDetailsDAO
        {
            [BsonRepresentation(BsonType.String)]
            [BsonElement("type")]
            public AccountType AccountType { get; set; }

            [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
            [BsonElement("llogin")]
            public DateTime LastLogin { get; set; }

            [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
            [BsonElement("join")]
            public DateTime JoinDate { get; set; }

            [BsonElement("subs")]
            public IEnumerable<SubscriptionDetailsDAO> Subscriptions { get; set; }

            [BsonElement("idetails")]
            public InstructorDetailsDAO InstructorDetails { get; set; }

        }

        [BsonIgnoreExtraElements]
        public class SubscriptionDetailsDAO
        {
            [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
            [BsonRepresentation(BsonType.ObjectId)]
            public string InstructorId { get; set; }

            [BsonElement("classes")]
            public IEnumerable<ClassDetailsDAO> Classes { get; set; }

        }

        [BsonIgnoreExtraElements]
        public class ClassDetailsDAO
        {
            [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
            [BsonRepresentation(BsonType.ObjectId)]
            public string ClassId { get; set; }

            [BsonElement("like")]
            public bool Liked { get; set; }

        }

        [BsonIgnoreExtraElements]
        public class InstructorDetailsDAO
        {
            [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
            [BsonElement("sdate")]
            public DateTime? StartDate { get; set; }
        }
    }
}
