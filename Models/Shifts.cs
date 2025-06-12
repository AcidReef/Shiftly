using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shiftly.Models
{
    public class Shift
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        public required string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Note { get; set; }
    }
}