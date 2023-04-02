using MongoDB.Driver;

namespace PostgresPerfomanceTest.Data.MongoContext
{
    public interface IMongoContext
    {
        IMongoCollection<MongoCompany> GetCollection<MongoCompany>(string name);
    }
}
