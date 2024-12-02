using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAdminLearnerRecord_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly int _pathwayId = 1;
        protected AdminLearnerRecord _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private AdminLearnerRecord _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AdminLearnerRecord
            {
                RegistrationPathwayId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = new DateTime(2005, 1, 23),
                MathsStatus = SubjectStatus.Achieved,
                EnglishStatus = SubjectStatus.Achieved,
                OverallCalculationStatus = CalculationStatus.Completed,
                Pathway = new Pathway
                {
                    Id = 1,
                    LarId = "6100008X",
                    Name = "T Level in Finance",
                    AcademicYear = 2020,
                    IndustryPlacements = new IndustryPlacement[]
                   {
                        new IndustryPlacement
                        {
                            Id = 5,
                            Status = IndustryPlacementStatus.Completed
                        },
                   },
                    Provider = new Provider
                    {
                        Id = 2,
                        Ukprn = 10000536,
                        Name = "Barnsley College",
                        DisplayName = "Barnsley College"
                    }
                },
                AwardingOrganisation = new AwardingOrganisation
                {
                    Id = 1,
                    Ukprn = 10009696,
                    Name = "Ncfe",
                    DisplayName = "NCFE"
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AdminLearnerRecord>(_mockHttpResult, string.Format(ApiConstants.GetAdminLearnerRecordUri, _pathwayId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAdminLearnerRecordAsync(_pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}
