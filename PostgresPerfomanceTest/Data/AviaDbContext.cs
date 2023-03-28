using Microsoft.EntityFrameworkCore;

namespace PostgresPerfomanceTest.Data;

public class AviaDbContext : DbContext
{

    public DbSet<Company> Companies { get; set; }
    
    public DbSet<Flight> Flights { get; set; }
    
    public DbSet<Plane> Planes { get; set; }
    
    public AviaDbContext(DbContextOptions<AviaDbContext> options) : base(options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>(entity =>
        {
            //entity.HasIndex(e => e.Name);
        });
        
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasOne(e => e.Plane)
                .WithMany(e => e.Flights)
                .HasForeignKey(e=>e.PlaneId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasIndex(e => e.Name);
            entity.HasOne(e => e.Company)
                .WithMany(e => e.Planes)
                .HasForeignKey(e=>e.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}