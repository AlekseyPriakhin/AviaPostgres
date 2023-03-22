namespace PostgresPerfomanceTest.Data;

public class Company
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Country  { get; set; }
    public int YearOfFoundation { get; set; }
    public virtual ICollection<Plane> Planes { get; set; }

    public Company()
    {
        Planes = new HashSet<Plane>();
    }
}