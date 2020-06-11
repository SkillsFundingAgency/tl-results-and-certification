using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FluentValidation;
using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Parser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service
{
    public class CsvHelperService<TImportModel, TResponseModel> : ICsvHelperService<TImportModel, TResponseModel> where TResponseModel : class, new() where TImportModel : FileBaseModel
    {
        private readonly IValidator<TImportModel> _validator;
        private readonly IDataParser<TResponseModel> _dataParser;

        public CsvHelperService(IValidator<TImportModel> validator, IDataParser<TResponseModel> dataParser)
        {
            _validator = validator;
            _dataParser = dataParser;
        }

        public async Task<IList<TResponseModel>> ReadAndParseFileAsync(TImportModel importModel)
        {
            var response = new List<TResponseModel>();

            try
            {
                var config = BuildCsvConfiguration();
                
                importModel.FileStream.Position = 0;
                using var reader = new StreamReader(importModel.FileStream);
                using var csv = new CsvReader(reader, config);

                // validate header
                ValidateHeader(csv);

                var columns = importModel.GetType().GetProperties()
                    .Where(pr => pr.GetCustomAttribute<NameAttribute>(false) != null)
                    .ToList();
                
                var rownum = 1;
                while (await csv.ReadAsync())
                {
                    rownum++;

                    // read a row
                    ReadRow(csv, importModel, columns);

                    // validate row
                    TResponseModel row;
                    var validationResult = await ValidateRowAsync(importModel);

                    // parse row into model
                    if (!validationResult.IsValid) 
                        row = _dataParser.ParseErrorObject(rownum, importModel, validationResult);
                    else
                        row = _dataParser.ParseRow(importModel, rownum);

                    if (row == null)
                        throw new Exception(ValidationMessages.UnableToParse);
                    
                    response.Add(row);
                }

                // Todo: at-leaset two records should be available.
            }
            // Todo: check if reporting any csv related error into the file ok?
            catch (UnauthorizedAccessException e)
            {
                CreateAndLogErrorObject(response, e, ValidationMessages.UnAuthorizedFileAccess);
            }
            catch (CsvHelperException e)
            {
                CreateAndLogErrorObject(response, e, ValidationMessages.UnableToReadCsvData);
            }
            catch (Exception e)
            {
                CreateAndLogErrorObject(response, e, ValidationMessages.UnexpectedError);
            }

            return response;
        }

        private async Task<ValidationResult> ValidateRowAsync(TImportModel importModel)
        {
            _validator.CascadeMode = CascadeMode.StopOnFirstFailure;
            var validationResult = await _validator.ValidateAsync(importModel);

            return validationResult;
        }

        private static void ReadRow(CsvReader csv, TImportModel importModel, List<PropertyInfo> columns)
        {
            foreach (var column in columns)
            {
                var nameAttr = column.GetCustomAttribute<NameAttribute>();
                var cellValue = csv.GetField(nameAttr.Names[0]);
                
                column.SetValue(importModel, cellValue);
            }
        }

        private static CsvConfiguration BuildCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                PrepareHeaderForMatch = (string header, int index) => header.ToLower()
            };
        }

        private void CreateAndLogErrorObject(List<TResponseModel> returnModel, Exception ex, string customErrorMessage)
        {
            // Todo: Log the the full error details.
            //var reg = _dataParser.ParseError(customErrorMessage);
            //returnModel.Add(reg);

            // Todo: top-level add error at the wrapper level.
        }

        private static void ValidateHeader(CsvReader csv)
        {
            try
            {
                csv.Read();
                csv.ReadHeader();
                csv.ValidateHeader<TImportModel>();
            }
            catch (Exception ex)
            {
                // Todo: Log actual exception.
                
                // create a new ex
                throw new Exception(ValidationMessages.FileHeaderNotFound);
            }
        }
    }
}
