using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Data.Builder
{
    public class DbContextBuilder : IDbContextBuilder
    {
        private readonly IConfiguration _config;
        private const string ConnectionString = "DefaultConnection";

        public DbContextBuilder(IConfiguration config)
        {
            _config = config;
        }

        public ResultsAndCertificationDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString(ConnectionString));

            return new ResultsAndCertificationDbContext(optionsBuilder.Options);
        }
    }
}
