using FluentValidation;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.IndustryPlacement
{
    public abstract class IndustryPlacementStatusCsvHelperServiceBaseTest : BaseTest
    {
        protected ILogger<CsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse>> Logger;
        protected ICsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse> Service;
        protected IValidator<IndustryPlacementCsvRecordRequest> Validator;
        protected IDataParser<IndustryPlacementCsvRecordResponse> DataParser;
        protected CsvResponseModel<IndustryPlacementCsvRecordResponse> ReadAndParseFileResponse;
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
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                return memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            };

            await using (var stream = File.Open(FilePath, FileMode.Open))
            {
                ReadAndParseFileResponse = await Service.ReadAndParseFileAsync(new IndustryPlacementCsvRecordRequest
                {
                    FileStream = stream
                });
            }
        }
    }
}
