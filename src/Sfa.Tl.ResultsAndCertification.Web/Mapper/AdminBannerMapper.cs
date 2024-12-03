using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminBannerMapper : Profile
    {
        public AdminBannerMapper()
        {
            CreateMap<AdminFindBannerCriteriaViewModel, AdminSearchBannerRequest>()
                .ForMember(d => d.SelectedTargets, opts => opts.MapFrom(s => s.SelectedTargets))
                .ForMember(d => d.SelectActiveBanners, opts => opts.MapFrom(s => s.SelectActiveBanners))
                .ForMember(d => d.PageNumber, opts => opts.MapFrom(s => s.PageNumber));

            CreateMap<SearchBannerDetail, AdminFindBannerDetailsViewModel>()
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content.Substring(0, Math.Min(50, s.Content.Length))))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target.GetDisplayName()))
                .ForMember(d => d.Active, opts => opts.MapFrom(s => s.IsOptedin))
                .ForMember(d => d.BannerDetailsLink, opts => opts.MapFrom(s => new ChangeRecordModel
                {
                    Text = s.Title,
                    Route = RouteConstants.AdminBannerDetails,
                    RouteAttributes = new Dictionary<string, string>() { { Constants.BannerId, s.Id.ToString() } }
                }));

            CreateMap<PagedResponse<SearchBannerDetail>, AdminFindBannerViewModel>()
                .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
                .ForMember(d => d.Details, opts => opts.MapFrom(s => s.Records))
                .ForMember(d => d.PagerInfo, opts => opts.MapFrom(s => s.PagerInfo));

            CreateMap<GetBannerResponse, AdminBannerDetailsViewModel>()
                .ForMember(d => d.BannerId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.SummaryTarget, opts => opts.MapFrom(s => CreateSummary("target", AdminBannerDetails.Label_Target, s.Target.GetDisplayName())))
                .ForMember(d => d.SummaryIsActive, opts => opts.MapFrom(s => CreateSummary("active", AdminBannerDetails.Label_Active, s.IsOptedin ? AdminBannerDetails.Label_Yes : AdminBannerDetails.Label_No)))
                .ForMember(d => d.SummaryStartDate, opts => opts.MapFrom(s => CreateSummary("startdate", AdminBannerDetails.Label_Start_Date, s.Start.ToDobFormat())))
                .ForMember(d => d.SummaryEndDate, opts => opts.MapFrom(s => CreateSummary("enddate", AdminBannerDetails.Label_End_Date, s.End.ToDobFormat())));

            CreateMap<GetBannerResponse, AdminEditBannerViewModel>()
                .ForMember(d => d.BannerId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsOptedin))
                .ForMember(d => d.StartDay, opts => opts.MapFrom(s => s.Start.Day.ToString()))
                .ForMember(d => d.StartMonth, opts => opts.MapFrom(s => s.Start.Month.ToString()))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.Start.Year.ToString()))
                .ForMember(d => d.EndDay, opts => opts.MapFrom(s => s.End.Day.ToString()))
                .ForMember(d => d.EndMonth, opts => opts.MapFrom(s => s.End.Month.ToString()))
                .ForMember(d => d.EndYear, opts => opts.MapFrom(s => s.End.Year.ToString()));

            CreateMap<AdminEditBannerViewModel, UpdateBannerRequest>()
                .ForMember(d => d.BannerId, opts => opts.MapFrom(s => s.BannerId))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsActive))
                .ForMember(d => d.Start, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.End, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom<UserNameResolver<AdminEditBannerViewModel, UpdateBannerRequest>>());

            CreateMap<AdminAddBannerViewModel, AddBannerRequest>()
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.Start, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.End, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminAddBannerViewModel, AddBannerRequest>>());
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