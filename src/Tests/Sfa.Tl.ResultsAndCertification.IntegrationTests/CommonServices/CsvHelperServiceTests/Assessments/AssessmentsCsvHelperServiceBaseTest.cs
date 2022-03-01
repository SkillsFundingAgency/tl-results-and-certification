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
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Assessments
{
    public abstract class AssessmentsCsvHelperServiceBaseTest : BaseTest
    {
        protected ILogger<CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>> Logger;
        protected ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> Service;
        protected IValidator<AssessmentCsvRecordRequest> Validator;
        protected IDataParser<AssessmentCsvRecordResponse> DataParser;
        protected CsvResponseModel<AssessmentCsvRecordResponse> ReadAndParseFileResponse;
        protected string FilePath;

        protected virtual string GetCodeBaseAbsolutePath()
        {
            var codeBaseUri = new Uri(Assembly.GetExecutingAssembly().Location);
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
                ReadAndParseFileResponse = await Service.ReadAndParseFileAsync(new AssessmentCsvRecordRequest
                {
                    FileStream = stream
                });
            }
        }
    }
}
