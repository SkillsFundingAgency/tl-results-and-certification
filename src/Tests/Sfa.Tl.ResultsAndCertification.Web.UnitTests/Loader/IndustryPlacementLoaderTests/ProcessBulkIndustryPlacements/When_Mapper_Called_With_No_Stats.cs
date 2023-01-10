using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessBulkIndustryPlacements
{
    public class When_Mapper_Called_With_No_Stats : TestSetup
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

            BulkIndustryPlacementRequest = new BulkProcessRequest { AoUkprn = Ukprn };

            BulkIndustryPlacementResponse = new BulkIndustryPlacementResponse
            {
                IsSuccess = false,
                BlobUniqueReference = Guid.NewGuid(),
                ErrorFileSize = 1.5
            };

            UploadIndustryPlacementsRequestViewModel = new UploadIndustryPlacementsRequestViewModel { AoUkprn = Ukprn, File = FormFile };
            InternalApiClient.ProcessBulkIndustryPlacementsAsync(BulkIndustryPlacementRequest).Returns(BulkIndustryPlacementResponse);
            Loader = new IndustryPlacementLoader(InternalApiClient, Mapper, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var bulkIndustryPlacementRequestMapperResult = Mapper.Map<BulkProcessRequest>(UploadIndustryPlacementsRequestViewModel);

            bulkIndustryPlacementRequestMapperResult.AoUkprn.Should().Be(UploadIndustryPlacementsRequestViewModel.AoUkprn);
            bulkIndustryPlacementRequestMapperResult.BlobFileName.Should().NotBeNullOrEmpty();
            bulkIndustryPlacementRequestMapperResult.BlobUniqueReference.Should().NotBeEmpty();
            bulkIndustryPlacementRequestMapperResult.DocumentType.Should().Be(DocumentType.IndustryPlacements);
            bulkIndustryPlacementRequestMapperResult.FileType.Should().Be(FileType.Csv);
            bulkIndustryPlacementRequestMapperResult.PerformedBy.Should().Be($"{_givename} {_surname}");

            var uploadIndustryPlacementsResponseMapperResult = Mapper.Map<UploadIndustryPlacementsResponseViewModel>(BulkIndustryPlacementResponse);

            uploadIndustryPlacementsResponseMapperResult.IsSuccess.Should().Be(BulkIndustryPlacementResponse.IsSuccess);
            uploadIndustryPlacementsResponseMapperResult.BlobUniqueReference.Should().Be(BulkIndustryPlacementResponse.BlobUniqueReference);
            uploadIndustryPlacementsResponseMapperResult.ErrorFileSize.Should().Be(BulkIndustryPlacementResponse.ErrorFileSize);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(IndustryPlacementMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<UploadIndustryPlacementsRequestViewModel, BulkProcessRequest>(HttpContextAccessor) :
                                type.Name.Contains("UserEmailResolver") ? (object)new UserEmailResolver<UploadIndustryPlacementsRequestViewModel, BulkProcessRequest>(HttpContextAccessor) :
                                null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
