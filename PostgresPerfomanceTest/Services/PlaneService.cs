using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Services;

public class PlaneService : IPlaneService
{
    private readonly AviaDbContext _context;

    public PlaneService(AviaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Plane>> GetPlanes()
    {
        var items = await _context.Planes.ToListAsync();
        return items.Take(100);
    }

    public async Task<Plane> GetPlane(int id)
    {
        var plane = await _context.Planes.Where(e=>e.PlaneId == id)
            .Include(e=>e.Flights).Include(e=>e.Company)
            .FirstOrDefaultAsync();
        return plane;
    }

    public async Task<Plane> AddPlane(PlaneDto dto)
    {
        var plane = new Plane
        {
            Name = dto.Name,
            Code = dto.Code,
            CompanyId = dto.CompanyId
        };
        _context.Planes.Add(plane);
        await _context.SaveChangesAsync();
        return await GetPlane(plane.PlaneId);
    }

    public async Task<Plane> UpdatePlane(PlaneDto dto)
    {
        var plane = await _context.Planes.FindAsync(dto.PlaneId);
        if (plane is null) return null;

        plane.Code = dto.Code;
        plane.Name = dto.Name;
        plane.CompanyId = dto.CompanyId;

        _context.Planes.Update(plane);
            await _context.SaveChangesAsync();
            
        return await GetPlane(plane.PlaneId);
    }

    public async Task DeletePlane(int id)
    {
        var plane = await _context.Planes.Where(e=>e.PlaneId == id)
            .Include(e=>e.Flights).FirstOrDefaultAsync();
        if(plane is null) return;
        foreach (var flight in plane.Flights)
        {
            _context.Flights.Remove(flight);
        }
        _context.Planes.Remove(plane);
        await _context.SaveChangesAsync();
    }
}