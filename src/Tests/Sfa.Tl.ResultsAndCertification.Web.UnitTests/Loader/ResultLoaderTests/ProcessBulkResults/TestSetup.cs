using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ProcessBulkResults
{
    public abstract class TestSetup : BaseTest<ResultLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<ResultLoader> Logger;
        protected IResultLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected readonly long Ukprn = 12345678;
        protected UploadResultsResponseViewModel ActualResult;
        protected BulkResultResponse BulkResultResponse;
        protected BulkProcessRequest BulkResultRequest;
        protected UploadResultsRequestViewModel UploadResultsRequestViewModel;
        protected UploadResultsResponseViewModel UploadResultsResponseViewModel;
        protected List<ProviderTlevel> ProviderTlevelDetails;
        protected IFormFile FormFile;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            Logger = Substitute.For<ILogger<ResultLoader>>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            FormFile = Substitute.For<IFormFile>();
            BlobUniqueReference = Guid.NewGuid();
            BulkResultRequest = new BulkProcessRequest { AoUkprn = Ukprn };
        }

        public override void Given()
        {
            BulkResultResponse = new BulkResultResponse
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            UploadResultsRequestViewModel = new UploadResultsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadResultsResponseViewModel = new UploadResultsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            Mapper.Map<BulkProcessRequest>(UploadResultsRequestViewModel).Returns(BulkResultRequest);
            Mapper.Map<UploadResultsResponseViewModel>(BulkResultResponse).Returns(UploadResultsResponseViewModel);
            InternalApiClient.ProcessBulkResultsAsync(BulkResultRequest).Returns(BulkResultResponse);
            Loader = new ResultLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessBulkResultsAsync(UploadResultsRequestViewModel);
        }
    }
}
