using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;
using PostgresPerfomanceTest.Services;

namespace PostgresPerfomanceTest.Controllers;

[ApiController,Route("/company")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _service;
    public CompanyController(ICompanyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var items = await _service.GetCompanies();

        return Ok(items);
    }
    
    [HttpGet("by_name")]
    public async Task<IActionResult> GetCompaniesByName(string name)
    {
        var items = await _service.GetCompaniesByName(name);

        return Ok(items);
    }
    
    [HttpGet("by_country")]
    public async Task<IActionResult> GetCompaniesByCountry(string country)
    {
        var items = await _service.GetCompaniesByCountry(country);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompany(int id)
    {
        var company = await _service.GetCompany(id);
        if (company is null)
        {
            return NotFound();
        }

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CompanyDto dto)
    {
        var company = await _service.AddCompany(dto);
        return Ok(company);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,CompanyDto dto)
    {
        dto.CompanyId = id;
        var company = await _service.UpdateCompany(dto);
        return Ok(company);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteCompany(id);
        return Ok();
    }

}