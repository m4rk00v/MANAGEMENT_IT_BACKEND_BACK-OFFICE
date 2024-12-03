using DbPersistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbPersistence;

public static class PersistenceService
{
    public static void AddPersistence(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString,
            p => p.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        
    }
    
}