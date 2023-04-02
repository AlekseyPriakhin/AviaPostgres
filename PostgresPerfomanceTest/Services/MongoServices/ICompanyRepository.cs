
using PostgresPerfomanceTest.Model.MongoModels;

namespace PostgresPerfomanceTest.Services.MongoServices
{
    public interface ICompanyRepository
    {
        public Task<long> CountAsync();

        public Task<List<MongoCompany>> GetAllAsync();

        public Task<MongoCompany> GetAsync(int id);

        public Task<IEnumerable<MongoCompany>> GetByCountryAsync(string country);

        public Task<IEnumerable<MongoCompany>> GetByNameAsync(string name);

        public Task InsertAsync(MongoCompany company);

        public Task UpdateAsync(MongoCompany company);

        public Task DeleteAsync(int id);
    }
}
