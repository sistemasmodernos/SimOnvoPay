using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimOnvoPay.Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // MySQL 8.0 para design-time (migraciones). En runtime se usa ServerVersion.AutoDetect.
        optionsBuilder.UseMySql(
            "Server=localhost;Database=SimOnvoPay;User=root;Password=;Port=3306;",
            new MySqlServerVersion(new Version(8, 0, 0))
        );
        return new AppDbContext(optionsBuilder.Options);
    }
}
