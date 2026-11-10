using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Data.Builder
{
    public class DbContextBuilder : IDbContextBuilder
    {
        private readonly ResultsAndCertificationConfiguration _config;

        public DbContextBuilder(ResultsAndCertificationConfiguration config)
        {
            _config = config;
        }

        public ResultsAndCertificationDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>();
            optionsBuilder.UseSqlServer(_config.SqlConnectionString, (option) => {
                option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            return new ResultsAndCertificationDbContext(optionsBuilder.Options);
        }
    }
}
