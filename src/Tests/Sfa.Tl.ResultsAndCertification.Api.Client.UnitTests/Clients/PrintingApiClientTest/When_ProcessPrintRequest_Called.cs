using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.PrintingApiClientTest
{
    public class When_ProcessPrintRequest_Called : BaseTest<PrintingApiClient>
    {        
        protected PrintToken _mockTokenHttpResult;
        private PrintResponse _result;        
        private PrintRequest _printRequest;
        protected PrintResponse _mockHttpResult;        
        private ResultsAndCertificationConfiguration _configuration;
        private PrintingApiClient _apiClient;

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                PrintingApiSettings = new PrintingApiSettings { Uri = "http://apitest.taone.co.uk", Username = "test", Password = "test" }
            };

            _printRequest = new PrintRequest
            {
                Batch = new PrintBatch { BatchNumber = 1, BatchDate = DateTime.UtcNow.ToPrintBatchDateFormat(), PostalContactCount = 1, TotalCertificateCount = 1 },
                PrintData = new List<PrintData>
                {
                    new PrintData {
                        PostalContact = new PostalContact
                        {
                            DepartmentName = "Exams Office",
                            Name = "Barnsley College",
                            ProviderName = "Barnsley College",
                            UKPRN = "123456789",
                            AddressLine1 = "Line1",
                            AddressLine2 = "",
                            Town = "Town",
                            Postcode = "AA1 1BB",
                            CertificateCount = 1
                        },
                        Certificates = new List<Certificate>
                        {
                           new Certificate
                           {
                               CertificateNumber = "00000001",
                               Type = PrintCertificateType.StatementOfAchievement.ToString(),
                               Uln = 1234587912,
                               LearnerName = "First Last",
                               LearningDetails = new LearningDetails
                               {
                                    TLevelTitle = "Design, Surveying and Planning for Construction",
                                    Grade = null,
                                    Date = "24 April 2021",
                                    Core = "Design Surveying and Planning",
                                    CoreGrade = "B",
                                    OccupationalSpecialism = new List<OccupationalSpecialism>
                                    {
                                        new OccupationalSpecialism
                                        {
                                            Specialism = "Surveying and Design for Construction and the Built Environment",
                                            Grade = "Merit"
                                        }
                                    },
                                    IndustryPlacement = "Not completed",
                                    EnglishAndMaths = "Met"
                               }
                           }
                        }
                    }
                }
            };

            _mockTokenHttpResult = new PrintToken { Token = Guid.NewGuid().ToString() };
            _mockHttpResult = new PrintResponse { PrintRequestResponse = new PrintRequestResponse { BatchNumber = 1, Status = ResponseStatus.Success.ToString(), Errors = new List<Error>() } };
        }

        public override void Given()
        {            
            var mockHttpHandler = new MockHttpMessageHandler<PrintToken>(_mockTokenHttpResult, string.Format(ApiConstants.PrintingTokenUri, _configuration.PrintingApiSettings.Username, _configuration.PrintingApiSettings.Password), HttpStatusCode.OK);
            mockHttpHandler.AddHttpResponses(_mockHttpResult, string.Format(ApiConstants.PrintRequestUri, _mockTokenHttpResult.Token), HttpStatusCode.OK, JsonConvert.SerializeObject(_printRequest));

            HttpClient = new HttpClient(mockHttpHandler);
            _apiClient = new PrintingApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.ProcessPrintRequestAsync(_printRequest);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.PrintRequestResponse.BatchNumber.Should().Be(_mockHttpResult.PrintRequestResponse.BatchNumber);
            _result.PrintRequestResponse.Status.Should().Be(_mockHttpResult.PrintRequestResponse.Status);
            _result.PrintRequestResponse.Errors.Should().BeEquivalentTo(_mockHttpResult.PrintRequestResponse.Errors);
        }
    }
}
