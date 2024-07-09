using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace crud_application.Models
{
    public class ItemModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; }

        public string SubName { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string Category { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public float UnitPrice { get; set; }

        public string BestSelling { get; set; }

        public string NewReleased { get; set; }

        public List<string> ImageUrl { get; set; }
    }
}
