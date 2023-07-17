using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IIndustryPlacementStatusConverter : IValueConverter<TqRegistrationPathway, IndustryPlacementStatus>
    {
    }
}