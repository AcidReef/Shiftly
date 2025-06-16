using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shiftly.Models
{
    public class Shift
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserId { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Note { get; set; }
    }
}