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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.ProcessBulkAssessments
{
    public abstract class TestSetup : BaseTest<AssessmentLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;
        protected IAssessmentLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected readonly long Ukprn = 12345678;
        protected UploadAssessmentsResponseViewModel ActualResult;
        protected BulkAssessmentResponse BulkAssessmentResponse;
        protected BulkAssessmentRequest BulkAssessmentRequest;
        protected UploadAssessmentsRequestViewModel UploadAssessmentsRequestViewModel;
        protected UploadAssessmentsResponseViewModel UploadAssessmentsResponseViewModel;
        protected List<ProviderTlevel> ProviderTlevelDetails;
        protected IFormFile FormFile;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";
        protected Guid BlobUniqueReference;
        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            Logger = Substitute.For<ILogger<AssessmentLoader>>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            FormFile = Substitute.For<IFormFile>();
            BlobUniqueReference = Guid.NewGuid();
            BulkAssessmentRequest = new BulkAssessmentRequest { AoUkprn = Ukprn };
        }

        public override void Given()
        {
            BulkAssessmentResponse = new BulkAssessmentResponse
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            UploadAssessmentsRequestViewModel = new UploadAssessmentsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadAssessmentsResponseViewModel = new UploadAssessmentsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            Mapper.Map<BulkAssessmentRequest>(UploadAssessmentsRequestViewModel).Returns(BulkAssessmentRequest);
            Mapper.Map<UploadAssessmentsResponseViewModel>(BulkAssessmentResponse).Returns(UploadAssessmentsResponseViewModel);
            InternalApiClient.ProcessBulkAssessmentsAsync(BulkAssessmentRequest).Returns(BulkAssessmentResponse);
            Loader = new AssessmentLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessBulkAssessmentsAsync(UploadAssessmentsRequestViewModel);
        }
    }
}
