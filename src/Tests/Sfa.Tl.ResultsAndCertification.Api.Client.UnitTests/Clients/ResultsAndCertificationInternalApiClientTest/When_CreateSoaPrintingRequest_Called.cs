using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_CreateSoaPrintingRequest_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private SoaPrintingResponse _result;
        protected SoaPrintingResponse _mockHttpResult;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private SoaPrintingRequest _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new SoaPrintingResponse
            {
                Uln = 1234567891,
                LearnerName = "Test User",
                IsSuccess = true
            };

            _model = new SoaPrintingRequest
            {
                AddressId = 11,
                RegistrationPathwayId = 9,
                Uln = 1111111119,
                LearnerName = "First 9 Last 9",
                LearningDetails = new LearningDetails
                {
                    TLevelTitle = "Design, Surveying and Planning for Construction",
                    Grade = null,
                    Date = "24 April 2021",
                    Core = "Design Surveying and Planning",
                    CoreGrade = "B",
                    OccupationalSpecialism = new List<OccupationalSpecialismDetails>
                    {
                        new OccupationalSpecialismDetails
                        {
                            Specialism = "Surveying and Design for Construction and the Built Environment",
                            Grade = "Merit"
                        }
                    },
                    IndustryPlacement = "Not completed",
                    EnglishAndMaths = "Met"
                },
                SoaPrintingDetails = new SoaPrintingDetails
                {
                    Uln = 1111111119,
                    Name = "First 9 Last 9",
                    Dateofbirth = "10 August 1987",
                    ProviderName = "Barnsley College (10000536)",
                    TlevelTitle = "Design, Surveying and Planning for Construction",
                    Core = "Design Surveying and Planning (78945617)",
                    CoreGrade = "B",
                    Specialism = "Surveying and Design for Construction and the Built Environment (ZT456897)",
                    SpecialismGrade = "Merit",
                    EnglishAndMaths = "Achieved minimum standard",
                    IndustryPlacement = "Not completed",
                    ProviderAddress = new Models.Contracts.ProviderAddress.Address
                    {
                        DepartmentName = "Exams Office",
                        OrganisationName = "Barnsley Academy",
                        AddressLine1 = "Main Block",
                        AddressLine2 = "Farm Road",
                        Town = "Barnsley",
                        Postcode = "S70 3DL"
                    }
                },
                PerformedBy = "Test Provider"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<SoaPrintingResponse>(_mockHttpResult, ApiConstants.CreateSoaPrintingRequestUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.CreateSoaPrintingRequestAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.LearnerName.Should().Be(_mockHttpResult.LearnerName);
            _result.IsSuccess.Should().Be(_mockHttpResult.IsSuccess);
        }
    }
}
