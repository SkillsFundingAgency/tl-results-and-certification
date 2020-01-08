using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data
{
    public class ResultsAndCertificationDbContext : DbContext
    {
        public ResultsAndCertificationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This will singularize all table names
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.Relational().TableName = entityType.DisplayName();
            }
        }
    }
}
