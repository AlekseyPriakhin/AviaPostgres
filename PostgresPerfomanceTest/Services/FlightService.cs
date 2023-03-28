using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Services;

public class FlightService : IFlightService
{
    private readonly AviaDbContext _context;

    public FlightService(AviaDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Flight>> GetFlights()
    {
        var items = await _context.Flights.ToListAsync();
        return items.Take(100);
    }

    public async Task<Flight> GetFlight(int id)
    {
        var company =  await _context.Flights.FindAsync(id);

        return company;
    }

    public async Task<Flight> AddFlight(FlightDto dto)
    {
        var flight = new Flight
        {
            From = dto.From,
            To = dto.To,
            PlaneId = dto.PlaneId
        };

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        return await GetFlight(flight.FlightId);
    }

    public async Task<Flight> UpdateFlight(FlightDto dto)
    {
        var flight = await _context.Flights.FindAsync(dto.FlightId);
        if (flight is null) return null;
        flight.From = dto.From;
        flight.To = dto.To;

        _context.Flights.Update(flight);
        await _context.SaveChangesAsync();
        return await GetFlight(dto.FlightId);
    }

    public async Task DeleteFlight(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        
        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
    }
}