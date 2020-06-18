using FluentValidation;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using System;
using System.IO;
using System.Reflection;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Registrations
{
    public abstract class RegistrationsCsvHelperServiceBaseTest : BaseTest
    {  
        protected ILogger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>> Logger;
        protected ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> Service;
        protected IValidator<RegistrationCsvRecordRequest> Validator;
        protected IDataParser<RegistrationCsvRecordResponse> DataParser;
        protected CsvResponseModel<RegistrationCsvRecordResponse> ReadAndParseFileResponse;
        protected string FilePath;

        protected virtual string GetCodeBaseAbsolutePath()
        {
            var codeBaseUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            return Uri.UnescapeDataString(codeBaseUri.AbsolutePath);
        }

        public override void When()
        {
            using (var stream = File.Open(FilePath, FileMode.Open))
            {
                ReadAndParseFileResponse = Service.ReadAndParseFileAsync(new RegistrationCsvRecordRequest
                {
                    FileStream = stream
                }).Result;
            }
        }
    }
}
