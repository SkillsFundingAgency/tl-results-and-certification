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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrations
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

            BulkRegistrationRequest = new BulkRegistrationRequest { AoUkprn = Ukprn };

            BulkRegistrationResponse = new BulkRegistrationResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10,
                    NewRecordsCount = 5,
                    AmendedRecordsCount = 3,
                    UnchangedRecordsCount = 2
                }
            };

            UploadRegistrationsRequestViewModel = new UploadRegistrationsRequestViewModel { AoUkprn = Ukprn, File = FormFile };
            InternalApiClient.ProcessBulkRegistrationsAsync(BulkRegistrationRequest).Returns(BulkRegistrationResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var bulkRegistrationRequestMapperResult = Mapper.Map<BulkRegistrationRequest>(UploadRegistrationsRequestViewModel);

            bulkRegistrationRequestMapperResult.AoUkprn.Should().Be(UploadRegistrationsRequestViewModel.AoUkprn);
            bulkRegistrationRequestMapperResult.BlobFileName.Should().NotBeNullOrEmpty();
            bulkRegistrationRequestMapperResult.BlobUniqueReference.Should().NotBeEmpty();
            bulkRegistrationRequestMapperResult.DocumentType.Should().Be(DocumentType.Registrations);
            bulkRegistrationRequestMapperResult.FileType.Should().Be(FileType.Csv);
            bulkRegistrationRequestMapperResult.PerformedBy.Should().Be($"{_givename} {_surname}");

            var uploadRegistrationsResponseMapperResult = Mapper.Map<UploadRegistrationsResponseViewModel>(BulkRegistrationResponse);

            uploadRegistrationsResponseMapperResult.IsSuccess.Should().Be(BulkRegistrationResponse.IsSuccess);
            uploadRegistrationsResponseMapperResult.Stats.Should().NotBeNull();
            uploadRegistrationsResponseMapperResult.Stats.TotalRecordsCount.Should().Be(BulkRegistrationResponse.Stats.TotalRecordsCount);
            uploadRegistrationsResponseMapperResult.Stats.NewRecordsCount.Should().Be(BulkRegistrationResponse.Stats.NewRecordsCount);
            uploadRegistrationsResponseMapperResult.Stats.AmendedRecordsCount.Should().Be(BulkRegistrationResponse.Stats.AmendedRecordsCount);
            uploadRegistrationsResponseMapperResult.Stats.UnchangedRecordsCount.Should().Be(BulkRegistrationResponse.Stats.UnchangedRecordsCount);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(RegistrationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>(HttpContextAccessor) :
                                type.Name.Contains("UserEmailResolver") ? (object)new UserEmailResolver<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>(HttpContextAccessor) :
                                null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
