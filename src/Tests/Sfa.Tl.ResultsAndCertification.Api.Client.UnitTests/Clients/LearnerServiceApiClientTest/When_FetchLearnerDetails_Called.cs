using FluentAssertions;
using Lrs.LearnerService.Api.Client;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.LearnerServiceApiClientTest
{
    public class When_FetchLearnerDetails_Called : BaseTest<LearnerServiceApiClient>
    {
        private findLearnerByULNResponse _result;
        private findLearnerByULNResponse _mockHttpResult;
        private RegisteredLearnerDetails _registrationLearnerDetails;
        private ILogger<ILearnerServiceApiClient> _logger;
        private ILearnerPortTypeClient _learnerPortTypeClient;
        private ResultsAndCertificationConfiguration _configuration;
        private LearnerServiceApiClient _apiClient;

        public override void Setup()
        {
            _logger = Substitute.For<ILogger<ILearnerServiceApiClient>>();
            _learnerPortTypeClient = Substitute.For<ILearnerPortTypeClient>();

            _registrationLearnerDetails = new RegisteredLearnerDetails
            {
                Uln = 1234567890,
                Firstname = "First 1",
                Lastname = "Last 1",
                DateofBirth = DateTime.UtcNow.AddYears(-30)
            };

            _configuration = new ResultsAndCertificationConfiguration
            {
                LearningRecordServiceSettings = new LearningRecordServiceSettings { VendorId = 1, Ukprn = "9856741231", Username = "test", Password = "test" }
            };           

            _mockHttpResult = new findLearnerByULNResponse
            {
                FindLearnerResponse = new FindLearnerResp
                {
                    ULN = _registrationLearnerDetails.Uln.ToString(),
                    GivenName = _registrationLearnerDetails.Firstname,
                    FamilyName = _registrationLearnerDetails.Lastname,
                    Learner = new List<Learner> { new Learner 
                    {
                        ULN = _registrationLearnerDetails.Uln.ToString(),
                        GivenName = _registrationLearnerDetails.Firstname,
                        FamilyName = _registrationLearnerDetails.Lastname,
                        Gender = ((int)LrsGender.Male).ToString()                        
                    }}.ToArray(),
                    ResponseCode = Constants.LearnerByUlnExactMatchResponseCode
                }
            };
        }

        public override void Given()
        {
            _apiClient = new LearnerServiceApiClient(_logger, _learnerPortTypeClient, _configuration);
            _learnerPortTypeClient.learnerByULNAsync(Arg.Any<learnerByULNRequest>()).Returns(_mockHttpResult);
        }

        public async override Task When()
        {
            _result = await _apiClient.FetchLearnerDetailsAsync(_registrationLearnerDetails);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.FindLearnerResponse.Should().NotBeNull();
            _result.FindLearnerResponse.Learner.Should().NotBeNull();

            var learner = _result.FindLearnerResponse.Learner[0];
            
            _result.FindLearnerResponse.ULN.Should().Be(_mockHttpResult.FindLearnerResponse.ULN);
            _result.FindLearnerResponse.GivenName.Should().Be(_mockHttpResult.FindLearnerResponse.GivenName);
            _result.FindLearnerResponse.FamilyName.Should().Be(_mockHttpResult.FindLearnerResponse.FamilyName);
            _result.FindLearnerResponse.ResponseCode.Should().Be(_mockHttpResult.FindLearnerResponse.ResponseCode);

            learner.ULN.Should().Be(_mockHttpResult.FindLearnerResponse.Learner[0].ULN);
            learner.GivenName.Should().Be(_mockHttpResult.FindLearnerResponse.Learner[0].GivenName);
            learner.FamilyName.Should().Be(_mockHttpResult.FindLearnerResponse.Learner[0].FamilyName);
            learner.Gender.Should().Be(_mockHttpResult.FindLearnerResponse.Learner[0].Gender);
        }
    }
}
