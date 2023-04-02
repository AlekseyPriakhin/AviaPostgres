using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PostgresPerfomanceTest.Data.MongoContext;
using PostgresPerfomanceTest.Model.MongoModels;
using System.Diagnostics;

namespace PostgresPerfomanceTest.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        
        var context = serviceProvider.GetRequiredService<AviaDbContext>();
        if (!context.Planes.Any())
        {
            var sw = new Stopwatch();
                    sw.Start();
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
                    }[i%3],
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
            sw.Stop();
            Console.WriteLine($"-----------{sw.Elapsed}-------------");
        }
    }

    public static async Task InitializeMongo(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<IMongoContext>();
        var options = serviceProvider.GetRequiredService<IOptions<MongoSettings>>();

        var companyCollection = context.GetCollection<MongoCompany>(options.Value.Companies);
        var planeCollection = context.GetCollection<MongoPlane>(options.Value.Planes);
        var flightCollection = context.GetCollection<MongoFlight>(options.Value.Flights);
        var profileCollection = context.GetCollection<BsonDocument>(options.Value.Profile);

        string[] companyNames = new string[] {
            "Aeroflot",
            "Aurora",
            "NordStar",
            "Pobeda",
            "Red Wings Airlines",
            "S7 Airlines",
            "UTair",
            "Azur Ait" };

        string[] companyCodes = new string[] {
            "SU", "HZ", "Y7", "DP", "WZ", "S7", "4R", "U6", "NN", "UT" };

        string[] companyCountries = new string[] {
            "Brazil",
            "Russia",
            "India",
            "China",
            "USA",
            "Germany",
            "Italy" };

        string[] planeNames = new string[] {
            "Boeing 777X",
            "Cessna 408",
            "Airbus A319neo",
            "ATR EVO",
            "Comac C919",
            "CRAIC CR929" };

        string[] planeCodes = new string[] {
            "777x",
            "408",
            "a319neo",
            "evo",
            "c919",
            "cr929" };

        string[] flightFrom = new string[] {
            "Moscow",
            "Saint-Petersburg",
            "Arkhangelsk",
            "Astrakhan",
            "Barnaul",
            "Vladivostok",
            "Volgograd" };

        string[] flightTo = new string[] {
            "Volgograd",
            "Astrakhan",
            "Vladivostok",
            "Saint-Petersburg",
            "Barnaul",
            "Arkhangelsk",
            "Moscow" };

        var companies = new List<MongoCompany>();
        var planes = new List<MongoPlane>();
        var flights = new List<MongoFlight>();

        var comment = DateTime.Now.GetHashCode().ToString();
        var insertOptions = new InsertManyOptions();
        insertOptions.Comment = comment;

        if (await companyCollection.CountDocumentsAsync(_ => true) == 0)
        {
            var rand = new Random();

            for (int i = 0; i < 100000; i++)
            {
                companies.Add(new MongoCompany(){ Id = i, Name = companyNames[rand.Next(0, 7)], Code = companyCodes[rand.Next(0, 9)], Country = companyCountries[rand.Next(0, 6)], YearOfFoundation = rand.Next(1950, 2020) });
                planes.Add(new MongoPlane(){ Id = i, Name = planeNames[rand.Next(0, 5)], Code = planeCodes[rand.Next(0, 5)], CompanyId = rand.Next(0, 99999) });
                flights.Add(new MongoFlight(){ Id = i, From = flightFrom[rand.Next(0, 6)], To = flightTo[rand.Next(0, 6)], PlaneId = rand.Next(0, 99999) });
            }

            var filter = new BsonDocument { { "command.comment", comment } };

            //Companies
            await companyCollection.InsertManyAsync(companies, insertOptions);

            var entries = await profileCollection.Find(filter).ToListAsync();
            Console.WriteLine($"Time: {entries.Last()["millis"].ToString()} ms");

            //Planes
            comment = DateTime.Now.GetHashCode().ToString();
            insertOptions.Comment = comment;
            await planeCollection.InsertManyAsync(planes, insertOptions);

            filter = new BsonDocument { { "command.comment", comment } };
            entries = await profileCollection.Find(filter).ToListAsync();
            Console.WriteLine($"Time: {entries.Last()["millis"].ToString()} ms");

            //Flights
            comment = DateTime.Now.GetHashCode().ToString();
            insertOptions.Comment = comment;
            await flightCollection.InsertManyAsync(flights, insertOptions);

            filter = new BsonDocument { { "command.comment", comment } };
            entries = await profileCollection.Find(filter).ToListAsync();
            Console.WriteLine($"Time: {entries.Last()["millis"].ToString()} ms");
        }
    }
}