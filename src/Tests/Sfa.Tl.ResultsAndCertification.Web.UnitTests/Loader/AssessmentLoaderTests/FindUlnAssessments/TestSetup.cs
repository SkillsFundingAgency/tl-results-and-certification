using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.FindUlnAssessments
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected readonly long Ukprn = 34567891;
        protected readonly long Uln = 7890123456;
        protected readonly int ProviderId = 1;

        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;
        public IBlobStorageService BlobStorageService { get; private set; }

        protected AssessmentLoader Loader;
        protected FindUlnResponse expectedApiResult;
        protected UlnAssessmentsNotFoundViewModel ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<AssessmentLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AssessmentMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AssessmentLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.FindUlnAssessmentsAsync(Ukprn, Uln);
        }
    }
}
