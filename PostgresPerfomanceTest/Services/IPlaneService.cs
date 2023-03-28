using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.DTO;

namespace PostgresPerfomanceTest.Services;

public interface IPlaneService
{
    public Task<IEnumerable<Plane>> GetPlanes();
    public Task<Plane> GetPlane(int id);
    public Task<Plane> AddPlane(PlaneDto dto);
    public Task<Plane> UpdatePlane(PlaneDto dto);
    public Task DeletePlane(int id);
}