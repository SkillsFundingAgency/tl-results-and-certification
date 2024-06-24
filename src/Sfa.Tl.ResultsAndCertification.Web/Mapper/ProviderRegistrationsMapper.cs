using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Content.ProviderRegistrations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ProviderRegistrationsMapper : Profile
    {
        public ProviderRegistrationsMapper()
        {
            CreateMap<int, AvailableStartYearViewModel>()
               .ForMember(d => d.Year, opts => opts.MapFrom(s => s))
               .ForMember(d => d.DisplayYear, opts => opts.MapFrom(s => GetDisplayYear(s)));

            CreateMap<DataExportResponse, DownloadRegistrationsDataForViewModel>()
                .ForMember(d => d.PageTitle, opts => opts.MapFrom((src, dest, destMember, context) => string.Format(DownloadRegistrationsDataFor.Page_Title, GetDisplayYear(context))))
                .ForMember(d => d.PageHeader, opts => opts.MapFrom((src, dest, destMember, context) => string.Format(DownloadRegistrationsDataFor.Download_Registrations_Data_For_Heading, GetDisplayYear(context))))
                .ForMember(d => d.DownloadLinkText, opts => opts.MapFrom((src, dest, destMember, context) => string.Format(DownloadRegistrationsDataFor.Download_Link_Registrations_Data, GetDisplayYear(context))))
                .ForMember(d => d.DownloadLink, opts => opts.MapFrom(s => CreateDownloadLink(s)))
                .ForMember(d => d.Breadcrumb, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    string displayYear = GetDisplayYear(context);
                    return CreateBreadCrumbs(displayYear);
                }));
        }

        private static string GetDisplayYear(ResolutionContext context)
        {
            int year = (int)context.Items["start-year"];
            return GetDisplayYear(year);
        }

        private static string GetDisplayYear(int year)
            => $"{year} to {year + 1}";

        private static DownloadLinkViewModel CreateDownloadLink(DataExportResponse response)
            => new()
            {
                BlobUniqueReference = response.BlobUniqueReference,
                FileSize = response.FileSize,
                FileType = FileType.Csv.ToString().ToUpperInvariant()
            };

        public BreadcrumbModel CreateBreadCrumbs(string displayYear)
            => new()
            {
                BreadcrumbItems = new List<BreadcrumbItem>
                {
                    new()
                    {
                        DisplayName = BreadcrumbContent.Home,
                        RouteName = RouteConstants.Home
                    },
                    new()
                    {
                        DisplayName = BreadcrumbContent.Download_Registrations_Data,
                        RouteName = RouteConstants.DownloadRegistrationsData
                    },
                    new()
                    {
                        DisplayName = string.Format(BreadcrumbContent.Download_Registrations_Data_For, displayYear)
                    }
                }
            };
    }
}