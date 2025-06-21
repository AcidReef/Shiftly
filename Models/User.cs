using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
     public string Id { get; set; } = string.Empty; // Domyślna wartość

    public string UserName { get; set; } = string.Empty; // Domyślna wartość
    public string Email { get; set; } = string.Empty; // Domyślna wartość
    public string Role { get; set; } = string.Empty; // Domyślna wartość
    public string PasswordHash { get; set; } = string.Empty; // Domyślna wartość
}