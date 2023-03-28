using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;
using PostgresPerfomanceTest.Services;

namespace PostgresPerfomanceTest.Controllers;

[ApiController, Route("/plane")]
public class PlaneController : ControllerBase
{

    private readonly IPlaneService _service;
    
    public PlaneController(IPlaneService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetPlanes()
    {
        var items = await _service.GetPlanes();
        return Ok(items);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlane(int id)
    {
        var plane = await _service.GetPlane(id);
        if (plane is null) return NotFound();
        return Ok(plane);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PlaneDto dto)
    {
        var plane = await _service.AddPlane(dto);
        return Ok(plane);
        //return CreatedAtAction(nameof(GetPlane), new {id = plane.PlaneId}, plane);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PlaneDto dto)
    {
        dto.PlaneId = id;
        var plane = await _service.UpdatePlane(dto);
        return Ok(plane);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeletePlane(id);
        return Ok();
    }
    
}