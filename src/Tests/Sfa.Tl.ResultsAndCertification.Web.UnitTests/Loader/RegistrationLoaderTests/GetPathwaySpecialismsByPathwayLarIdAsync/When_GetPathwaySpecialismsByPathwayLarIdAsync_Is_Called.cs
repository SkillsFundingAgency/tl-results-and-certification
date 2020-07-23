using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPathwaySpecialismsByPathwayLarIdAsync
{
    public abstract class When_GetPathwaySpecialismsByPathwayLarIdAsync_Is_Called : BaseTest<RegistrationLoader>
    {
        protected readonly long Ukprn = 12345678;
        protected readonly string PathwayLarId = "123";
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected RegistrationLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected PathwaySpecialisms ApiClientResponse;
        protected PathwaySpecialismsViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new PathwaySpecialisms
            {
                Id = 1,
                PathwayCode = "12345",
                PathwayName = "Test 1",
                Specialisms = new List<SpecialismDetails> { new SpecialismDetails { Id = 1, Code = "76543", Name = "Specialism 1" } }
            };

            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, PathwayLarId).Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TlevelLoader).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public override void When()
        {
            ActualResult = Loader.GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, PathwayLarId).Result;
        }
    }
}
