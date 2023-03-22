using System.Text.Json.Serialization;

namespace PostgresPerfomanceTest.Data;

public class Plane
{
    public int PlaneId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    
    public int CompanyId { get; set; }
    
    [JsonIgnore]
    public virtual Company Company { get; set; }
    public virtual ICollection<Flight> Flights { get; set; }

    public Plane()
    {
        Flights = new HashSet<Flight>();
    }
}