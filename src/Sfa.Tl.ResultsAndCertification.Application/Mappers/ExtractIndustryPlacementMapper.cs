using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.ExtractIndustryPlacement;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ExtractIndustryPlacementMapper : Profile
    {
        public ExtractIndustryPlacementMapper()
        {
            CreateMap<TqRegistrationPathway, ExtractData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.TlevelTitle, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForMember(d => d.UKPRN, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacements.Any() ? s.IndustryPlacements.First().Status.ToString() : "Null"))
                .ForMember(d => d.PendingWithdrawnFlag, opts => opts.MapFrom(s => s.IsPendingWithdrawal.ToString()));
        }
    }
}