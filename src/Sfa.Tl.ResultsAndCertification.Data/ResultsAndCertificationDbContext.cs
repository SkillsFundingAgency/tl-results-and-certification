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

        public virtual DbSet<BaseEntity> BaseEntity { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<TqProvider> TqProvider { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////This will singularize all table names
            //foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    entityType.Relational().TableName = entityType.DisplayName();
            //}
        }
    }
}
