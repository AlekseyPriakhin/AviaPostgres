using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Controllers;

[ApiController,Route("/company")]
public class CompanyController : ControllerBase
{
    private readonly AviaDbContext _context;
    public CompanyController(AviaDbContext context)
    {
        _context = context;
    }

    [HttpGet("by_name")]
    public async Task<IActionResult> GetCompaniesByName(string name)
    {
        var items = await _context.Companies.Where(e => e.Name == name)
            .ToListAsync();

        return Ok(items.Take(100));
    }
    
    [HttpGet("by_country")]
    public async Task<IActionResult> GetCompaniesByCountry(string country)
    {
        var items = await _context.Companies.Where(e => e.Country == country).Include(e=>e.Planes)
            .ThenInclude(e=>e.Flights)
            .ToListAsync();

        return Ok(items.Take(100));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var company = await _context.Companies.Where(e=>e.CompanyId == id)
            .FirstOrDefaultAsync();
        if (company is null)
        {
            return NotFound();
        }

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CompanyDto dto)
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

        return await Get(company.CompanyId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,CompanyDto dto)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company is null) return NotFound();

        company.Name = dto.Name;
        company.Code = dto.Code;
        company.YearOfFoundation = dto.YearOfFoundation;
        company.Country = dto.Country;

        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
        return Ok(company);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company is null) return NotFound();
        
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return Ok();
    }

}