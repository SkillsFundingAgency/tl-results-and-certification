using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver.AdminAssessmentResult;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
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
                .ForMember(d => d.AssessmentDetails, opts => opts.MapFrom(s => s.Pathway))
                .ForMember(d => d.IsPendingWithdrawal, opts => opts.MapFrom(s => s.IsPendingWithdrawl))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => s.OverallResult));

            CreateMap<Pathway, AdminAssessmentDetailsViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items[Constants.RegistrationPathwayId]))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"))
                .ForMember(d => d.PathwayAssessments, opts => opts.MapFrom(s => s.PathwayAssessments))
                .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => s.Specialisms));

            CreateMap<Assessment, AdminAssessmentViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items[Constants.RegistrationPathwayId]))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Result != null ? s.Result.Grade : null))
                .ForMember(d => d.PrsDisplayText, opts => opts.MapFrom(s => GetPrsDisplayText(s)))
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom(s => (s.Result != null ? s.Result.LastUpdatedOn : s.LastUpdatedOn).ToDobFormat()))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedBy : s.LastUpdatedBy))
                .ForMember(d => d.IsResultChangeAllowed, opt => opt.MapFrom<IsChangeAllowedResolver>())
                .ForMember(d => d.ActionButton, opt => opt.MapFrom<TableButtonResolver>())
                .ForMember(d => d.AddResultRouteName, opt => opt.MapFrom(s =>
                    s.ComponentType == ComponentType.Core
                    ? RouteConstants.AdminAddPathwayResultClear
                    : s.ComponentType == ComponentType.Specialism
                        ? RouteConstants.AdminAddSpecialismResultClear
                        : string.Empty))
                .ForMember(d => d.ChangeResultRoute, opt => opt.MapFrom(s =>
                    s.ComponentType == ComponentType.Core
                    ? RouteConstants.AdminChangePathwayResultClear
                    : s.ComponentType == ComponentType.Specialism
                        ? RouteConstants.AdminChangeSpecialismResultClear
                        : string.Empty));

            CreateMap<Specialism, AdminSpecialismViewModel>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items[Constants.RegistrationPathwayId]))
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
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.TlevelStartYear, opts => opts.MapFrom(s => s.Pathway.StartYear))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.DisplayAcademicYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
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

            CreateMap<ReviewChangeStartYearViewModel, ReviewChangeRequest>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
               .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
               .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
               .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId));

            CreateMap<ReviewChangeStartYearViewModel, ReviewChangeStartYearRequest>()
               .ForMember(d => d.ChangeStartYearDetails, opts => opts.MapFrom(s => s))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<ReviewChangeStartYearViewModel, ReviewChangeStartYearRequest>>());

            CreateMap<ReviewChangeStartYearViewModel, ChangeStartYearDetails>()
               .ForMember(d => d.StartYearFrom, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.StartYearTo, opts => opts.MapFrom(s => s.AcademicYearTo));

            CreateMap<AdminReviewChangesIndustryPlacementViewModel, ReviewChangeIndustryPlacementRequest>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.AdminChangeIpViewModel.AdminIpCompletion.RegistrationPathwayId))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewChangesIndustryPlacementViewModel, ReviewChangeIndustryPlacementRequest>>())
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.AdminChangeIpViewModel.AdminIpCompletion.IndustryPlacementStatusTo))
                .ForMember(d => d.HoursSpentOnPlacement, opts => opts.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.AdminChangeIpViewModel.HoursViewModel.Hours)
                        ? null as int?
                        : int.Parse(s.AdminChangeIpViewModel.HoursViewModel.Hours)))
                .ForMember(d => d.SpecialConsiderationReasons, opts => opts.MapFrom(s => s.SelectedReasons));

            CreateMap<AdminLearnerRecord, AdminCoreComponentViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.DisplayStartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.ValidPathwayAssessmentSeries, opts => opts.MapFrom((src, dest, destMember, context) => context.Items[Constants.AdminValidAssessmentSeries]))
                .ForMember(d => d.HasCoreAssessmentEntries, opts => opts.MapFrom(s => s.Pathway.PathwayAssessments.Any()))
                .ForMember(d => d.AssessmentDetails, opts => opts.MapFrom(s => s.Pathway));

            CreateMap<AdminLearnerRecord, AdminOccupationalSpecialismViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.TlevelName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.Pathway.AcademicYear))
                .ForMember(d => d.DisplayStartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items[Constants.AdminSpecialismAssessmentId]))
                .ForMember(d => d.ValidPathwayAssessmentSeries, opts => opts.MapFrom((src, dest, destMember, context) => context.Items[Constants.AdminValidAssessmentSeries]))
                .ForMember(d => d.AssessmentDetails, opts => opts.MapFrom(s => s.Pathway));

            CreateMap<AdminLearnerRecord, AdminRemovePathwayAssessmentEntryViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.LastUpdatedOn.ToDobFormat())))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.LastUpdatedBy)))
                .ForMember(d => d.CanAssessmentEntryBeRemoved, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result == null)));

            CreateMap<AdminLearnerRecord, AdminRemoveSpecialismAssessmentEntryViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                    return $"{specialism?.Name} ({specialism.LarId})";
                }))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.LastUpdatedOn.ToDobFormat())))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.LastUpdatedBy)))
                .ForMember(d => d.CanAssessmentEntryBeRemoved, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result == null)));

            CreateMap<AdminReviewChangesCoreAssessmentViewModel, ReviewChangeRequest>()
                   .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                   .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                   .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
                   .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId));

            CreateMap<AdminReviewChangesCoreAssessmentViewModel, ReviewAddCoreAssessmentRequest>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.AdminCoreComponentViewModel.RegistrationPathwayId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewChangesCoreAssessmentViewModel, ReviewAddCoreAssessmentRequest>>())
                .ForMember(d => d.AddCoreAssessmentDetails, opts => opts.MapFrom(s => s));

            CreateMap<AdminReviewChangesCoreAssessmentViewModel, AddCoreAssessmentDetails>()
               .ForMember(d => d.CoreAssessmentFrom, opts => opts.MapFrom(s => $"{ReviewChangeAssessment.No_Assessment_Recorded} {s.AdminCoreComponentViewModel.AssessmentYearTo.ToLower()}"))
               .ForMember(d => d.CoreAssessmentTo, opts => opts.MapFrom(s => s.AdminCoreComponentViewModel.AssessmentYearTo));


            CreateMap<AdminReviewChangesSpecialismAssessmentViewModel, ReviewChangeRequest>()
                  .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                  .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                  .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
                  .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId));

            CreateMap<AdminReviewChangesSpecialismAssessmentViewModel, ReviewAddSpecialismAssessmentRequest>()
           .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.AdminOccupationalSpecialismViewModel.RegistrationPathwayId))
           .ForMember(d => d.SpecialismId, opts => opts.MapFrom(s => s.AdminOccupationalSpecialismViewModel.SpecialismAssessmentId))
           .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewChangesSpecialismAssessmentViewModel, ReviewAddSpecialismAssessmentRequest>>())
           .ForMember(d => d.AddSpecialismDetails, opts => opts.MapFrom(s => s));

            CreateMap<AdminReviewChangesSpecialismAssessmentViewModel, AddSpecialismDetails>()
              .ForMember(d => d.SpecialismAssessmentFrom, opts => opts.MapFrom(s => $"{ReviewChangeAssessment.No_Assessment_Recorded} {s.AdminOccupationalSpecialismViewModel.AssessmentYearTo.ToLower()}"))
              .ForMember(d => d.SpecialismAssessmentTo, opts => opts.MapFrom(s => s.AdminOccupationalSpecialismViewModel.AssessmentYearTo));

            #region Add pathway result

            CreateMap<AdminLearnerRecord, AdminAddPathwayResultViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<AdminAddPathwayResultViewModel, AdminAddPathwayResultReviewChangesViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId.HasValue ? s.SelectedGradeId.Value : default))
                .ForMember(d => d.SelectedGradeValue, opts => opts.MapFrom(s => s.SelectedGradeValue))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminAddPathwayResultReviewChangesViewModel, AddPathwayResultRequest>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
                .ForMember(d => d.GradeTo, opts => opts.MapFrom(s => s.SelectedGradeValue))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskTicketId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminAddPathwayResultReviewChangesViewModel, AddPathwayResultRequest>>());

            #endregion

            #region Add specialism result

            CreateMap<AdminLearnerRecord, AdminAddSpecialismResultViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                    return $"{specialism?.Name} ({specialism.LarId})";

                })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<AdminAddSpecialismResultViewModel, AdminAddSpecialismResultReviewChangesViewModel>()
                    .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                    .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.SpecialismAssessmentId))
                    .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId.HasValue ? s.SelectedGradeId.Value : default))
                    .ForMember(d => d.SelectedGradeValue, opts => opts.MapFrom(s => s.SelectedGradeValue))
                    .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                    .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                    .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
                    .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
                    .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
                    .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminAddSpecialismResultReviewChangesViewModel, AddSpecialismResultRequest>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.SpecialismAssessmentId))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
                .ForMember(d => d.GradeTo, opts => opts.MapFrom(s => s.SelectedGradeValue))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskTicketId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminAddSpecialismResultReviewChangesViewModel, AddSpecialismResultRequest>>());

            #endregion

            CreateMap<AdminReviewRemoveCoreAssessmentEntryViewModel, ReviewRemoveCoreAssessmentEntryRequest>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.RegistrationPathwayId))
               .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.PathwayAssessmentId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForPath(d => d.ChangeAssessmentDetails.PathwayName, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.PathwayName))
               .ForPath(d => d.ChangeAssessmentDetails.From, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.ExamPeriod))
               .ForPath(d => d.ChangeAssessmentDetails.To, opts => opts.MapFrom(s => string.Format(AdminReviewRemoveAssessmentEntry.Label_No_Assessment_Entry_Recorded, s.PathwayAssessmentViewModel.ExamPeriod)))
               .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
               .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
               .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
               .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewRemoveCoreAssessmentEntryViewModel, ReviewRemoveCoreAssessmentEntryRequest>>());

            CreateMap<AdminReviewRemoveSpecialismAssessmentEntryViewModel, ReviewRemoveSpecialismAssessmentEntryRequest>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.RegistrationPathwayId))
               .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.SpecialismAssessmentId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Specialism))
               .ForPath(d => d.ChangeSpecialismAssessmentDetails.SpecialismName, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.SpecialismName))
               .ForPath(d => d.ChangeSpecialismAssessmentDetails.From, opts => opts.MapFrom(s => s.PathwayAssessmentViewModel.ExamPeriod))
               .ForPath(d => d.ChangeSpecialismAssessmentDetails.To, opts => opts.MapFrom(s => string.Format(AdminReviewRemoveAssessmentEntry.Label_No_Assessment_Entry_Recorded, s.PathwayAssessmentViewModel.ExamPeriod)))
               .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
               .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => s.RequestDate))
               .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
               .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskId))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewRemoveSpecialismAssessmentEntryViewModel, ReviewRemoveSpecialismAssessmentEntryRequest>>());


            CreateMap<AdminLearnerRecord, AdminChangePathwayResultViewModel>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
               .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
               .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
               .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
               .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
               .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
               .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
               .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<AdminLearnerRecord, AdminChangeSpecialismResultViewModel>()
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
               .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
               {
                   int assessmentId = (int)context.Items[Constants.AssessmentId];
                   Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                   return $"{specialism?.Name} ({specialism.LarId})";

               })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
               .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
               .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
               .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
               .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
               .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
               .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));


            #region change pathway result


            CreateMap<AdminChangePathwayResultViewModel, AdminChangePathwayResultReviewChangesViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId.HasValue ? s.SelectedGradeId.Value : default))
                .ForMember(d => d.SelectedGradeValue, opts => opts.MapFrom(s => s.SelectedGradeValue))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminChangePathwayResultReviewChangesViewModel, ChangePathwayResultRequest>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.ChangePathwayDetails, opts => opts.MapFrom(s => s))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskTicketId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminChangePathwayResultReviewChangesViewModel, ChangePathwayResultRequest>>());

            CreateMap<AdminChangePathwayResultReviewChangesViewModel, ChangePathwayDetails>()
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.SelectedGradeFrom, opts => opts.MapFrom(s => s.Grade))
                .ForMember(d => d.SelectedGradeTo, opts => opts.MapFrom(s => s.SelectedGradeValue));



            CreateMap<AdminChangeSpecialismResultViewModel, AdminChangeSpecialismResultReviewChangesViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.SpecialismAssessmentId))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId.HasValue ? s.SelectedGradeId.Value : default))
                .ForMember(d => d.SelectedGradeValue, opts => opts.MapFrom(s => s.SelectedGradeValue))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminChangeSpecialismResultReviewChangesViewModel, ChangeSpecialismResultRequest>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.ChangeSpecialismDetails, opts => opts.MapFrom(s => s))
                .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.RequestDate, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskId, opts => opts.MapFrom(s => s.ZendeskTicketId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminChangeSpecialismResultReviewChangesViewModel, ChangeSpecialismResultRequest>>());

            CreateMap<AdminChangeSpecialismResultReviewChangesViewModel, ChangeSpecialismDetails>()
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.SpecialismAssessmentId))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
                .ForMember(d => d.SelectedGradeFrom, opts => opts.MapFrom(s => s.Grade))
                .ForMember(d => d.SelectedGradeTo, opts => opts.MapFrom(s => s.SelectedGradeValue));


            CreateMap<AdminLearnerRecord, AdminAddRommOutcomeChangeGradeCoreViewModel>()
              .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
              .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
              .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
              .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
              .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
              .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
              .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
              .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
              .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
              .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
              .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
              .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));



            CreateMap<AdminLearnerRecord, AdminAddRommOutcomeChangeGradeSpecialismViewModel>()
              .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
              .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
              .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
              {
                  int assessmentId = (int)context.Items[Constants.AssessmentId];
                  Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                  return $"{specialism?.Name} ({specialism.LarId})";

              })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
              .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
              .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
              .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
              .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
              .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
              .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
              .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
              .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<AdminLearnerRecord, AdminAddAppealOutcomeChangeGradeCoreViewModel>()
              .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
              .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
              .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
              .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
              .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
              .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
              .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
              .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
              .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
              .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
              .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
              .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));


            CreateMap<AdminLearnerRecord, AdminAddAppealOutcomeChangeGradeSpecialismViewModel>()
              .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
              .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
              .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
              {
                  int assessmentId = (int)context.Items[Constants.AssessmentId];
                  Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                  return $"{specialism?.Name} ({specialism.LarId})";

              })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
              .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
              .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
              .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
              .ForMember(d => d.StartYear, opts => opts.MapFrom(s => GetDisplayAcademicYear(s.Pathway.AcademicYear)))
              .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
              .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
              .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
              .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            #endregion

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

        private string GetPrsDisplayText(Assessment assessment)
        {
            if (string.IsNullOrWhiteSpace(assessment?.Result?.GradeCode))
            {
                return string.Empty;
            }

            bool canChangeResult = CommonHelper.IsValidGradeForChangeResult(assessment?.Result?.GradeCode, assessment.ComponentType);

            if (canChangeResult)
            {
                return string.Empty;
            }

            return CommonHelper.GetPrsStatusDisplayText(assessment?.Result?.PrsStatus, assessment.RommEndDate, assessment.AppealEndDate);
        }

        private T GetPathwayAssessmentPropertyValue<T>(AdminLearnerRecord learnerRecord, int assessmentId, Func<Assessment, T> getPropertyValue)
        {
            var pathwayAssessment = learnerRecord?.Pathway?.PathwayAssessments?.SingleOrDefault(p => p.Id == assessmentId);
            return pathwayAssessment == null ? default : getPropertyValue(pathwayAssessment);
        }

        private T GetSpecialismAssessmentPropertyValue<T>(AdminLearnerRecord learnerRecord, int assessmentId, Func<Assessment, T> getPropertyValue)
        {
            Assessment specialismAssessment = learnerRecord?.Pathway?.Specialisms?.SelectMany(s => s.Assessments).SingleOrDefault(a => a.Id == assessmentId);
            return specialismAssessment == null ? default : getPropertyValue(specialismAssessment);
        }



    }
}