using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ConnectChain.Data.Context
{
    public class ConnectChainDbContextFactory : IDesignTimeDbContextFactory<ConnectChainDbContext>
    {
        public ConnectChainDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Database connection string is missing in appsettings.json!");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ConnectChainDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ConnectChainDbContext(optionsBuilder.Options);
        }
    }
}
