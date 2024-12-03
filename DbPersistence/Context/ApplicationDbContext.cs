using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbPersistence.Context;

public class ApplicationDbContext : DbContext
{
    
    public DbSet<User> Users { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ZipModel> ZipModels { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior",true);
        
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}