namespace PostgresPerfomanceTest.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AviaDbContext>();
        if (!context.Planes.Any())
        {
            for (int i = 0; i < 100000; i++)
            {
                var company = new Company
                {
                    Country = new List<string>
                    {
                        "RU",
                        "USA",
                        "FRA",
                        "ENG",
                        "CHI"
                    }[i%5],
                    Name = new List<string>
                    {
                        "Aurora",
                        "Condor",
                        "Delta",
                        "E",
                        "F",
                        "G",
                        "H",
                        "M",
                        "K",
                        "L"
                    }[i%10],
                    Code = new List<string>
                    {
                        "BT",
                        "XY",
                        "HZ",
                        "FH"
                        
                    }[i%4],
                    YearOfFoundation = 1980 + i % 5
                };
                context.Companies.Add(company);
               
            }
            await context.SaveChangesAsync();
            var random = new Random();
            for (int i = 0; i < 100000; i++)
            {
                var plane = new Plane
                {
                    Name = new List<string>
                    {
                        "Boeing 737-800",
                        "Airbus A380",
                        "Tu-144"
                        
                    }[i % 3],
                    Code = new List<string>
                    {
                        "737","380","144"
                    }[i % 3],
                    CompanyId = random.Next(1,100000)
                };

                context.Planes.Add(plane);
                

            }
            await context.SaveChangesAsync();

            for (int i = 0; i < 100000; i++)
            {
                var flight = new Flight
                {
                    From = new List<string>
                    {
                        "Moscow",
                        "Tokio",
                        "Paris"
                    }[i % 3],
                    To = new List<string>
                    {
                        "Paris",
                        "Moscow",
                        "New-York"
                    }[i % 3],
                    PlaneId = random.Next(1, 100000)
                };

                context.Flights.Add(flight);
                
            }
            await context.SaveChangesAsync();
        }
    }
}