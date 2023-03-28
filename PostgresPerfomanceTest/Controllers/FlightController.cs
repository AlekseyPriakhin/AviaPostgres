using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;
using PostgresPerfomanceTest.Services;

namespace PostgresPerfomanceTest.Controllers;

[ApiController,Route("/flight")]
public class FlightController : ControllerBase
{
    private readonly IFlightService _service;
    public FlightController(IFlightService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFlights()
    {
        var items = await _service.GetFlights();
        return Ok(items);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlight(int id)
    {
        var flight = await _service.GetFlight(id);
        if (flight is null) return NotFound();
        return Ok(flight);
    }

    [HttpPost]
    public async Task<IActionResult> Post(FlightDto dto)
    {
        var newFlight = await _service.AddFlight(dto);
        return Ok(newFlight);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FlightDto dto)
    {
        dto.FlightId = id;

        var flight =  await _service.UpdateFlight(dto);
        
        return Ok(flight);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteFlight(id);
        return Ok();
    }
    
}