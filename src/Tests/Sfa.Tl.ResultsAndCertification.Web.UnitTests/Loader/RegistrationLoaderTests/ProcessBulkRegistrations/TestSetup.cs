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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrations
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected IRegistrationLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected readonly long Ukprn = 12345678;
        protected UploadRegistrationsResponseViewModel ActualResult;
        protected BulkRegistrationResponse BulkRegistrationResponse;
        protected BulkRegistrationRequest BulkRegistrationRequest;
        protected UploadRegistrationsRequestViewModel UploadRegistrationsRequestViewModel;
        protected UploadRegistrationsResponseViewModel UploadRegistrationsResponseViewModel;
        protected List<ProviderTlevel> ProviderTlevelDetails;
        protected IFormFile FormFile;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";
        protected Guid BlobUniqueReference;
        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            FormFile = Substitute.For<IFormFile>();
            BlobUniqueReference = Guid.NewGuid();
            BulkRegistrationRequest = new BulkRegistrationRequest { AoUkprn = Ukprn };
        }

        public override void Given()
        {          
            BulkRegistrationResponse = new BulkRegistrationResponse
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            UploadRegistrationsRequestViewModel = new UploadRegistrationsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadRegistrationsResponseViewModel = new UploadRegistrationsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            Mapper.Map<BulkRegistrationRequest>(UploadRegistrationsRequestViewModel).Returns(BulkRegistrationRequest);
            Mapper.Map<UploadRegistrationsResponseViewModel>(BulkRegistrationResponse).Returns(UploadRegistrationsResponseViewModel);
            InternalApiClient.ProcessBulkRegistrationsAsync(BulkRegistrationRequest).Returns(BulkRegistrationResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel);
        }
    }
}
