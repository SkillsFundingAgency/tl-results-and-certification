using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_FindPrsLearnerRecord_Called_With_ProfileId : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _aoUkprn = 12345678;
        private readonly int _profileId = 87654321;
        private IList<PrsAssessment> _pathwayAssessments;

        // results
        private FindPrsLearnerRecord _actualResult;
        private FindPrsLearnerRecord _mockApiResponse;

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

            _pathwayAssessments = new List<PrsAssessment>
            {
                new PrsAssessment { AssessmentId = 11, SeriesName = "Summer 2021", HasResult = true },
                new PrsAssessment { AssessmentId = 12, SeriesName = "Autumn 2021", HasResult = true }
            };
            _mockApiResponse = new FindPrsLearnerRecord
            {
                ProfileId = 11,
                Uln = _profileId,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow.AddYears(30),
                ProviderName = "Barnsley College",
                ProviderUkprn = 123456789,
                TlevelTitle = "Title",
                Status = RegistrationPathwayStatus.Active,
                PathwayAssessments = _pathwayAssessments
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<FindPrsLearnerRecord>(_mockApiResponse, string.Format(ApiConstants.FindPrsLearnerRecordByProfileIdUri, _aoUkprn, _profileId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.FindPrsLearnerRecordAsync(_aoUkprn, null, _profileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Uln.Should().Be(_mockApiResponse.Uln);
            _actualResult.Firstname.Should().Be(_mockApiResponse.Firstname);
            _actualResult.Lastname.Should().Be(_mockApiResponse.Lastname);
            _actualResult.DateofBirth.Should().Be(_mockApiResponse.DateofBirth);
            _actualResult.ProviderName.Should().Be(_mockApiResponse.ProviderName);
            _actualResult.ProviderUkprn.Should().Be(_mockApiResponse.ProviderUkprn);
            _actualResult.TlevelTitle.Should().Be(_mockApiResponse.TlevelTitle);
            _actualResult.Status.Should().Be(_mockApiResponse.Status);
            _actualResult.SingleAssessmentWithNoGrade.Should().BeFalse();
            _actualResult.PathwayAssessments.Should().NotBeEmpty();
            _actualResult.PathwayAssessments.Count().Should().Be(_pathwayAssessments.Count());

            for (int i = 0; i < _pathwayAssessments.Count(); i++)
            {
                _actualResult.PathwayAssessments.ElementAt(i).AssessmentId.Should().Be(_pathwayAssessments[i].AssessmentId);
                _actualResult.PathwayAssessments.ElementAt(i).SeriesName.Should().Be(_pathwayAssessments[i].SeriesName);
                _actualResult.PathwayAssessments.ElementAt(i).HasResult.Should().Be(_pathwayAssessments[i].HasResult);
            }
        }
    }
}
