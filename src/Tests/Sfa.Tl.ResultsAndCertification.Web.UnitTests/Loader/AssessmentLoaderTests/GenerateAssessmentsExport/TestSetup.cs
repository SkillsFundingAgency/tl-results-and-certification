using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GenerateAssessmentsExport
{
    public abstract class TestSetup : BaseTest<AssessmentLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected string RequestedBy = "System";
        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;
        protected IBlobStorageService BlobStorageService;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected AssessmentLoader Loader;
        protected IList<DataExportResponse> ExpectedApiResult;
        protected IList<DataExportResponse> ActualResult;

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
            ActualResult = await Loader.GenerateAssessmentsExportAsync(AoUkprn, RequestedBy);
        }
    }
}
