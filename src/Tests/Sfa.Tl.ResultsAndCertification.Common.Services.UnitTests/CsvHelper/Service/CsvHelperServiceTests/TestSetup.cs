using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public abstract class TestSetup : BaseTest<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>
    {
        protected IValidator<RegistrationCsvRecordRequest> RegValidator;
        protected IDataParser<RegistrationCsvRecordResponse> DataParser;
        protected ILogger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>> Logger;
        
        protected CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> Service { get; private set; }
        protected CsvResponseModel<RegistrationCsvRecordResponse> Response;
        
        public StringBuilder InputFileContent;

        public override void Setup()
        {
            RegValidator = Substitute.For<IValidator<RegistrationCsvRecordRequest>>();
            DataParser = Substitute.For<IDataParser<RegistrationCsvRecordResponse>>();
            Logger = Substitute.For<ILogger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>>();

            Service = new CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>(RegValidator, DataParser, Logger);
        }
        
        public async override Task When()
        {
            Response = await Service.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = GetInputFileStream() });
        }

        private Stream GetInputFileStream()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(InputFileContent.ToString()));
            return stream;
        }
    }
}
