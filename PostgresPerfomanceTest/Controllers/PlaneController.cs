using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Controllers;

[ApiController, Route("/plane")]
public class PlaneController : ControllerBase
{

    private readonly AviaDbContext _context;
    
    public PlaneController(AviaDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _context.Planes.ToListAsync();
        return Ok(items.Take(100));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlane(int id)
    {
        var plane = await _context.Planes.Where(e=>e.PlaneId == id)
            .Include(e=>e.Flights).Include(e=>e.Company)
            .Select(e=> new
            {
                e.Name,
                e.Code,
                e.PlaneId,
                e.Company,
                e.Flights
            })
            .FirstOrDefaultAsync();
        if (plane is null) return NotFound();
        return Ok(plane);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PlaneDto dto)
    {
        var plane = new Plane
        {
            Name = dto.Name,
            Code = dto.Code,
            CompanyId = dto.CompanyId
        };
        _context.Planes.Add(plane);
        await _context.SaveChangesAsync();
        return Ok();
        //return CreatedAtAction(nameof(GetPlane), new {id = plane.PlaneId}, plane);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PlaneDto dto)
    {
        var plane = await _context.Planes.FindAsync(id);
        if (plane is null) return NotFound();

        plane.Code = dto.Code;
        plane.Name = dto.Name;
        plane.CompanyId = dto.CompanyId;

        try
        {
            _context.Planes.Update(plane);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest();
        }
        
        return Ok(plane);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plane = await _context.Planes.FindAsync(id);
        if (plane is null) return NotFound();
        _context.Planes.Remove(plane);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
}