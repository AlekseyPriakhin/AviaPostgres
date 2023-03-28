using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Services;

public interface IFlightService
{
    public Task<IEnumerable<Flight>> GetFlights();
    public Task<Flight> GetFlight(int id);
    public Task<Flight> AddFlight(FlightDto dto);
    public Task<Flight> UpdateFlight(FlightDto dto);
    public Task DeleteFlight(int id);
}