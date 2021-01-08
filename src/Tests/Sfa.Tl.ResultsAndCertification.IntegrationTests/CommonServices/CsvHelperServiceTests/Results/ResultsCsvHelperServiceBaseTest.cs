using FluentValidation;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Results
{
    public abstract class ResultsCsvHelperServiceBaseTest : BaseTest
    {
        protected ILogger<CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>> Logger;
        protected ICsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse> Service;
        protected IValidator<ResultCsvRecordRequest> Validator;
        protected IDataParser<ResultCsvRecordResponse> DataParser;
        protected CsvResponseModel<ResultCsvRecordResponse> ReadAndParseFileResponse;
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
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            };

            await using (var stream = File.Open(FilePath, FileMode.Open))
            {
                ReadAndParseFileResponse = await Service.ReadAndParseFileAsync(new ResultCsvRecordRequest
                {
                    FileStream = stream
                });
            }
        }
    }
}
