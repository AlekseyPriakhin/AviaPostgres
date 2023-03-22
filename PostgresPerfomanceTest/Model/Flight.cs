using System.Text.Json.Serialization;

namespace PostgresPerfomanceTest.Data;

public class Flight
{
    public int FlightId { get; set; }
    public string To { get; set; }
    public string From { get; set; }


    public int PlaneId { get; set; }
    [JsonIgnore]
    public virtual Plane Plane { get; set; }
}