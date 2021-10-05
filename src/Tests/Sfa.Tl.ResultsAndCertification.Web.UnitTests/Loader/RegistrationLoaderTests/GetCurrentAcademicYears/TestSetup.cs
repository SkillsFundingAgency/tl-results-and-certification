using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetCurrentAcademicYears
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {        
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        public IBlobStorageService BlobStorageService { get; private set; }

        protected RegistrationLoader Loader;
        protected IEnumerable<AcademicYear> expectedApiResult;
        protected IEnumerable<AcademicYear> ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AssessmentMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetCurrentAcademicYearsAsync();
        }
    }
}
