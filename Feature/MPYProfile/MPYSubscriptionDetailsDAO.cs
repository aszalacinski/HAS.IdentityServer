using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HAS.IdentityServer
{
    public class MPYSubscriptionDetailsDAO
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InstructorId { get; set; }
        
        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public static class MPYSubscriptionDetailsDAOExtensions
    {
        public static MPYSubscriptionDetails ToValueObject(this MPYSubscriptionDetailsDAO dao) => MPYSubscriptionDetails.Create(dao.InstructorId);

        public static IEnumerable<MPYSubscriptionDetails> ToValueObject(this IEnumerable<MPYSubscriptionDetailsDAO> dao) => dao.Select(x => x.ToValueObject()).AsEnumerable();
    }
}
