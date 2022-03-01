using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Results
{
    public abstract class TestSetup : BaseTest<CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>>
    {
        protected IValidator<ResultCsvRecordRequest> RegValidator;
        protected IDataParser<ResultCsvRecordResponse> DataParser;
        protected ILogger<CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>> Logger;

        protected CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse> Service { get; private set; }
        protected CsvResponseModel<ResultCsvRecordResponse> Response;

        public StringBuilder InputFileContent;
        public string Header = "ULN,ComponentCode (Core),AssessmentSeries (Core),ComponentGrade (Core),ComponentCode (Specialisms),AssessmentSeries (Specialisms),ComponentGrade (Specialisms)";

        public override void Setup()
        {
            RegValidator = Substitute.For<IValidator<ResultCsvRecordRequest>>();
            DataParser = Substitute.For<IDataParser<ResultCsvRecordResponse>>();
            Logger = Substitute.For<ILogger<CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>>>();

            Service = new CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>(RegValidator, DataParser, Logger);
        }

        public async override Task When()
        {
            Response = await Service.ReadAndParseFileAsync(new ResultCsvRecordRequest { FileStream = GetInputFileStream() });
        }

        private Stream GetInputFileStream()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(InputFileContent.ToString()));
            return stream;
        }
    }
}
