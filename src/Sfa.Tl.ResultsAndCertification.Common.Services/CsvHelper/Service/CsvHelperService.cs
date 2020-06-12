using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FluentValidation;
using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using System;
using System.Collections.Generic;
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

        public CsvHelperService(IValidator<TImportModel> validator, IDataParser<TModel> dataParser)
        {
            _validator = validator;
            _dataParser = dataParser;
        }

        public async Task<TResponseModel> ReadAndParseFileAsync(TImportModel importModel)
        {
            var response = new TResponseModel();
            var rowsModelList = new List<TModel>();
            var config = BuildCsvConfiguration();

            importModel.FileStream.Position = 0;
            using var reader = new StreamReader(importModel.FileStream);
            using var csv = new CsvReader(reader, config);

            // validate header
            var isValidHeader = ValidateHeader(csv);
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

            var properties = importModel.GetType().GetProperties()
                .Where(pr => pr.GetCustomAttribute<NameAttribute>(false) != null)
                .ToList();

            var rownum = 1;
            while (await csv.ReadAsync())
            {
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

            if (rownum == 1)
            {
                response.IsDirty = true;
                response.ErrorMessage = ValidationMessages.NoRecordsFound;
            }
            else
            {
                rowsProperty?.SetValue(rowsModelList, response);
            }

            return response;
        }

        //public async Task<IList<TResponseModel>> ReadAndParseFileAsync_Test(TImportModel importModel)
        //{
        //    var response = new List<TResponseModel>();

        //    var config = BuildCsvConfiguration();

        //    importModel.FileStream.Position = 0;
        //    using var reader = new StreamReader(importModel.FileStream);
        //    using var csv = new CsvReader(reader, config);

        //    // validate header
        //    var isValidHeader = ValidateHeader(csv);
        //    if (!isValidHeader)
        //    {
        //        //Todo: Set Dirty flag and return.
        //        return response;
        //    }

        //    var properties = importModel.GetType().GetProperties()
        //        .Where(pr => pr.GetCustomAttribute<NameAttribute>(false) != null)
        //        .ToList();

        //    var rownum = 1;
        //    while (await csv.ReadAsync())
        //    {
        //        rownum++;

        //        // read a row
        //        ReadRow(csv, importModel, properties);

        //        // validate row
        //        TResponseModel row;
        //        var validationResult = await ValidateRowAsync(importModel);

        //        // parse row into model
        //        if (!validationResult.IsValid)
        //            row = _dataParser.ParseErrorObject(rownum, importModel, validationResult);
        //        else
        //            row = _dataParser.ParseRow(importModel, rownum);

        //        if (row == null)
        //            throw new Exception(ValidationMessages.UnableToParse);

        //        response.Add(row);
        //    }

        //    if (rownum == 1)
        //    {
        //        // Todo: set dirty flag. 
        //    }

        //    return response;
        //}

        private async Task<ValidationResult> ValidateRowAsync(TImportModel importModel)
        {
            _validator.CascadeMode = CascadeMode.StopOnFirstFailure;
            var validationResult = await _validator.ValidateAsync(importModel);

            return validationResult;
        }

        private static void ReadRow(CsvReader csv, TImportModel importModel, List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                var nameAttr = prop.GetCustomAttribute<NameAttribute>();
                var cellValue = csv.GetField(nameAttr.Names[0]);

                prop.SetValue(importModel, cellValue);
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

        private void CreateAndLogErrorObject(CsvResponseModel<TResponseModel> returnModel, Exception ex, string customErrorMessage)
        {
            // Todo: Log the the full error details.

            returnModel.IsDirty = true;
            returnModel.ErrorMessage = customErrorMessage;
        }

        private static bool ValidateHeader(CsvReader csv)
        {
            try
            {
                csv.Read();
                csv.ReadHeader();
                csv.ValidateHeader<TImportModel>();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
