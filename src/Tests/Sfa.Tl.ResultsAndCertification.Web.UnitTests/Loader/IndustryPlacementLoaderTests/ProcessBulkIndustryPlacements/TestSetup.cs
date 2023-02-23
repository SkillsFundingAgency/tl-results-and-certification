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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessBulkIndustryPlacements
{
    public abstract class TestSetup : BaseTest<IndustryPlacementLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<IndustryPlacementLoader> Logger;
        protected IIndustryPlacementLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected readonly long Ukprn = 12345678;
        protected UploadIndustryPlacementsResponseViewModel ActualResult;
        protected BulkIndustryPlacementResponse BulkIndustryPlacementResponse;
        protected BulkProcessRequest BulkIndustryPlacementRequest;
        protected UploadIndustryPlacementsRequestViewModel UploadIndustryPlacementsRequestViewModel;
        protected UploadIndustryPlacementsResponseViewModel UploadIndustryPlacementsResponseViewModel;
        protected List<ProviderTlevel> ProviderTlevelDetails;
        protected IFormFile FormFile;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            Logger = Substitute.For<ILogger<IndustryPlacementLoader>>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            FormFile = Substitute.For<IFormFile>();
            BlobUniqueReference = Guid.NewGuid();
            BulkIndustryPlacementRequest = new BulkProcessRequest { AoUkprn = Ukprn };
        }

        public override void Given()
        {
            BulkIndustryPlacementResponse = new BulkIndustryPlacementResponse
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            UploadIndustryPlacementsRequestViewModel = new UploadIndustryPlacementsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadIndustryPlacementsResponseViewModel = new UploadIndustryPlacementsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            Mapper.Map<BulkProcessRequest>(UploadIndustryPlacementsRequestViewModel).Returns(BulkIndustryPlacementRequest);
            Mapper.Map<UploadIndustryPlacementsResponseViewModel>(BulkIndustryPlacementResponse).Returns(UploadIndustryPlacementsResponseViewModel);
            InternalApiClient.ProcessBulkIndustryPlacementsAsync(BulkIndustryPlacementRequest).Returns(BulkIndustryPlacementResponse);
            Loader = new IndustryPlacementLoader(InternalApiClient, Mapper, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessBulkIndustryPlacementsAsync(UploadIndustryPlacementsRequestViewModel);
        }
    }
}
