using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Controllers;

[ApiController,Route("/flight")]
public class FlightController : ControllerBase
{
    private readonly AviaDbContext _context;

    public FlightController(AviaDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFlights()
    {
        var items = await _context.Flights.ToListAsync();
        return Ok(items.Take(100));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlight(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight is null) return NotFound();
        return Ok(flight);
    }

    [HttpPost]
    public async Task<IActionResult> Post(FlightDto dto)
    {
        var flight = new Flight
        {
            From = dto.From,
            To = dto.To,
            PlaneId = dto.PlaneId
        };

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        return Ok(await GetFlight(flight.FlightId));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FlightDto dto)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight is null) return NotFound();
        flight.From = dto.From;
        flight.To = dto.To;

        _context.Flights.Update(flight);
        await _context.SaveChangesAsync();
        return Ok(await GetFlight(id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight is null) return NotFound();

        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
        return Ok("Deleted");
    }
    
}