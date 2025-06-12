using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shiftly.Models
{
    public class SwapRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        public string FromUserId { get; set; } = default!;    // kto inicjuje zamianę
        public string ToUserId { get; set; } = default!;      // z kim chce się zamienić
        public string FromShiftId { get; set; } = default!;   // jego obecna zmiana
        public string ToShiftId { get; set; } = default!;     // zmiana do przejęcia
        public string Status { get; set; } = "Pending";       // "Pending", "Accepted", "Rejected"
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}