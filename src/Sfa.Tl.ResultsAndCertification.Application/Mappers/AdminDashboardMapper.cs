using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminDashboardLearnerMapper : Profile
    {
        public AdminDashboardLearnerMapper()
        {
            CreateMap<TqRegistrationPathway, AdminLearnerRecord>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.MathsStatus, opts => opts.MapFrom(s => s.TqRegistrationProfile.MathsStatus))
                .ForMember(d => d.EnglishStatus, opts => opts.MapFrom(s => s.TqRegistrationProfile.EnglishStatus))
                .ForMember(d => d.Pathway, opts => opts.MapFrom(s => s))
                .ForMember(d => d.AwardingOrganisation, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton))
                .ForMember(d => d.OverallCalculationStatus, opts => opts.MapFrom(s => GetOverallCalculationStatus(s.OverallResults)));

            CreateMap<TlAwardingOrganisation, AwardingOrganisation>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.UkPrn))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName));
        }

        private CalculationStatus? GetOverallCalculationStatus(ICollection<OverallResult> overallResults)
        {
            if (overallResults.IsNullOrEmpty())
            {
                return null;
            }

            OverallResult overallResult = overallResults.First();
            return overallResult.CalculationStatus;
        }
    }
}
