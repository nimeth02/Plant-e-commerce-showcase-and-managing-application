using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace crud_application.Models
{
    public class ItemWithCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

            public string Name { get; set; }

            public string SubName { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Category { get; set; }// This will hold the category ID

        public string CategoryName { get; set; } // This will hold the category name

            public string Description { get; set; }

            public int Quantity { get; set; }

            public float UnitPrice { get; set; }

        public string BestSelling { get; set; }

        public string NewReleased { get; set; }

        public List<string> ImageUrl { get; set; }
        
    }
}
