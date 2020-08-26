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
       
        public virtual DbSet<TlAwardingOrganisation> TlAwardingOrganisation { get; set; }
        public virtual DbSet<TlPathway> TlPathway { get; set; }
        public virtual DbSet<TlProvider> TlProvider { get; set; }
        public virtual DbSet<TlRoute> TlRoute { get; set; }
        public virtual DbSet<TlSpecialism> TlSpecialism { get; set; }
        public virtual DbSet<TqAwardingOrganisation> TqAwardingOrganisation { get; set; }
        public virtual DbSet<TqProvider> TqProvider { get; set; }
        public virtual DbSet<NotificationTemplate> NotificationTemplate { get; set; }
        public virtual DbSet<DocumentUploadHistory> DocumentUploadHistory { get; set; }
        public virtual DbSet<TqRegistrationProfile> TqRegistrationProfile { get; set; }
        public virtual DbSet<TqRegistrationPathway> TqRegistrationPathway { get; set; }
        public virtual DbSet<TqRegistrationSpecialism> TqRegistrationSpecialism { get; set; }

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
