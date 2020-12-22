using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Assessments
{
    public abstract class TestSetup : BaseTest<CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>>
    {
        protected IValidator<AssessmentCsvRecordRequest> RegValidator;
        protected IDataParser<AssessmentCsvRecordResponse> DataParser;
        protected ILogger<CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>> Logger;

        protected CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> Service { get; private set; }
        protected CsvResponseModel<AssessmentCsvRecordResponse> Response;

        public StringBuilder InputFileContent;

        public override void Setup()
        {
            RegValidator = Substitute.For<IValidator<AssessmentCsvRecordRequest>>();
            DataParser = Substitute.For<IDataParser<AssessmentCsvRecordResponse>>();
            Logger = Substitute.For<ILogger<CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>>>();

            Service = new CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>(RegValidator, DataParser, Logger);
        }

        public async override Task When()
        {
            Response = await Service.ReadAndParseFileAsync(new AssessmentCsvRecordRequest { FileStream = GetInputFileStream() });
        }

        private Stream GetInputFileStream()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(InputFileContent.ToString()));
            return stream;
        }
    }
}
