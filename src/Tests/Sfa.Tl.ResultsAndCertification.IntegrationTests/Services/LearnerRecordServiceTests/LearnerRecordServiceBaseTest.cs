using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.LearnerRecordServiceTests
{
    public abstract class LearnerRecordServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected IMapper Mapper;

        protected ILogger<RegistrationRepository> RegistrationRepositoryLogger;
        protected IRegistrationRepository RegistrationRepository;
        protected ILogger<ILearnerRecordService> LearnerRecordServiceLogger;

        protected ILogger<GenericRepository<Qualification>> QualificationRepositoryLogger;
        protected IRepository<Qualification> QualificationRepository;
        protected LearnerRecordService LrsService;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(LearningRecordServiceMapper).Assembly));
            Mapper = new Mapper(mapperConfig);
        }

        public TqRegistrationProfile SeedRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            DbContext.SaveChanges();
            return tqRegistrationProfile;
        }


        public IList<TqRegistrationProfile> SeedRegistrationProfilesData()
        {
            var profiles = new TqRegistrationProfileBuilder().BuildLrsVerificationLearningEventsList();

            foreach (var profile in profiles)
            {
                RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            }
            DbContext.SaveChanges();
            return profiles;
        }       
    }
}