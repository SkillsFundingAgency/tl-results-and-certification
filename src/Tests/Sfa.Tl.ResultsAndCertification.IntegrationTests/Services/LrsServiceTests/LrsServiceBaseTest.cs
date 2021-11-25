using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.LrsServiceTests
{
    public abstract class LrsServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected IMapper Mapper;

        protected ILogger<RegistrationRepository> RegistrationRepositoryLogger;
        protected IRegistrationRepository RegistrationRepository;
        protected ILogger<ILrsService> LrsServiceLogger;

        protected ILogger<GenericRepository<Qualification>> QualificationRepositoryLogger;
        protected IRepository<Qualification> QualificationRepository;
        protected LrsService LrsService;

        protected IList<TlLookup> TlLookup;
        protected IList<Qualification> Qualifications;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(LrsServiceMapper).Assembly));
            Mapper = new Mapper(mapperConfig);
        }

        public void SeedData()
        {
            Qualifications = SeedQualificationsData();
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
        }

        public TqRegistrationProfile SeedRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            DbContext.SaveChanges();
            return tqRegistrationProfile;
        }

        public IList<TqRegistrationProfile> SeedRegistrationProfilesData(bool withLrsData = true)
        {
            var profiles = withLrsData ? new TqRegistrationProfileBuilder().BuildLrsVerificationLearningEventsList() : new TqRegistrationProfileBuilder().BuildListWithoutLrsData();

            foreach (var profile in profiles)
            {
                RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            }
            DbContext.SaveChanges();
            return profiles;
        }
        
        public IList<Qualification> SeedQualificationsData()
        {
            var qualificationsList = new QualificationBuilder().BuildList();
            var qualifications = QualificationDataProvider.CreateQualificationList(DbContext, qualificationsList);

            foreach(var qual in qualifications)
            {
                qual.QualificationType.QualificationGrades = new QualificationGradeBuilder().BuildList(qual.QualificationType);
            }

            DbContext.SaveChanges();
            return qualifications;
        }

        public LrsLearnerRecordDetails BuildLearnerRecordDetails(TqRegistrationProfile profile, bool seedLearningEvents = true, bool isEnglishAchieved = true, bool isMathsAchieved = true, bool seedGender = true)
        {
            var learnerRecord = new LrsLearnerRecordDetails
            {
                ProfileId = profile.Id,
                Uln = profile.UniqueLearnerNumber,
                Firstname = profile.Firstname,
                Lastname = profile.Lastname,
                DateofBirth = profile.DateofBirth,
                Gender = seedGender ? LrsGender.Male.ToString() : null,
                IsLearnerVerified = true,
                PerformedBy = profile.CreatedBy
            };

            if(seedLearningEvents)
            {              
                var engQual = Qualifications.FirstOrDefault(e => e.TlLookup.Code == "Eng");
                var mathQual = Qualifications.FirstOrDefault(e => e.TlLookup.Code == "Math");

                var engQualifcationGrades = engQual.QualificationType.QualificationGrades;
                var mathsQualifcationGrades = mathQual.QualificationType.QualificationGrades;

                learnerRecord.LearningEventDetails.Add(new LrsLearningEventDetails
                {
                    QualificationGradeId = engQualifcationGrades.FirstOrDefault(g => g.IsAllowable == isEnglishAchieved).Id,
                    Grade = engQualifcationGrades.FirstOrDefault(g => g.IsAllowable == isEnglishAchieved).Grade,
                    QualificationId = engQual.Id,
                    QualificationCode = engQual.Code,
                    IsQualificationAllowed = engQual.IsActive,
                    IsEnglishSubject = engQual.TlLookup?.Code.Equals("Eng", StringComparison.InvariantCultureIgnoreCase) ?? false
                });

                learnerRecord.LearningEventDetails.Add(new LrsLearningEventDetails
                {
                    QualificationGradeId = mathsQualifcationGrades.FirstOrDefault(g => g.IsAllowable == isMathsAchieved).Id,
                    Grade = mathsQualifcationGrades.FirstOrDefault(g => g.IsAllowable == isMathsAchieved).Grade,
                    QualificationId = mathQual.Id,
                    QualificationCode = mathQual.Code,
                    IsQualificationAllowed = mathQual.IsActive,
                    IsMathsSubject = mathQual.TlLookup?.Code.Equals("Math", StringComparison.InvariantCultureIgnoreCase) ?? false
                });                   
            }
            return learnerRecord;
        }
    }
}