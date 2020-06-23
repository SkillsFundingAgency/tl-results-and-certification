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
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public abstract class When_ProcessBulkRegistrationsAsync_Is_Called : BaseTest<RegistrationLoader>
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
        private Guid _blobUniqueReference;
        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            FormFile = Substitute.For<IFormFile>();
        }

        public override void Given()
        {
            _blobUniqueReference = Guid.NewGuid();
            BulkRegistrationRequest = new BulkRegistrationRequest { AoUkprn = Ukprn };

            BulkRegistrationResponse = new BulkRegistrationResponse
            {
                IsSuccess = false,
                BlobUniqueReference = _blobUniqueReference,
                ErrorFileSize = 1.5
            };

            UploadRegistrationsRequestViewModel = new UploadRegistrationsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadRegistrationsResponseViewModel = new UploadRegistrationsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = _blobUniqueReference,
                ErrorFileSize = 1.5
            };

            Mapper.Map<BulkRegistrationRequest>(UploadRegistrationsRequestViewModel).Returns(BulkRegistrationRequest);
            Mapper.Map<UploadRegistrationsResponseViewModel>(BulkRegistrationResponse).Returns(UploadRegistrationsResponseViewModel);
            InternalApiClient.ProcessBulkRegistrationsAsync(BulkRegistrationRequest).Returns(BulkRegistrationResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public override void When()
        {
            ActualResult = Loader.ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel).Result;
        }
    }
}
