using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYInstructorDetailsDAO
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("sdate")]
        public DateTime? StartDate { get; set; }

        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public static class MPYInstructorDetailsDAOExtensions
    {
        public static MPYInstructorDetails ToValueObject(this MPYInstructorDetailsDAO dao) => MPYInstructorDetails.Create(dao.StartDate);
    }
}
