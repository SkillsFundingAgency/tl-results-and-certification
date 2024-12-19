using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminNotificationMapper : Profile
    {
        public AdminNotificationMapper()
        {
            CreateMap<AdminFindNotificationCriteriaViewModel, AdminSearchNotificationRequest>()
                .ForMember(d => d.SelectedActive, opts => opts.MapFrom(s => s.ActiveFilters.Where(f => f.IsSelected).Select(f => f.Id)))
                .ForMember(d => d.PageNumber, opts => opts.MapFrom(s => s.PageNumber));

            CreateMap<SearchNotificationDetail, AdminFindNotificationDetailsViewModel>()
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.End.ToDobFormat()))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target.GetDisplayName()))
                .ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsActive ? AdminFindNotification.Label_Yes : AdminFindNotification.Label_No))
                .ForMember(d => d.NotificationDetailsLink, opts => opts.MapFrom(s => new ChangeRecordModel
                {
                    Text = s.Title,
                    Route = RouteConstants.AdminNotificationDetails,
                    RouteAttributes = new Dictionary<string, string>() { { Constants.NotificationId, s.Id.ToString() } }
                }));

            CreateMap<PagedResponse<SearchNotificationDetail>, AdminFindNotificationViewModel>()
                .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
                .ForMember(d => d.Details, opts => opts.MapFrom(s => s.Records))
                .ForMember(d => d.PagerInfo, opts => opts.MapFrom(s => s.PagerInfo));

            CreateMap<GetNotificationResponse, AdminNotificationDetailsViewModel>()
                .ForMember(d => d.NotificationId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.SummaryTarget, opts => opts.MapFrom(s => CreateSummary("target", AdminNotificationDetails.Label_Target, s.Target.GetDisplayName())))
                .ForMember(d => d.SummaryStartDate, opts => opts.MapFrom(s => CreateSummary("startdate", AdminNotificationDetails.Label_Start_Date, s.Start.ToDobFormat())))
                .ForMember(d => d.SummaryEndDate, opts => opts.MapFrom(s => CreateSummary("enddate", AdminNotificationDetails.Label_End_Date, s.End.ToDobFormat())))
                .ForMember(d => d.SummaryIsActive, opts => opts.MapFrom(s => CreateSummary("active", AdminNotificationDetails.Label_Active, s.IsActive ? AdminNotificationDetails.Label_Yes : AdminNotificationDetails.Label_No)))
                .ForMember(d => d.DashboardBanner, opts => opts.MapFrom(s => new DashboardBannerModel(s.Content)));

            CreateMap<GetNotificationResponse, AdminEditNotificationViewModel>()
                .ForMember(d => d.NotificationId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.StartDay, opts => opts.MapFrom(s => s.Start.Day.ToString()))
                .ForMember(d => d.StartMonth, opts => opts.MapFrom(s => s.Start.Month.ToString()))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.Start.Year.ToString()))
                .ForMember(d => d.EndDay, opts => opts.MapFrom(s => s.End.Day.ToString()))
                .ForMember(d => d.EndMonth, opts => opts.MapFrom(s => s.End.Month.ToString()))
                .ForMember(d => d.EndYear, opts => opts.MapFrom(s => s.End.Year.ToString()));

            CreateMap<AdminEditNotificationViewModel, UpdateNotificationRequest>()
                .ForMember(d => d.NotificationId, opts => opts.MapFrom(s => s.NotificationId))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.Start, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.End, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom<UserNameResolver<AdminEditNotificationViewModel, UpdateNotificationRequest>>());

            CreateMap<AdminAddNotificationViewModel, AddNotificationRequest>()
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.Start, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.End, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminAddNotificationViewModel, AddNotificationRequest>>());
        }

        private static SummaryItemModel CreateSummary(string id, string title, string value)
           => new()
           {
               Id = id,
               Title = title,
               Value = value
           };
    }
}