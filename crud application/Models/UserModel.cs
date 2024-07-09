using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace crud_application.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
