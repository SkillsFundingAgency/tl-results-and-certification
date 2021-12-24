using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentsDataFile
{
    public abstract class TestSetup : BaseTest<AssessmentLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected Guid BlobUniqueReference;
        protected ComponentType ComponentType;
        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;
        protected IBlobStorageService BlobStorageService;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected AssessmentLoader Loader;        
        protected Stream ActualResult;

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
            ActualResult = await Loader.GetAssessmentsDataFileAsync(AoUkprn, BlobUniqueReference, ComponentType);
        }
    }
}
