using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blazesoft.SlotMachine.Common.Data
{
    public class BaseMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string? Id { get; set; }

        [BsonElement("uuid")]
        public string? Uuid { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
