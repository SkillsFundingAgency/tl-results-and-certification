using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.TrainingProviderRepositoryTests
{
    public abstract class TrainingProviderRepositoryBaseTest : BaseTest<QualificationAchieved>
    {
        // Seed objects.
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TlProvider TlProvider;
        protected TqProvider TqProvider;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TqProvider> TqProviders;

        // Dependencies.
        protected ILogger<TrainingProviderRepository> TraningProviderRepositoryLogger;
        protected ITrainingProviderRepository TrainingProviderRepository;

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();

            DbContext.SaveChangesAsync();
        }

        public TqRegistrationProfile SeedRegistrationDataByStatus(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            tqRegistrationPathway.Status = status;

            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }

        public void SeedProfileAndQualification(List<SeedUlnQualifications> seedUlnQualifications)
        {
            foreach (var ulnQual in seedUlnQualifications)
            {
                var profile = SeedRegistrationDataByStatus(ulnQual.Uln, RegistrationPathwayStatus.Active, TqProvider);
                DbContext.SaveChanges();

                var qualsAchieved = new QualificationAchievedBuilder().BuildList(profile);

                if (!ulnQual.HasSendQualification && !ulnQual.HasSendGrade)
                {
                    QualificationAchievedDataProvider.CreateQualificationAchieved(DbContext, qualsAchieved, true);
                    continue;
                }

                if (ulnQual.HasSendQualification)
                {
                    var qualification = DbContext.Qualification.FirstOrDefault(x => x.IsSendQualification);
                    qualsAchieved[1].Qualification = qualification;
                    qualsAchieved[1].QualificationId = qualification.Id;

                    QualificationAchievedDataProvider.CreateQualificationAchieved(DbContext, qualsAchieved, true);
                }

                if (ulnQual.HasSendGrade)
                {
                    var qualificationGrade = DbContext.QualificationGrade.FirstOrDefault(x => x.IsSendGrade);
                    qualsAchieved[0].QualificationGrade = qualificationGrade;
                    qualsAchieved[0].QualificationGradeId = qualificationGrade.Id;

                    QualificationAchievedDataProvider.CreateQualificationAchieved(DbContext, qualsAchieved, true);
                }
            }
        }

        public void SeedQualificationData()
        {
            var _qualTypes = new QualificationTypeBuilder().BuildList();
            QualificationTypeDataProvider.CreateTlLookupList(DbContext, _qualTypes, true);

            var _quals = new QualificationBuilder().BuildList();
            QualificationDataProvider.CreateQualificationList(DbContext, _quals, true);

            var _qualGrades = new QualificationGradeBuilder().BuildList();
            _qualGrades.ToList().ForEach(x => x.QualificationTypeId = 4);
            QualificationGradeDataProvider.CreateTlLookupList(DbContext, _qualGrades, true);
        }

        public class SeedUlnQualifications
        {
            public long Uln { get; set; }
            public bool HasSendQualification { get; set; }
            public bool HasSendGrade { get; set; }
        }
    }
}
