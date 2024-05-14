using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class SearchRegistrationMapper : Profile
    {
        public SearchRegistrationMapper()
        {
            CreateMap<SearchRegistrationFilters, SearchRegistrationFiltersViewModel>()
                .ForMember(d => d.AcademicYears, opts => opts.MapFrom(s => s.AcademicYears));

            CreateMap<PagedResponse<SearchRegistrationDetail>, SearchRegistrationDetailsListViewModel>()
                .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
                .ForMember(d => d.RegistrationDetails, opts => opts.MapFrom(s => s.Records))
                .ForMember(d => d.PagerInfo, opts => opts.MapFrom(s => s.PagerInfo));

            CreateMap<SearchRegistrationDetail, SearchRegistrationDetailsViewModel>()
                .ForMember(d => d.RegistrationProfileId, opts => opts.MapFrom(s => s.RegistrationProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.ProviderName} ({s.ProviderUkprn})"))
                .ForMember(d => d.Core, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayLarId})"))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.AcademicYear} to {s.AcademicYear + 1}"))
                .ForMember(d => d.Route, opts => opts.MapFrom((src, dest, destMember, context) => GetRoute((SearchRegistrationType)context.Items["search-type"], src)));

            CreateMap<SearchRegistrationCriteriaViewModel, SearchRegistrationRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.SearchKey, opts => opts.MapFrom(s => s.SearchKey))
                .ForMember(d => d.PageNumber, opts => opts.MapFrom(s => s.PageNumber))
                .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => GetSelectedProviderId(s)))
                .ForMember(d => d.SelectedAcademicYears, opts => opts.MapFrom(s => GetSelectedAcademicYearIds(s)));
        }

        #region Filters

        private static int? GetSelectedProviderId(SearchRegistrationCriteriaViewModel searchCriteria)
          => searchCriteria?.Filters?.SelectedProviderId;

        private static IList<int> GetSelectedAcademicYearIds(SearchRegistrationCriteriaViewModel searchCriteria)
            => GetSelectedFilterIds(searchCriteria?.Filters?.AcademicYears);

        private static IList<int> GetSelectedFilterIds(IList<FilterLookupData> filters)
            => filters.IsNullOrEmpty()
                ? new List<int>()
                : filters.Where(p => p.IsSelected).Select(p => p.Id).ToList();

        #endregion

        #region Routes

        private static RouteModel GetRoute(SearchRegistrationType type, SearchRegistrationDetail searchRegistrationDetail)
            => type switch
            {
                SearchRegistrationType.Assessment => GetAssessmentRoute(searchRegistrationDetail.RegistrationProfileId, searchRegistrationDetail.IsWithdrawn),
                SearchRegistrationType.Result => GetResultRoute(searchRegistrationDetail.RegistrationProfileId, searchRegistrationDetail.IsWithdrawn),
                SearchRegistrationType.PostResult => GetPostResultRoute(searchRegistrationDetail.RegistrationProfileId, searchRegistrationDetail.IsWithdrawn, searchRegistrationDetail.HasResults),
                _ => GetRegistrationRoute(searchRegistrationDetail.RegistrationProfileId)
            };

        private static RouteModel GetRegistrationRoute(int registrationProfileId)
            => GetRoute(registrationProfileId, () => RouteConstants.RegistrationDetails);

        private static RouteModel GetAssessmentRoute(int registrationProfileId, bool isWithdrawn)
            => GetRoute(registrationProfileId, () => isWithdrawn ? RouteConstants.AssessmentWithdrawnDetails : RouteConstants.AssessmentDetails);

        private static RouteModel GetResultRoute(int registrationProfileId, bool isWithdrawn)
            => GetRoute(registrationProfileId, () => isWithdrawn ? RouteConstants.ResultWithdrawnDetails : RouteConstants.ResultDetails);

        private static RouteModel GetPostResultRoute(int registrationProfileId, bool isWithdrawn, bool hasResult)
            => GetRoute(registrationProfileId, () =>
            {
                if (isWithdrawn)
                    return RouteConstants.PrsUlnWithdrawn;

                if (!hasResult)
                    return RouteConstants.PrsNoResults;

                return RouteConstants.PrsLearnerDetails;
            });

        private static RouteModel GetRoute(int registrationProfileId, Func<string> getRoute)
            => new(getRoute(), (Constants.ProfileId, registrationProfileId.ToString()));

        #endregion
    }
}
