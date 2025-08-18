using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Action
{
    internal class AnalystOverallResultExtractionEmptyToNullTextAction : IMappingAction<TqRegistrationPathway, AnalystOverallResultExtractionData>
    {
        private const char QuotationMark = '"', Space = ' ';
        private const string Null = "NULL";

        public void Process(TqRegistrationPathway source, AnalystOverallResultExtractionData destination, ResolutionContext context)
        {
            destination.Status = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.Status);
            destination.ProviderName = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.ProviderName);
            destination.LastName = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.LastName);
            destination.FirstName = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.FirstName);
            destination.Gender = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.Gender);
            destination.TlevelTitle = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.TlevelTitle);
            destination.StartYear = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.StartYear);
            destination.CoreComponent = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.CoreComponent);
            destination.CoreCode = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.CoreCode);
            destination.CoreResult = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.CoreResult);
            destination.HighestAttainedCoreSeries = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.HighestAttainedCoreSeries);
            destination.OccupationalSpecialism = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.OccupationalSpecialism);
            destination.SpecialismCode = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.SpecialismCode);
            destination.SpecialismResult = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.SpecialismResult);
            destination.IndustryPlacementStatus = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.IndustryPlacementStatus);
            destination.OverallResult = ToNullTextIfNullOrEmptyOrOnlyQuotes(destination.OverallResult);
        }

        private static string ToNullTextIfNullOrEmptyOrOnlyQuotes(string value)
            => string.IsNullOrWhiteSpace(value)
                ? Null
                : string.IsNullOrEmpty(value.Trim(QuotationMark, Space))
                    ? Null
                    : value;
    }
}