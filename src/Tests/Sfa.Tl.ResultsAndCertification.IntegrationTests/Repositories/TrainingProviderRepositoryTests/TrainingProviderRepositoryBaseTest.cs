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
        protected IEnumerable<TlProvider> TlProviders;
        protected TlProvider TlProvider;
        protected TqProvider TqProvider;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TqProvider> TqProviders;
        protected IList<Qualification> Qualifications;

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
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProviders.First());
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

        public void BuildLearnerRecordCriteria(TqRegistrationProfile profile, bool? isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool? isEngishAndMathsAchieved, bool seedIndustryPlacement = false, bool? isSendLearner = null)
        {
            if (profile == null) return;

            profile.IsRcFeed = isRcFeed;
            profile.IsEnglishAndMathsAchieved = isEngishAndMathsAchieved;
            profile.IsSendLearner = isSendLearner;

            if (seedQualificationAchieved)
            {
                var engQual = Qualifications.FirstOrDefault(e => e.TlLookup.Code == "Eng" && e.IsSendQualification == isSendQualification);
                var mathQual = Qualifications.FirstOrDefault(e => e.TlLookup.Code == "Math");

                var engQualifcationGrade = engQual.QualificationType.QualificationGrades.FirstOrDefault(x => x.IsAllowable == isEngishAndMathsAchieved);
                var mathsQualifcationGrade = mathQual.QualificationType.QualificationGrades.FirstOrDefault(x => x.IsAllowable == isEngishAndMathsAchieved);

                profile.QualificationAchieved.Add(new QualificationAchieved
                {
                    TqRegistrationProfileId = profile.Id,
                    QualificationId = engQual.Id,
                    QualificationGradeId = engQualifcationGrade.Id,
                    IsAchieved = engQualifcationGrade.IsAllowable
                });

                profile.QualificationAchieved.Add(new QualificationAchieved
                {
                    TqRegistrationProfileId = profile.Id,
                    QualificationId = mathQual.Id,
                    QualificationGradeId = mathsQualifcationGrade.Id,
                    IsAchieved = mathsQualifcationGrade.IsAllowable
                });
            }

            if (seedIndustryPlacement)
            {
                var pathway = profile.TqRegistrationPathways.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                IndustryPlacementProvider.CreateQualificationAchieved(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
            }
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

        public IList<Qualification> SeedQualificationData()
        {
            var qualificationsList = new QualificationBuilder().BuildList();
            var qualifications = QualificationDataProvider.CreateQualificationList(DbContext, qualificationsList);

            foreach (var qual in qualifications)
            {
                qual.QualificationType.QualificationGrades = new QualificationGradeBuilder().BuildList(qual.QualificationType);
            }

            return qualifications;
        }

        public class SeedUlnQualifications
        {
            public long Uln { get; set; }
            public bool HasSendQualification { get; set; }
            public bool HasSendGrade { get; set; }
        }


        public void TransferRegistration(TqRegistrationProfile profile, Provider transferTo)
        {
            var toProvider = DbContext.TlProvider.FirstOrDefault(x => x.UkPrn == (long)transferTo);
            var transferToTqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, toProvider, true);

            foreach (var pathway in profile.TqRegistrationPathways)
            {
                pathway.Status = RegistrationPathwayStatus.Transferred;
                pathway.EndDate = DateTime.UtcNow;

                foreach (var specialism in pathway.TqRegistrationSpecialisms)
                {
                    specialism.IsOptedin = true;
                    specialism.EndDate = DateTime.UtcNow;
                }
            }

            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, profile, transferToTqProvider);
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);
            DbContext.SaveChanges();
        }
    }

    public enum Provider
    {
        BarsleyCollege = 10000536,
        WalsallCollege = 10007315
    }
}
