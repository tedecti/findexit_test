using Microsoft.EntityFrameworkCore;

namespace findexit_test.Data ;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Npgsql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}
