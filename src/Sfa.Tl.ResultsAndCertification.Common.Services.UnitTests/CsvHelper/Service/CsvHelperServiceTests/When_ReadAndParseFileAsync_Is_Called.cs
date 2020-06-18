using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public abstract class When_ReadAndParseFileAsync_Is_Called : BaseTest<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>
    {
        protected IValidator<RegistrationCsvRecordRequest> RegValidator;
        protected IDataParser<RegistrationCsvRecordResponse> DataParser;
        protected ILogger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>> Logger;
        
        protected CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> Service { get; private set; }
        protected virtual MemoryStream InputStream { get; set; }

        protected Task<CsvResponseModel<RegistrationCsvRecordResponse>> Response;

        public override void Setup()
        {
            RegValidator = Substitute.For<IValidator<RegistrationCsvRecordRequest>>();
            DataParser = Substitute.For<IDataParser<RegistrationCsvRecordResponse>>();
            Logger = Substitute.For<ILogger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>>();

            Service = new CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>(RegValidator, DataParser, Logger);
        }
        
        public override void When()
        {
            Response = Service.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = InputStream });
        }
    }
}
