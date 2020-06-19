using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces
{
    public interface IDataParser<out T> where T : class
    {
        T ParseRow(FileBaseModel model, int rownum);
        T ParseErrorObject(int rownum, FileBaseModel importModel, ValidationResult validationResult);
}
    }
