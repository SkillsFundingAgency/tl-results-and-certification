using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetManageCoreResult
{
    public abstract class TestSetup : BaseTest<ResultLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly int ProfileId = 1;
        protected readonly int AssessmentId = 1;

        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<ResultLoader> Logger;
        public IBlobStorageService BlobStorageService { get; private set; }

        protected bool IsChangeMode = false;
        protected ResultLoader Loader;
        protected ManageCoreResultViewModel ActualResult;

        protected LearnerRecord expectedApiResultDetails;
        protected IList<LookupData> expectedApiLookupData;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ResultLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ResultMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new ResultLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetManageCoreResultAsync(AoUkprn, ProfileId, AssessmentId, IsChangeMode);
        }
    }
}
