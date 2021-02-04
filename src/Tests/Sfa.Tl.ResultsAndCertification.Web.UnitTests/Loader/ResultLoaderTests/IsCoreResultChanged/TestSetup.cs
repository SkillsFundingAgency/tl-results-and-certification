using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.IsCoreResultChanged
{
    public abstract class TestSetup : BaseTest<ResultLoader>
    {
        // Dependenices
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<ResultLoader> Logger;
        public IBlobStorageService BlobStorageService { get; private set; }

        protected readonly long AoUkprn = 12345678; 
        protected ResultLoader Loader;
        protected ManageCoreResultViewModel ViewModel = new ManageCoreResultViewModel { ResultId = 1, SelectedGradeCode = "PCG1" };
        protected bool? ActualResult;

        protected ResultDetails expectedApiResultDetails;
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
            ActualResult = await Loader.IsCoreResultChanged(AoUkprn, ViewModel);
        }
    }
}
