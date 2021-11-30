using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetActiveAssessmentEntryDetails
{
    public abstract class TestSetup : BaseTest<AssessmentLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly int ProfileId = 1;
        protected readonly int AssessmentId = 1;
        protected readonly ComponentType componentType = ComponentType.Core;

        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;        
        protected IBlobStorageService BlobStorageService;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected AssessmentLoader Loader;
        protected AssessmentEntryDetails ExpectedApiResult;
        protected AssessmentEntryDetailsViewModel ActualResult;

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
            ActualResult = await Loader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, AssessmentId, componentType);
        }
    }
}
