using FluentValidation;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) => {
                return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            };

            await using (var stream = File.Open(FilePath, FileMode.Open))
            {
                ReadAndParseFileResponse = await Service.ReadAndParseFileAsync(new RegistrationCsvRecordRequest
                {
                    FileStream = stream
                });
            }
        }
    }
}
