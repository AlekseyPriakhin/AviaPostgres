using MongoDB.Bson.Serialization.Attributes;

namespace PostgresPerfomanceTest.Model.MongoModels
{
    public class MongoPlane
    {
        public MongoPlane()
        {
            this.Flights = new HashSet<MongoFlight>();
        }

        [BsonId]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("company_id")]
        public int? CompanyId { get; set; }

        [BsonElement("flights")]
        public ICollection<MongoFlight>? Flights { get; set; }
    }
}
