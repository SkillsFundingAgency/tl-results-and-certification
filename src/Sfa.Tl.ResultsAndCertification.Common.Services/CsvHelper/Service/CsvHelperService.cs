using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service
{
    public class CsvHelperService<TImportModel, TResponseModel, TModel> : ICsvHelperService<TImportModel, TResponseModel, TModel> where TModel : class, new() where TResponseModel : CsvResponseBaseModel, new() where TImportModel : FileBaseModel
    {
        private readonly IValidator<TImportModel> _validator;
        private readonly IDataParser<TModel> _dataParser;
        private readonly ILogger<CsvHelperService<TImportModel, TResponseModel, TModel>> _logger;

        public CsvHelperService(IValidator<TImportModel> validator, 
            IDataParser<TModel> dataParser, 
            ILogger<CsvHelperService<TImportModel, TResponseModel, TModel>> logger)
        {
            _validator = validator;
            _dataParser = dataParser;
            _logger = logger;
        }

        public async Task<TResponseModel> ReadAndParseFileAsync(TImportModel importModel)
        {
            var response = new TResponseModel();
            var rowsModelList = new List<TModel>();
            var config = BuildCsvConfiguration();

            var properties = importModel.GetType().GetProperties()
                .Where(pr => pr.GetCustomAttribute<ColumnAttribute>(false) != null)
                .ToList();

            importModel.FileStream.Position = 0;
            using var reader = new StreamReader(importModel.FileStream);
            using var csv = new CsvReader(reader, config);

            // validate header
            var isValidHeader = ValidateHeader(csv, properties);
            if (!isValidHeader)
            {
                response.IsDirty = true;
                response.ErrorMessage = ValidationMessages.FileHeaderNotFound;
                return response;
            }

            var rowsProperty = response.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType 
                                && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)
                                && p.PropertyType.GenericTypeArguments.First() == typeof(TModel));

            var rownum = 1;
            while (true)
            {
                try
                {
                    if (!await csv.ReadAsync())
                        break;
                    rownum++;

                    // read a row
                    ReadRow(csv, importModel, properties);

                    // validate row
                    TModel row;
                    var validationResult = await ValidateRowAsync(importModel);

                    // parse row into model
                    if (!validationResult.IsValid)
                        row = _dataParser.ParseErrorObject(rownum, importModel, validationResult);
                    else
                        row = _dataParser.ParseRow(importModel, rownum);

                    if (row == null)
                        throw new Exception(ValidationMessages.UnableToParse);

                    rowsModelList.Add(row);
                }
                catch (BadDataException)
                {
                    rownum++;
                    TModel row = _dataParser.ParseErrorObject(rownum, null, null, string.Format(ValidationMessages.InvalidColumnFound, properties.Count));
                    rowsModelList.Add(row);
                }
            }

            if (rownum == 1)
            {
                response.IsDirty = true;
                response.ErrorMessage = ValidationMessages.NoRecordsFound;
            }
            else
            {
                rowsProperty?.SetValue(response, rowsModelList);
            }

            return response;
        }

        public async Task<byte[]> WriteFileAsync<T>(IList<T> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using var ms = new MemoryStream();
            using (var sw = new StreamWriter(ms))
            using (var cw = new CsvWriter(sw, config))
            {
                var headerNames = typeof(T).GetProperties().Select(pr => pr.GetCustomAttribute<DisplayNameAttribute>(false).DisplayName).ToList();
                headerNames.ForEach(headerName =>
                {
                    cw.WriteField(headerName);
                });
                cw.NextRecord();
                await cw.WriteRecordsAsync<T>(data);
            }
            return ms.ToArray();
        }

        private async Task<ValidationResult> ValidateRowAsync(TImportModel importModel)
        {
            _validator.CascadeMode = CascadeMode.StopOnFirstFailure;
            var validationResult = await _validator.ValidateAsync(importModel);

            return validationResult;
        }

        private static void ReadRow(CsvReader csv, TImportModel importModel, List<PropertyInfo> properties)
        {
            properties.ForEach(prop =>
            {
                prop.SetValue(importModel, csv.GetField(prop.GetCustomAttribute<ColumnAttribute>(false).Name));
            });
        }

        private static CsvConfiguration BuildCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                PrepareHeaderForMatch = (string header, int index) => header.Trim().ToLower(),
                DetectColumnCountChanges = true
            };
        }

        private static bool ValidateHeader(CsvReader csv, List<PropertyInfo> properties)
        {
            try
            {
                csv.Read();
                csv.ReadHeader();

                var csvFileHeaderColumns = csv.Context.HeaderRecord.Select(x => x.Trim());
                var entityHeaderColumns = properties.Select(x => x.GetCustomAttribute<ColumnAttribute>(false).Name);

                if (entityHeaderColumns.Count() != csvFileHeaderColumns.Count()) return false;

                var hasAnyAdditionalHeaders = csvFileHeaderColumns.Except(entityHeaderColumns, StringComparer.OrdinalIgnoreCase).Any();
                return !hasAnyAdditionalHeaders;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
