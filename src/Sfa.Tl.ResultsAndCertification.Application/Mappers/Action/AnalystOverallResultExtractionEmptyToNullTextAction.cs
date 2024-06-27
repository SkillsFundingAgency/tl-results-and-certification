using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Action
{
    internal class AnalystOverallResultExtractionEmptyToNullTextAction : IMappingAction<TqRegistrationPathway, AnalystOverallResultExtractionData>
    {
        private const string Null = "NULL";

        public void Process(TqRegistrationPathway source, AnalystOverallResultExtractionData destination, ResolutionContext context)
        {
            destination.Status = ToNullTextIfNullOrEmpty(destination.Status);
            destination.ProviderName = ToNullTextIfNullOrEmpty(destination.ProviderName);
            destination.LastName = ToNullTextIfNullOrEmpty(destination.LastName);
            destination.FirstName = ToNullTextIfNullOrEmpty(destination.FirstName);
            destination.Gender = ToNullTextIfNullOrEmpty(destination.Gender);
            destination.TlevelTitle = ToNullTextIfNullOrEmpty(destination.TlevelTitle);
            destination.StartYear = ToNullTextIfNullOrEmpty(destination.StartYear);
            destination.CoreComponent = ToNullTextIfNullOrEmpty(destination.CoreComponent);
            destination.CoreCode = ToNullTextIfNullOrEmpty(destination.CoreCode);
            destination.CoreResult = ToNullTextIfNullOrEmpty(destination.CoreResult);
            destination.OccupationalSpecialism = ToNullTextIfNullOrEmpty(destination.OccupationalSpecialism);
            destination.SpecialismCode = ToNullTextIfNullOrEmpty(destination.SpecialismCode);
            destination.SpecialismResult = ToNullTextIfNullOrEmpty(destination.SpecialismResult);
            destination.IndustryPlacementStatus = ToNullTextIfNullOrEmpty(destination.IndustryPlacementStatus);
            destination.OverallResult = ToNullTextIfNullOrEmpty(destination.OverallResult);
        }

        private static string ToNullTextIfNullOrEmpty(string value)
            => string.IsNullOrWhiteSpace(value) ? Null : value;
    }
}