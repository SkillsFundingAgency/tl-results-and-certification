using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public abstract class TrainingProviderServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected TrainingProviderService TrainingProviderService;
        protected ILogger<GenericRepository<TqRegistrationProfile>> RegistrationProfileRepositoryLogger;
        protected IRepository<TqRegistrationProfile> RegistrationProfileRepository;
        protected ILogger<GenericRepository<TqRegistrationPathway>> RegistrationPathwayRepositoryLogger;
        protected IRepository<TqRegistrationPathway> RegistrationPathwayRepository;
        protected ILogger<GenericRepository<IndustryPlacement>> IndustryPlacementRepositoryLogger;
        protected IRepository<IndustryPlacement> IndustryPlacementRepository;
        protected IMapper TrainingProviderMapper;
        protected ILogger<TrainingProviderService> TrainingProviderServiceLogger;
        protected ITrainingProviderRepository TrainingProviderRepository;
        protected ILogger<TrainingProviderRepository> TrainingProviderRepositoryLogger;
        protected NotificationService NotificationService;
        protected IAsyncNotificationClient NotificationsClient;
        protected ILogger<NotificationService> NotificationLogger;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected ILogger<GenericRepository<NotificationTemplate>> NotificationTemplateRepositoryLogger;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;

        // Data Seed variables
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IEnumerable<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<Qualification> Qualifications;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TrainingProviderMapper).Assembly));
            TrainingProviderMapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProviders.First());
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            NotificationDataProvider.CreateNotificationTemplate(DbContext, NotificationTemplateName.EnglishAndMathsLrsDataQueried);

            DbContext.SaveChangesAsync();

            Qualifications = SeedQualificationsData();
        }

        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildListWithoutLrsData().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (status == RegistrationPathwayStatus.Withdrawn || status == RegistrationPathwayStatus.Transferred)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }        

        public IList<Qualification> SeedQualificationsData()
        {
            var qualificationsList = new QualificationBuilder().BuildList();
            var qualifications = QualificationDataProvider.CreateQualificationList(DbContext, qualificationsList);

            foreach (var qual in qualifications)
            {
                qual.QualificationType.QualificationGrades = new QualificationGradeBuilder().BuildList(qual.QualificationType);
            }

            DbContext.SaveChanges();
            return qualifications;
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
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
            }
        }

        public void BuildLearnerSubjectCriteria(TqRegistrationProfile profile, SubjectStatus? mathsStatus = null, SubjectStatus? englishStatus = null)
        {
            if (profile == null) return;

            profile.MathsStatus = mathsStatus; 
            profile.EnglishStatus = englishStatus;
        }
    }

    public enum Provider
    {
        BarsleyCollege = 10000536,
        WalsallCollege = 10007315
    }
}
