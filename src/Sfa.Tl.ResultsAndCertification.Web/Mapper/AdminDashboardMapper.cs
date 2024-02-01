using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminDashboardMapper : Profile
    {
        public AdminDashboardMapper()
        {
            CreateMap<AdminSearchLearnerFilters, AdminSearchLearnerFiltersViewModel>()
                .ForMember(d => d.AwardingOrganisations, opts => opts.MapFrom(s => s.AwardingOrganisations))
                .ForMember(d => d.AcademicYears, opts => opts.MapFrom(s => s.AcademicYears));

            CreateMap<AdminLearnerRecord, AdminLearnerRecordViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.TlPathwayId, opts => opts.MapFrom(s => s.Pathway.Id))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.AwardingOrganisationName, opts => opts.MapFrom(s => s.AwardingOrganisation.DisplayName))
                .ForMember(d => d.MathsStatus, opts => opts.MapFrom(s => s.MathsStatus))
                .ForMember(d => d.EnglishStatus, opts => opts.MapFrom(s => s.EnglishStatus))
                .ForMember(d => d.RegistrationPathwayStatus, opts => opts.MapFrom(s => s.Pathway.Status))
                .ForMember(d => d.IsLearnerRegistered, opts => opts.MapFrom(s => s.IsRegistered))
                .ForMember(d => d.IndustryPlacementId, opts => opts.MapFrom(s => GetIndustryPlacementId(s)))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => GetIndustryPlacementStatus(s)))
                .ForMember(d => d.AssessmentDetails, opts => opts.MapFrom(s => s.Pathway));

            CreateMap<Pathway, AdminAssessmentDetailsViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["registrationPathwayId"]))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"))
                .ForMember(d => d.PathwayAssessments, opts => opts.MapFrom(s => s.PathwayAssessments))
                .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => s.Specialisms));

            CreateMap<Assessment, AdminAssessmentViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["registrationPathwayId"]))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Result != null ? s.Result.Grade : null))
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedOn.ToDobFormat() : null))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedBy : null));

            CreateMap<Specialism, AdminSpecialismViewModel>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["registrationPathwayId"]))
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"))
               .ForMember(d => d.Assessments, opts => opts.MapFrom(s => s.Assessments));

            CreateMap<AdminSearchLearnerDetail, AdminSearchLearnerDetailsViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Provider} ({s.ProviderUkprn})"))
                .ForMember(d => d.AwardingOrganisation, opts => opts.MapFrom(s => s.AwardingOrganisation))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.AcademicYear)));

            CreateMap<PagedResponse<AdminSearchLearnerDetail>, AdminSearchLearnerDetailsListViewModel>()
                .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
                .ForMember(d => d.LearnerDetails, opts => opts.MapFrom(s => s.Records))
                .ForMember(d => d.PagerInfo, opts => opts.MapFrom(s => s.PagerInfo));

            CreateMap<AdminLearnerRecord, AdminChangeStartYearViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.Pathway.Id))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.TlevelStartYear, opts => opts.MapFrom(s => s.Pathway.StartYear))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.DisplayAcademicYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.AcademicStartYearsToBe, opts => opts.MapFrom(s => GetChangeAcademicYearOptions(s.Pathway.AcademicYear, s.Pathway.StartYear)))
                .ForMember(d => d.LearnerRegistrationPathwayStatus, opts => opts.MapFrom(s => s.Pathway.Status))
                .ForMember(d => d.OverallCalculationStatus, opts => opts.MapFrom(s => s.OverallCalculationStatus));

            CreateMap<AdminSearchLearnerCriteriaViewModel, AdminSearchLearnerRequest>()
                .ForMember(d => d.SearchKey, opts => opts.MapFrom(s => s.SearchKey))
                .ForMember(d => d.PageNumber, opts => opts.MapFrom(s => s.PageNumber))
                .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => GetSelectedProviderId(s)))
                .ForMember(d => d.SelectedAcademicYears, opts => opts.MapFrom(s => GetSelectedAcademicYearIds(s)))
                .ForMember(d => d.SelectedAwardingOrganisations, opts => opts.MapFrom(s => GetSelectedAwardingOrganisationIds(s)));

            CreateMap<AdminLearnerRecord, ReviewChangeStartYearViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.DisplayAcademicYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)));

            CreateMap<AdminLearnerRecord, AdminIpCompletionViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => GetIndustryPlacementStatus(s)))
                .ForMember(d => d.IndustryPlacementStatusTo, opts => opts.MapFrom(s => GetIndustryPlacementStatus(s)));

            CreateMap<ReviewChangeStartYearViewModel, ReviewChangeStartYearRequest>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.AcademicYearTo, opts => opts.MapFrom(s => s.AcademicYearTo))
               .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
               .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
               .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
               .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId))
               .ForMember(d => d.ChangeStartYearDetails, opts => opts.MapFrom(s => s))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.LoggedInUser))
               .ForMember(d => d.DisplayAcademicYear, opts => opts.MapFrom(s => s.DisplayAcademicYear));

            CreateMap<ReviewChangeStartYearViewModel, ChangeStartYearDetails>()
                .ForMember(d => d.StartYearFrom, opts => opts.MapFrom(s => s.AcademicYear))
                .ForMember(d => d.StartYearTo, opts => opts.MapFrom(s => s.AcademicYearTo));
        }

        private int? GetSelectedProviderId(AdminSearchLearnerCriteriaViewModel searchCriteria)
           => searchCriteria?.SearchLearnerFilters?.SelectedProviderId;

        private List<int> GetSelectedAcademicYearIds(AdminSearchLearnerCriteriaViewModel searchCriteria)
            => GetSelectedFilterIds(searchCriteria?.SearchLearnerFilters?.AcademicYears);

        private List<int> GetSelectedAwardingOrganisationIds(AdminSearchLearnerCriteriaViewModel searchCriteria)
            => GetSelectedFilterIds(searchCriteria?.SearchLearnerFilters?.AwardingOrganisations);

        private List<int> GetSelectedFilterIds(IList<FilterLookupData> filters)
        {
            if (filters.IsNullOrEmpty())
            {
                return new List<int>();
            }

            return filters.Where(p => p.IsSelected).Select(p => p.Id).ToList();
        }

        private int GetIndustryPlacementId(AdminLearnerRecord adminLearnerRecord)
            => GetIndustryPlacementProperty(adminLearnerRecord, ip => ip.Id, () => default);

        private IndustryPlacementStatus GetIndustryPlacementStatus(AdminLearnerRecord adminLearnerRecord)
            => GetIndustryPlacementProperty(adminLearnerRecord, ip => ip.Status, () => IndustryPlacementStatus.NotSpecified);

        private TProperty GetIndustryPlacementProperty<TProperty>(AdminLearnerRecord adminLearnerRecord, Func<IndustryPlacement, TProperty> getProperty, Func<TProperty> getDefault)
        {
            IEnumerable<IndustryPlacement> industryPlacements = adminLearnerRecord?.Pathway?.IndustryPlacements;

            if (industryPlacements?.Any() != true)
            {
                return getDefault();
            }

            IndustryPlacement industryPlacement = industryPlacements.First();
            return getProperty(industryPlacement);
        }

        private string GetDisplayAcademicYear(int academicYear)
            => string.Format(AdminSearchLearnerDetails.Start_Year_Value, academicYear, academicYear + 1);

        private List<int> GetChangeAcademicYearOptions(int academicYear, int tLevelStartYear)
        {
            var options = new List<int>();

            for (int i = academicYear - 1, j = 1; i >= tLevelStartYear && j <= 2; i--, j++)
            {
                options.Add(i);
            }

            return options;
        }
    }
}