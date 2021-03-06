﻿using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.ProcessBulkAssessments
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

            BulkAssessmentRequest = new BulkProcessRequest { AoUkprn = Ukprn };

            BulkAssessmentResponse = new BulkAssessmentResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10
                }
            };

            UploadAssessmentsRequestViewModel = new UploadAssessmentsRequestViewModel { AoUkprn = Ukprn, File = FormFile };
            InternalApiClient.ProcessBulkAssessmentsAsync(BulkAssessmentRequest).Returns(BulkAssessmentResponse);
            Loader = new AssessmentLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var bulkAssessmentRequestMapperResult = Mapper.Map<BulkProcessRequest>(UploadAssessmentsRequestViewModel);

            bulkAssessmentRequestMapperResult.AoUkprn.Should().Be(UploadAssessmentsRequestViewModel.AoUkprn);
            bulkAssessmentRequestMapperResult.BlobFileName.Should().NotBeNullOrEmpty();
            bulkAssessmentRequestMapperResult.BlobUniqueReference.Should().NotBeEmpty();
            bulkAssessmentRequestMapperResult.DocumentType.Should().Be(DocumentType.Assessments);
            bulkAssessmentRequestMapperResult.FileType.Should().Be(FileType.Csv);
            bulkAssessmentRequestMapperResult.PerformedBy.Should().Be($"{_givename} {_surname}");

            var uploadAssessmentsResponseMapperResult = Mapper.Map<UploadAssessmentsResponseViewModel>(BulkAssessmentResponse);

            uploadAssessmentsResponseMapperResult.IsSuccess.Should().Be(BulkAssessmentResponse.IsSuccess);
            uploadAssessmentsResponseMapperResult.Stats.Should().NotBeNull();
            uploadAssessmentsResponseMapperResult.Stats.TotalRecordsCount.Should().Be(BulkAssessmentResponse.Stats.TotalRecordsCount);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AssessmentMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<UploadAssessmentsRequestViewModel, BulkProcessRequest>(HttpContextAccessor) :
                                type.Name.Contains("UserEmailResolver") ? (object)new UserEmailResolver<UploadAssessmentsRequestViewModel, BulkProcessRequest>(HttpContextAccessor) :
                                null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
