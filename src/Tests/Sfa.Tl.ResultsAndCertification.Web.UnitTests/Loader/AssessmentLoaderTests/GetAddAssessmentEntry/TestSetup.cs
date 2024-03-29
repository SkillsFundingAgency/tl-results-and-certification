﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAddAssessmentEntry
{
    public abstract class TestSetup : BaseTest<AssessmentLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly int ProfileId = 1;
        protected readonly string ComponentLarIds;
        protected readonly ComponentType componentType = ComponentType.Core;
        protected readonly long Uln = 1234567890;
        protected readonly int ProviderId = 1;
        protected string ComponentIds;

        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;
        public IBlobStorageService BlobStorageService { get; private set; }

        protected AssessmentLoader Loader;
        protected AddAssessmentEntryViewModel ActualResult;

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
            ActualResult = await Loader.GetAddAssessmentEntryAsync<AddAssessmentEntryViewModel>(AoUkprn, ProfileId, componentType, ComponentLarIds);
        }
    }
}
