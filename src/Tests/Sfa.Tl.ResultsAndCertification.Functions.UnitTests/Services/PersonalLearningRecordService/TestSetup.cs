using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.PersonalLearningRecordService
{
    public abstract class TestSetup : BaseTest<Functions.Services.PersonalLearningRecordService>
    {
        protected IMapper Mapper;
        protected ILogger<IPersonalLearningRecordService> Logger;
        protected ILearnerRecordService LearnerRecordService;
        protected IPersonalLearningRecordApiClient PersonalLearningRecordApiClient;
        protected Functions.Services.PersonalLearningRecordService Service;
        protected LearnerVerificationAndLearningEventsResponse ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<IPersonalLearningRecordService>>();
            LearnerRecordService = Substitute.For<ILearnerRecordService>();
            PersonalLearningRecordApiClient = Substitute.For<IPersonalLearningRecordApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Service = new Functions.Services.PersonalLearningRecordService(Mapper, Logger, PersonalLearningRecordApiClient, LearnerRecordService);
        }

        public async override Task When()
        {
            ActualResult = await Service.ProcessLearnerVerificationAndLearningEventsAsync();
        }
    }
}
