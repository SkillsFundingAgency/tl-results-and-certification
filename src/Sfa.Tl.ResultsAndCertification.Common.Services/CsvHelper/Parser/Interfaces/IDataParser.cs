using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Parser.Interfaces
{
    public interface IDataParser<out T> where T : class
    {
        T ParseRow(FileBaseModel model, int rownum);
        T ParseErrorObject(int rownum, FileBaseModel importModel, ValidationResult validationResult);
}
    }
