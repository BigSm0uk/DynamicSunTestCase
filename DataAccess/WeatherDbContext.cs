using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public sealed class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<WeatherEntity> WeatherEntities  { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Database.Migrate();
    }
}