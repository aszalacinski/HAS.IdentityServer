using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYLocationDetailsDAO
    {
        [BsonElement("ctry")]
        public string Country { get; set; }
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("strp")]
        public string StateProvince { get; set; }
        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public static class MPYLocationDetailsDAOExtensions
    {
        public static MPYLocationDetails ToValueObject(this MPYLocationDetailsDAO dao) => MPYLocationDetails.Create(dao.Country, dao.City, dao.StateProvince);
    }
}
