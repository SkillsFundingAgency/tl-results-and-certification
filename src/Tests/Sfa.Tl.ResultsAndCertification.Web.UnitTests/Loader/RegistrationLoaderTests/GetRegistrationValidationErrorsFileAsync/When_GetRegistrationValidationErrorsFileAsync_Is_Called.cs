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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationValidationErrorsFileAsync
{
    public abstract class When_GetRegistrationValidationErrorsFileAsync_Is_Called : BaseTest<RegistrationLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected IRegistrationLoader Loader;
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
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
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
                DocumentType = (int)DocumentType.Registrations,
                FileType = (int)FileType.Csv,
                Status = (int)DocumentUploadStatus.Failed,
                CreatedBy = $"{Givenname} {Surname}"
            };

            InternalApiClient.GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference).Returns(ApiResponse);
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public override void When()
        {
            ActualResult = Loader.GetRegistrationValidationErrorsFileAsync(Ukprn, BlobUniqueReference).Result;
        }
    }
}
