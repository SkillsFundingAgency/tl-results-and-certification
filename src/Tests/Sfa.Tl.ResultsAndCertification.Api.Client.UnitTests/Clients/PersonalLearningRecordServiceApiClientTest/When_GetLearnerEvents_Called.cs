using FluentAssertions;
using Lrs.PersonalLearningRecordService.Api.Client;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.PersonalLearningRecordServiceApiClientTest
{
    public class When_GetLearnerEvents_Called : BaseTest<PersonalLearningRecordServiceApiClient>
    {
        private GetLearnerLearningEventsResponse _result;
        private GetLearnerLearningEventsResponse _mockHttpResult;
        private RegistrationLearnerDetails _registrationLearnerDetails;        
        private ILogger<ILearnerServiceR9Client> _logger;
        private ILearnerServiceR9Client _learnerServiceR9Client;
        private ResultsAndCertificationConfiguration _configuration;
        private PersonalLearningRecordServiceApiClient _apiClient;

        public override void Setup()
        {
            _logger = Substitute.For<ILogger<ILearnerServiceR9Client>>();
            _learnerServiceR9Client = Substitute.For<ILearnerServiceR9Client>();

            _registrationLearnerDetails = new RegistrationLearnerDetails
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

            _mockHttpResult = new GetLearnerLearningEventsResponse
            {
                FoundULN = _registrationLearnerDetails.Uln.ToString(),
                IncomingULN = _registrationLearnerDetails.Uln.ToString(),
                LearnerRecord = new LearningEvent[] { new LearningEvent { Grade = "Pass", SubjectCode = "100/2510/8" }, new LearningEvent { Grade = "Fail", SubjectCode = "100/2508/X" } }
            };
        }

        public override void Given()
        {
            _apiClient = new PersonalLearningRecordServiceApiClient(_logger, _learnerServiceR9Client, _configuration);
            _learnerServiceR9Client.GetLearnerLearningEventsAsync(Arg.Any<InvokingOrganisationR10>(), "ORG", _configuration.LearningRecordServiceSettings.VendorId, "ENG",
                                                                    _registrationLearnerDetails.Uln.ToString(), _registrationLearnerDetails.Firstname, _registrationLearnerDetails.Lastname,
                                                                    _registrationLearnerDetails.DateofBirth.ToString("yyyy-MM-dd"), null, "FULL").Returns(_mockHttpResult);            
        }

        public async override Task When()
        {
            _result = await _apiClient.GetLearnerEventsAsync(_registrationLearnerDetails.Uln.ToString(), _registrationLearnerDetails.Firstname, _registrationLearnerDetails.Lastname, _registrationLearnerDetails.DateofBirth);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.FoundULN.Should().Be(_mockHttpResult.FoundULN);
            _result.IncomingULN.Should().Be(_mockHttpResult.IncomingULN);
            _result.LearnerRecord.Should().NotBeNullOrEmpty();
            _result.LearnerRecord.Length.Should().Be(_mockHttpResult.LearnerRecord.Length);
            _result.LearnerRecord.Should().BeEquivalentTo(_mockHttpResult.LearnerRecord);
        }
    }
}
