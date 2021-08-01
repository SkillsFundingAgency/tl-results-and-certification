using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessPrintingRequest_With_Valid_Data : TestSetup
    {
        private List<PrintRequest> _printRequests;
        private CertificatePrintingResponse _expectedResult;
        private PrintResponse _apiResponse;

        public override void Given()
        {
            var printRequest = new PrintRequest
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

            _printRequests = new List<PrintRequest> { printRequest };
            PrintingService.GetPendingPrintRequestsAsync().Returns(_printRequests);

            _apiResponse = new PrintResponse { PrintRequestResponse = new PrintRequestResponse { BatchNumber = printRequest.Batch.BatchNumber, Errors = new List<Error>(), Status = "Success"  } };
            PrintingApiClient.ProcessPrintRequestAsync(Arg.Any<PrintRequest>()).Returns(_apiResponse);

            _expectedResult = new CertificatePrintingResponse { IsSuccess = true, TotalCount = 1, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1 };
            PrintingService.UpdatePrintRequestResponsesAsync(Arg.Any<List<PrintRequestResponse>>()).Returns(_expectedResult);
        }

        public async override Task When()
        {
            ActualResult = await Service.ProcessPrintingRequestAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            PrintingService.Received(1).GetPendingPrintRequestsAsync();
            PrintingApiClient.Received(1).ProcessPrintRequestAsync(Arg.Any<PrintRequest>());
            PrintingService.Received(1).UpdatePrintRequestResponsesAsync(Arg.Any<List<PrintRequestResponse>>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(_expectedResult.IsSuccess);
            ActualResult.TotalCount.Should().Be(_expectedResult.TotalCount);
            ActualResult.PrintingProcessedCount.Should().Be(_expectedResult.PrintingProcessedCount);
            ActualResult.ModifiedCount.Should().Be(_expectedResult.ModifiedCount);
            ActualResult.SavedCount.Should().Be(_expectedResult.SavedCount);
        }
    }
}
