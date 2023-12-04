using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminDashboardMapper : Profile
    {
        public AdminDashboardMapper()
        {
            CreateMap<TqRegistrationPathway, AdminLearnerRecord>()
                .ForMember(p => p.Uln, opts => opts.MapFrom(p => p.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(p => p.FirstName, opts => opts.MapFrom(p => p.TqRegistrationProfile.Firstname))
                .ForMember(p => p.LastName, opts => opts.MapFrom(p => p.TqRegistrationProfile.Lastname))
                .ForMember(p => p.Provider, opts => opts.MapFrom(p => p.TqProvider.TlProvider.Name))
                .ForMember(p => p.StartYear, opts => opts.ConvertUsing(new AcademicYearConverter(), a => a.AcademicYear))
                .ForMember(p => p.TLevel, opts => opts.MapFrom(p => p.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle));
        }
    }
}
