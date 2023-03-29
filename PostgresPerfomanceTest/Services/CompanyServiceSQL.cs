using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Services;

public class CompanyServiceSQL : ICompanyService
{
    private readonly AviaDbContext _context;

    public CompanyServiceSQL(AviaDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Company>> GetCompanies()
    {
        var items = await _context.Companies
            .Include(e=>e.Planes)
            .ThenInclude(e=>e.Flights)
            .ToListAsync();
        return items.Take(100);
    }

    public async Task<IEnumerable<Company>> GetCompaniesByCountry(string country)
    {
        var items = await _context.Companies.Where(e=>e.Country == country)
            .Include(e=>e.Planes)
            .ThenInclude(e=>e.Flights)
            .ToListAsync();
        return items.Take(100);
    }

    public async Task<IEnumerable<Company>> GetCompaniesByName(string name)
    {
        var items = await _context.Companies.Where(e=>e.Name == name)
            .Include(e=>e.Planes)
            .ThenInclude(e=>e.Flights)
            .ToListAsync();
        return items.Take(100);
    }

    public async Task<Company> GetCompany(int id)
    {
        var company = await _context.Companies.Where(e=>e.CompanyId == id)
            .Include(e=>e.Planes)
            .ThenInclude(e=>e.Flights)
            .FirstOrDefaultAsync();

        return company;
    }

    public async Task<Company> AddCompany(CompanyDto dto)
    {
        var company = new Company
        {
            Name = dto.Name,
            Code = dto.Code,
            Country = dto.Country,
            YearOfFoundation = dto.YearOfFoundation
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return await GetCompany(company.CompanyId);
    }

    public async Task<Company> UpdateCompany(CompanyDto dto)
    {
        var company = await _context.Companies.FindAsync(dto.CompanyId);
        if (company is null) return null;

        company.Name = dto.Name;
        company.Code = dto.Code;
        company.YearOfFoundation = dto.YearOfFoundation;
        company.Country = dto.Country;

        _context.Companies.Update(company);
        await _context.SaveChangesAsync();

        return await GetCompany(company.CompanyId);
    }

    public async Task DeleteCompany(int id)
    {
        var company = await _context.Companies.Where(e=>e.CompanyId == id)
            .Include(e=>e.Planes).ThenInclude(e=>e.Flights)
            .FirstOrDefaultAsync();
        if(company is null) return;
        foreach (var plane in company.Planes)
        {
            foreach (var flight in plane.Flights)
            {
                _context.Remove(flight);
            }

            _context.Remove(plane);
        }
        
        
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }
}