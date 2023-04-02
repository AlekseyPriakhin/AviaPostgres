using MongoDB.Bson.Serialization.Attributes;

namespace PostgresPerfomanceTest.Model.MongoModels
{
    public class MongoCompany
    {
        public MongoCompany()
        {
            this.Planes = new HashSet<MongoPlane>();
        }

        [BsonId]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("yearOfFoundation")]
        public int YearOfFoundation { get; set; }

        [BsonElement("planes")]
        public virtual ICollection<MongoPlane>? Planes { get; set; }
    }
}
