using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PostgresPerfomanceTest.Data.MongoContext;
using PostgresPerfomanceTest.Model.MongoModels;

namespace PostgresPerfomanceTest.Services.MongoServices
{
    public class CompanyRepository : ICompanyRepository
    {
        const string FIND_ALL = "fildAll";
        const string FIND_BY_ID = "findById";
        const string FIND_BY_NAME = "findByName";
        const string FIND_BY_COUNTRY = "findByCountry";
        const string INSERT = "insert";
        const string UPDATE = "update";
        const string DELETE = "delete";

        private readonly IMongoContext _context;
        private readonly ILogger<CompanyRepository> _logger;
        protected IMongoCollection<MongoCompany> _collection;
        protected IMongoCollection<BsonDocument> _profileCollection;

        public CompanyRepository(IMongoContext context, ILogger<CompanyRepository> logger, IOptions<MongoSettings> options)
        {
            this._context = context;
            this._collection = _context.GetCollection<MongoCompany>(options.Value.Companies);
            this._profileCollection = _context.GetCollection<BsonDocument>(options.Value.Profile);
            this._logger = logger;
        }

        public async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(_ => true);

        public async Task<List<MongoCompany>> GetAllAsync()
        {
            var comment = GetNewComment();
            var options = new FindOptions();
            options.Comment = comment;

            var res = await _collection.Find(_ => true, options).ToListAsync();

            await PrintAsync(FIND_ALL, comment);

            return res;
        }

        public async Task<MongoCompany> GetAsync(int id)
        {
            var comment = GetNewComment();
            var options = new AggregateOptions();
            options.Comment = comment;

            var pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$lookup",
                                new BsonDocument("from", "flights")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "plane_id")
                                    .Add("as", "flights")));

            var lookup = new BsonDocument("$lookup",
                                new BsonDocument("from", "planes")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "company_id")
                                    .Add("pipeline", pipeline)
                                    .Add("as", "planes"));

            var result = await _collection
                                .Aggregate(options)
                                .Match(new BsonDocument("_id", id))
                                .AppendStage<MongoCompany>(lookup)
                                .SingleOrDefaultAsync();

            await PrintAsync(FIND_BY_ID, comment);

            return result;
        }

        public async Task<IEnumerable<MongoCompany>> GetByCountryAsync(string country)
        {
            var comment = GetNewComment();
            var options = new AggregateOptions();
            options.Comment = comment;

            /*
            var pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$lookup",
                                new BsonDocument("from", "flights")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "plane_id")
                                    .Add("as", "flights")));

            var lookup = new BsonDocument("$lookup",
                                new BsonDocument("from", "planes")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "company_id")
                                    .Add("pipeline", pipeline)
                                    .Add("as", "planes"));
            */

            var result = await _collection
                               .Aggregate(options)
                               .Match(new BsonDocument("country", country))
                               //.AppendStage<MongoCompany>(lookup)
                               .ToListAsync();

            await PrintAsync(FIND_BY_COUNTRY, comment, result.Count());

            return result.Take(100);
        }

        public async Task<IEnumerable<MongoCompany>> GetByNameAsync(string name)
        {
            var comment = GetNewComment();
            var options = new AggregateOptions();
            options.Comment = comment;

            /*
            var pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$lookup",
                                new BsonDocument("from", "flights")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "plane_id")
                                    .Add("as", "flights")));

            var lookup = new BsonDocument("$lookup",
                                new BsonDocument("from", "planes")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "company_id")
                                    //.Add("pipeline", pipeline)
                                    .Add("as", "planes"));
            */

            var result = await _collection
                               .Aggregate(options)
                               .Match(new BsonDocument("name", name))
                               //.AppendStage<MongoCompany>(lookup)
                               .ToListAsync();

            await PrintAsync(FIND_BY_NAME, comment, result.Count());

            return result.Take(100);
        }


        public async Task InsertAsync(MongoCompany company)
        {
            var comment = GetNewComment();
            var options = new InsertOneOptions();
            options.Comment = comment;

            await _collection.InsertOneAsync(company, options);

            await PrintAsync(INSERT, comment);
        }

        public async Task UpdateAsync(MongoCompany company)
        {
            var comment = GetNewComment();
            var options = new ReplaceOptions();
            options.Comment = comment;

            await _collection.ReplaceOneAsync(e => e.Id == company.Id, company, options);

            await PrintAsync(UPDATE, comment);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = GetNewComment();
            var options = new DeleteOptions();
            options.Comment = comment;

            await _collection.DeleteOneAsync(e => e.Id == id, options);

            await PrintAsync(DELETE, comment);
        }

        private string GetNewComment()
            => DateTime.Now.GetHashCode().ToString();

        private async Task PrintAsync(string operation, string comment, int find_elements = 0)
        {
            var filter = new BsonDocument { { "command.comment", comment } };
            var entries = await _profileCollection.Find(filter).ToListAsync();

            if (find_elements != 0)
                _logger.LogInformation($"Operation: {operation}, Time: {entries.Last()["millis"].ToString()}ms, Find elements: {find_elements}");
            else
                _logger.LogInformation($"Operation: {operation}, Time: {entries.Last()["millis"].ToString()}ms");
        }
    }
}
