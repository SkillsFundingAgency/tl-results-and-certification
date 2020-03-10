using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class TlevelMapper : Profile
    {
        public TlevelMapper()
        {
            CreateMap<AwardingOrganisationPathwayStatus, YourTlevelsViewModel>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"))
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.StatusId))
                .ForMember(d => d.PageTitle, opts => opts.MapFrom(s => "Your T Levels"));

            CreateMap<TlevelPathwayDetails, TLevelDetailsViewModel>()
               .ForMember(d => d.PageTitle, opts => opts.MapFrom(s => "T Level details"))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.ShowSomethingIsNotRight, opts => opts.MapFrom(s => s.PathwayStatusId == (int)TlevelReviewStatus.Confirmed))
               .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.RouteName))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms));

            CreateMap<TlevelPathwayDetails, VerifyTlevelViewModel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.RouteId, opts => opts.MapFrom(s => s.RouteId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => s.PathwayStatusId))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms))
               .ForMember(d => d.IsEverythingCorrect, opts => opts.Ignore());

            CreateMap<AwardingOrganisationPathwayStatus, TlevelToReviewViewModel>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"));

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, SelectToReviewPageViewModel>()
                .ForMember(d => d.TlevelsToReview, opts => opts.MapFrom(s => s.Where(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation)))
                .ForMember(d => d.ShowViewReviewedTlevelsLink, opts => opts.MapFrom(s => s.Any(a => a.StatusId == (int)TlevelReviewStatus.Confirmed)));

            CreateMap<VerifyTlevelViewModel, ConfirmTlevelDetails>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => (int)TlevelReviewStatus.Confirmed))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom<UserNameResolver<VerifyTlevelViewModel, ConfirmTlevelDetails>>());

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, TlevelConfirmationViewModel>()
               .ForMember(d => d.PathwayId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["pathwayId"]))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom((src, dest, destMember, context) => $"{src.FirstOrDefault(x => x.PathwayId == (int)context.Items["pathwayId"]).RouteName}: {src.FirstOrDefault(x => x.PathwayId == (int)context.Items["pathwayId"]).PathwayName}"))
               .ForMember(d => d.TlevelConfirmationText, opts => opts.MapFrom((src, dest, destMember, context) => string.Format(Confirmation.Section_Heading,  src.FirstOrDefault(x => x.PathwayId == (int)context.Items["pathwayId"]).StatusId == (int)TlevelReviewStatus.Confirmed ? Confirmation.Confirmed_Text : Confirmation.Queried_Text)))
               .ForMember(d => d.IsQueried, opts => opts.MapFrom(s => s.Any(x => x.StatusId == (int)TlevelReviewStatus.Queried)))
               .ForMember(d => d.ShowMoreTlevelsToReview, opts => opts.MapFrom(s => s.Any(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation)));

            CreateMap<TlevelPathwayDetails, TlevelQueryViewModel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => s.PathwayStatusId))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms));
        }
    }
}
