using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DownloadOverallResultsLoaderTests.DownloadOverallResultsData
{
    public abstract class TestSetup : BaseTest<DownloadOverallResultsLoader>
    {
        protected readonly long providerUkprn = 12345678;
        protected string RequestedBy = "System";
        protected IMapper Mapper;
        protected ILogger<DownloadOverallResultsLoader> Logger;
        protected IBlobStorageService BlobStorageService;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected DownloadOverallResultsLoader Loader;
        protected Stream ActualResult;
        protected Stream ExpectedApiResult;
        
        public override void Setup()
        {
            Logger = Substitute.For<ILogger<DownloadOverallResultsLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AssessmentMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new DownloadOverallResultsLoader(InternalApiClient, BlobStorageService, Logger);
        }

        public async override Task When()
        {
            ActualResult = await Loader.DownloadOverallResultsDataAsync(providerUkprn, RequestedBy);
        }
    }
}
