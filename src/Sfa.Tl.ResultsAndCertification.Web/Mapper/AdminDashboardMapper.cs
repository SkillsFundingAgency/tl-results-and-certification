using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminDashboardMapper : Profile
    {
        public AdminDashboardMapper()
        {
            CreateMap<AdminSearchLearnerFilters, AdminSearchLearnerFiltersViewModel>()
                .ForMember(d => d.AwardingOrganisations, opts => opts.MapFrom(s => s.AwardingOrganisations))
                .ForMember(d => d.AcademicYears, opts => opts.MapFrom(s => s.AcademicYears));

            CreateMap<AdminLearnerRecord, AdminChangeStartYearViewModel>();
        }
    }
}