using Microsoft.EntityFrameworkCore;
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
        public virtual DbSet<TlAwardingOrganisation> TlAwardingOrganisation { get; set; }
        public virtual DbSet<TlMandatoryAdditionalRequirement> TlMandatoryAdditionalRequirement { get; set; }
        public virtual DbSet<TlPathway> TlPathway { get; set; }
        public virtual DbSet<TlPathwaySpecialismCombination> TlPathwaySpecialismCombination { get; set; }
        public virtual DbSet<TlPathwaySpecialismMar> TlPathwaySpecialismMar { get; set; }
        public virtual DbSet<TlProvider> TlProvider { get; set; }
        public virtual DbSet<TlRoute> TlRoute { get; set; }
        public virtual DbSet<TlSpecialism> TlSpecialism { get; set; }
        public virtual DbSet<TqAwardingOrganisation> TqAwardingOrganisation { get; set; }
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
