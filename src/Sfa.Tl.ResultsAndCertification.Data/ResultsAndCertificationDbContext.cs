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

        public virtual DbSet<TlLookup> TlLookup { get; set; }
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
        public virtual DbSet<AssessmentSeries> AssessmentSeries { get; set; }
        public virtual DbSet<TqPathwayAssessment> TqPathwayAssessment { get; set; }
        public virtual DbSet<TqSpecialismAssessment> TqSpecialismAssessment { get; set; }        
        public virtual DbSet<TqPathwayResult> TqPathwayResult { get; set; }

        public virtual DbSet<QualificationType> QualificationType { get; set; }
        public virtual DbSet<QualificationGrade> QualificationGrade { get; set; }
        public virtual DbSet<Qualification> Qualification { get; set; }
        public virtual DbSet<QualificationAchieved> QualificationAchieved { get; set; }
        public virtual DbSet<FunctionLog> FunctionLog { get; set; }

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
