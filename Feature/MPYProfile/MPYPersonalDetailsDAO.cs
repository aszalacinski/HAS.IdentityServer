using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYPersonalDetailsDAO
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
        public MPYLocationDetailsDAO Location { get; set; }


        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public static class MPYPersonalDetailsDAOExtensions
    {
        public static MPYPersonalDetails ToValueObject(this MPYPersonalDetailsDAO dao) => MPYPersonalDetails.Create(dao.UserId, dao.Email, dao.ScreenName, dao.FirstName, dao.LastName, dao.Location.ToValueObject());
    }
}
