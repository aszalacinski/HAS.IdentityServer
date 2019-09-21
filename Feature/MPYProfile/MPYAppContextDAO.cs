using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYAppContextDAO
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
        public IEnumerable<MPYSubscriptionDetailsDAO> Subscriptions { get; set; }

        [BsonElement("idetails")]
        public MPYInstructorDetailsDAO InstructorDetails { get; set; }

        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public static class MPYAppContextDAOExtensions
    {
        public static MPYAppContext ToValueObject(this MPYAppContextDAO dao) => MPYAppContext.Create(dao.AccountType, dao.LastLogin, dao.JoinDate, dao.Subscriptions.ToValueObject(), dao.InstructorDetails.ToValueObject());
    }
}
