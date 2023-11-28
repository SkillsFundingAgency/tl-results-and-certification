using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
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

            CreateMap<AdminSearchLearnerDetail, AdminSearchLearnerDetailsViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Provider} ({s.ProviderUkprn})"))
                .ForMember(d => d.AwardingOrganisation, opts => opts.MapFrom(s => s.AwardingOrganisation))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => string.Format(AdminSearchLearnerDetails.Start_Year_Value, s.AcademicYear, s.AcademicYear + 1)));

            CreateMap<PagedResponse<AdminSearchLearnerDetail>, AdminSearchLearnerDetailsListViewModel>()
               .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
               .ForMember(d => d.LearnerDetails, opts => opts.MapFrom(s => s.Records))
               .ForMember(d => d.PagerInfo, opts => opts.MapFrom(s => s.PagerInfo))
               .ForMember(d => d.LearnerDetails, opts => opts.MapFrom(s => s.Records));
        }
    }
}