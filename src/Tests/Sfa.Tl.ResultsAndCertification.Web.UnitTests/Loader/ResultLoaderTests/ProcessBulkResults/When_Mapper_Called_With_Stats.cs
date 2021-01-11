using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ProcessBulkResults
{
    public class When_Mapper_Called_With_Stats : TestSetup
    {
        private readonly string _givename = "test";
        private readonly string _surname = "user";
        private readonly string _email = "test.user@test.com";

        public override void Given()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, _givename),
                    new Claim(ClaimTypes.Surname, _surname),
                    new Claim(ClaimTypes.Email, _email)
                }))
            });

            CreateMapper();

            BulkResultRequest = new BulkProcessRequest { AoUkprn = Ukprn };

            BulkResultResponse = new BulkResultResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10
                }
            };

            UploadResultsRequestViewModel = new UploadResultsRequestViewModel { AoUkprn = Ukprn, File = FormFile };
            InternalApiClient.ProcessBulkResultsAsync(BulkResultRequest).Returns(BulkResultResponse);
            Loader = new ResultLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var bulkResultRequestMapperResult = Mapper.Map<BulkProcessRequest>(UploadResultsRequestViewModel);

            bulkResultRequestMapperResult.AoUkprn.Should().Be(UploadResultsRequestViewModel.AoUkprn);
            bulkResultRequestMapperResult.BlobFileName.Should().NotBeNullOrEmpty();
            bulkResultRequestMapperResult.BlobUniqueReference.Should().NotBeEmpty();
            bulkResultRequestMapperResult.DocumentType.Should().Be(DocumentType.Results);
            bulkResultRequestMapperResult.FileType.Should().Be(FileType.Csv);
            bulkResultRequestMapperResult.PerformedBy.Should().Be($"{_givename} {_surname}");

            var uploadResultsResponseMapperResult = Mapper.Map<UploadResultsResponseViewModel>(BulkResultResponse);

            uploadResultsResponseMapperResult.IsSuccess.Should().Be(BulkResultResponse.IsSuccess);
            uploadResultsResponseMapperResult.Stats.Should().NotBeNull();
            uploadResultsResponseMapperResult.Stats.TotalRecordsCount.Should().Be(BulkResultResponse.Stats.TotalRecordsCount);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ResultMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<UploadResultsRequestViewModel, BulkProcessRequest>(HttpContextAccessor) :
                                type.Name.Contains("UserEmailResolver") ? (object)new UserEmailResolver<UploadResultsRequestViewModel, BulkProcessRequest>(HttpContextAccessor) :
                                null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
