using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shiftly.Models
{
    public class LeaveRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"
        public string? Reason { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}