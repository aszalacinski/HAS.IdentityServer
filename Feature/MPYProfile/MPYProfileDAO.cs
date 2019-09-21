using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYProfileDAO
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("lupdate")]
        public DateTime LastUpdate { get; set; }

        [BsonElement("pdetails")]
        public MPYPersonalDetailsDAO PersonalDetails { get; set; }

        [BsonElement("acdetails")]
        public MPYAppContextDAO AppContext { get; set; }

        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public static class MPYProfileDAOExtensons
    {
        public static MPYProfile ToEntity(this MPYProfileDAO dao) => MPYProfile.Create(dao.Id.ToString(), dao.LastUpdate, dao.PersonalDetails.ToValueObject(), dao.AppContext.ToValueObject());
    }
}
