using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace crud_application.Models
{
    public class CarModel
    {
        
        public string Id { get; set; }

        public string Model { get; set; }

        public string NumberPlate { get; set; }
    }
}
