using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Services;

public interface ICompanyService
{
    public Task<IEnumerable<Company>> GetCompanies();
    public Task<IEnumerable<Company>> GetCompaniesByCountry(string country);
    public Task<IEnumerable<Company>> GetCompaniesByName(string name);
    public Task<Company> GetCompany(int id);
    public Task<Company> AddCompany(CompanyDto dto);
    public Task<Company> UpdateCompany(CompanyDto dto);
    public Task DeleteCompany(int id);
}