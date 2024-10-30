using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Configuration
{
    public class TestDatabaseConfiguration
    {
        public static string IntTestSqlConnectionString { get; }

        static TestDatabaseConfiguration()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            IntTestSqlConnectionString = configuration[Constants.IntTestSqlConnectionString];
        }

        public static ResultsAndCertificationDbContext CreateRelationalDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
                .UseSqlServer(IntTestSqlConnectionString, builder =>
                    builder.EnableRetryOnFailure().UseNetTopologySuite()).Options;

            return new ResultsAndCertificationDbContext(dbContextOptions);
        }
    }
}