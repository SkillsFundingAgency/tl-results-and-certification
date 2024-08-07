﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetSoaLearnerRecordDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _providerUkprn = 12345678;
        private readonly int _profileId = 1;
        private readonly long _uln = 987654321;

        // results
        private SoaLearnerRecordDetails _actualResult;
        private SoaLearnerRecordDetails _mockApiResponse;

        // dependencies
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockApiResponse = new SoaLearnerRecordDetails
            {
                ProfileId = _profileId,
                Uln = _uln,
                Firstname = "Test",
                Lastname = "User",
                DateofBirth = DateTime.UtcNow.AddYears(30),
                ProviderName = "Barnsley College",
                ProviderUkprn = 123456789,
                TlevelTitle = "Title",
                RegistrationPathwayId = 11,
                PathwayName = "Design",
                PathwayCode = "8631254",
                PathwayGrade = "A",
                SpecialismName = "Specialism",
                SpecialismCode = "123456",
                SpecialismGrade = "",
                MathsStatus = SubjectStatus.Achieved,
                EnglishStatus = SubjectStatus.Achieved,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                ProviderAddress = new Models.Contracts.ProviderAddress.Address { AddressId = 1, OrganisationName = "Org", DepartmentName = "Dept", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "Town", Postcode = "xx1 1yy" },
                Status = RegistrationPathwayStatus.Active
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<SoaLearnerRecordDetails>(_mockApiResponse, string.Format(ApiConstants.GetSoaLearnerRecordDetailsUri, _providerUkprn, _profileId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetSoaLearnerRecordDetailsAsync(_providerUkprn, _profileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.ProfileId.Should().Be(_mockApiResponse.ProfileId);
            _actualResult.Uln.Should().Be(_mockApiResponse.Uln);
            _actualResult.Firstname.Should().Be(_mockApiResponse.Firstname);
            _actualResult.Lastname.Should().Be(_mockApiResponse.Lastname);
            _actualResult.DateofBirth.Should().Be(_mockApiResponse.DateofBirth);
            _actualResult.RegistrationPathwayId.Should().Be(_mockApiResponse.RegistrationPathwayId);
            _actualResult.ProviderName.Should().Be(_mockApiResponse.ProviderName);
            _actualResult.ProviderUkprn.Should().Be(_mockApiResponse.ProviderUkprn);
            _actualResult.TlevelTitle.Should().Be(_mockApiResponse.TlevelTitle);
            _actualResult.PathwayName.Should().Be(_mockApiResponse.PathwayName);
            _actualResult.PathwayCode.Should().Be(_mockApiResponse.PathwayCode);
            _actualResult.PathwayGrade.Should().Be(_mockApiResponse.PathwayGrade);
            _actualResult.SpecialismName.Should().Be(_mockApiResponse.SpecialismName);
            _actualResult.SpecialismCode.Should().Be(_mockApiResponse.SpecialismCode);
            _actualResult.SpecialismGrade.Should().Be(_mockApiResponse.SpecialismGrade);
            _actualResult.MathsStatus.Should().Be(_mockApiResponse.MathsStatus);
            _actualResult.EnglishStatus.Should().Be(_mockApiResponse.EnglishStatus);
            _actualResult.IndustryPlacementStatus.Should().Be(_mockApiResponse.IndustryPlacementStatus);
            _actualResult.ProviderAddress.Should().BeEquivalentTo(_mockApiResponse.ProviderAddress);
            _actualResult.Status.Should().Be(_mockApiResponse.Status);
            _actualResult.HasPathwayResult.Should().Be(_mockApiResponse.HasPathwayResult);
            _actualResult.IsIndustryPlacementAdded.Should().Be(_mockApiResponse.IsIndustryPlacementAdded);
            _actualResult.IsLearnerRegistered.Should().Be(_mockApiResponse.IsLearnerRegistered);
            _actualResult.IsNotWithdrawn.Should().Be(_mockApiResponse.IsNotWithdrawn);
            _actualResult.IsIndustryPlacementCompleted.Should().Be(_mockApiResponse.IsIndustryPlacementCompleted);
        }
    }
}
