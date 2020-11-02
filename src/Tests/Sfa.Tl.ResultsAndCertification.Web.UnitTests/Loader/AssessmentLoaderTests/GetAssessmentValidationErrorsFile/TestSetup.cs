using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentValidationErrorsFile
{
    public abstract class TestSetup : BaseTest<AssessmentLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<AssessmentLoader> Logger;
        protected IAssessmentLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected Stream ActualResult;
        protected DocumentUploadHistoryDetails ApiResponse;
        protected readonly long Ukprn = 12345678;
        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string BlobFileName = "inputfile.csv";
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            Logger = Substitute.For<ILogger<AssessmentLoader>>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
        }

        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            ApiResponse = new DocumentUploadHistoryDetails
            {
                AoUkprn = Ukprn,
                BlobFileName = BlobFileName,
                BlobUniqueReference = BlobUniqueReference,
                DocumentType = (int)DocumentType.Assessments,
                FileType = (int)FileType.Csv,
                Status = (int)DocumentUploadStatus.Failed,
                CreatedBy = $"{Givenname} {Surname}"
            };

            InternalApiClient.GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference).Returns(ApiResponse);
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            Loader = new AssessmentLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetAssessmentValidationErrorsFileAsync(Ukprn, BlobUniqueReference);
        }
    }
}
