using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IPathwayResultConverter : IValueConverter<TqRegistrationPathway, TqPathwayResult>
    {
    }
}
