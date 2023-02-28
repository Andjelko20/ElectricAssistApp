using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DemoBack.Models
{
    public class Driver
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public int Championships { get; set; }
        public int Number { get; set; }
        public int YearOfBirth { get; set; }
        public string Team { get; set; } = null!;
    }
}
