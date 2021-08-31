using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class TlevelMapper : Profile
    {
        public TlevelMapper()
        {
            CreateMap<AwardingOrganisationPathwayStatus, YourTlevelViewModel>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, YourTlevelsViewModel>()
                .ForMember(d => d.IsAnyReviewPending, opts => opts.MapFrom(s => s.Any(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation)))
                .ForMember(d => d.ConfirmedTlevels, opts => opts.MapFrom(s => s.Where(x => x.StatusId == (int)TlevelReviewStatus.Confirmed)))
                .ForMember(d => d.QueriedTlevels, opts => opts.MapFrom(s => s.Where(x => x.StatusId == (int)TlevelReviewStatus.Queried)));

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, ConfirmedTlevelsViewModel>()
                .ForMember(d => d.Tlevels, opts => opts.MapFrom(s => s));

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, QueriedTlevelsViewModel>()
                .ForMember(d => d.Tlevels, opts => opts.MapFrom(s => s));

            CreateMap<TlevelPathwayDetails, TLevelConfirmedDetailsViewModel>()
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.IsValid, opts => opts.MapFrom(s => s.PathwayStatusId == (int)TlevelReviewStatus.Confirmed))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})")));

            CreateMap<TlevelPathwayDetails, TlevelQueriedDetailsViewModel>()
               .ForMember(d => d.IsValid, opts => opts.MapFrom(s => s.PathwayStatusId == (int)TlevelReviewStatus.Queried))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})")))
               .ForMember(d => d.QueriedBy, opts => opts.MapFrom(s => s.VerifiedBy))
               .ForMember(d => d.QueriedOn, opts => opts.MapFrom(s => s.VerifiedOn.HasValue ? s.VerifiedOn.Value.ToDobFormat() : string.Empty));

            CreateMap<TlevelPathwayDetails, ConfirmTlevelViewModel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.RouteId, opts => opts.MapFrom(s => s.RouteId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => s.PathwayStatusId))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})")))
               .ForMember(d => d.IsEverythingCorrect, opts => opts.Ignore());

            CreateMap<AwardingOrganisationPathwayStatus, TlevelToReviewViewModel>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, SelectToReviewPageViewModel>()
                .ForMember(d => d.TlevelsToReview, opts => opts.MapFrom(s => s.Where(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation)));

            CreateMap<ConfirmTlevelViewModel, VerifyTlevelDetails>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => (int)TlevelReviewStatus.Confirmed))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom<UserNameResolver<ConfirmTlevelViewModel, VerifyTlevelDetails>>());

            CreateMap<IEnumerable<AwardingOrganisationPathwayStatus>, TlevelConfirmationViewModel>()
               .ForMember(d => d.PathwayId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["pathwayId"]))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom((src, dest, destMember, context) => src.FirstOrDefault(x => x.PathwayId == (int)context.Items["pathwayId"]).TlevelTitle))
               .ForMember(d => d.TlevelConfirmationText, opts => opts.MapFrom((src, dest, destMember, context) => string.Format(Confirmation.Section_Heading, src.FirstOrDefault(x => x.PathwayId == (int)context.Items["pathwayId"]).StatusId == (int)TlevelReviewStatus.Confirmed ? Confirmation.Confirmed_Text : Confirmation.Queried_Text)))
               .ForMember(d => d.IsQueried, opts => opts.MapFrom((src, dest, destMember, context) => src.FirstOrDefault(x => x.PathwayId == (int)context.Items["pathwayId"]).StatusId == (int)TlevelReviewStatus.Queried))
               .ForMember(d => d.ShowMoreTlevelsToReview, opts => opts.MapFrom(s => s.Any(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation)));

            CreateMap<TlevelPathwayDetails, TlevelQueryViewModel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.RouteId, opts => opts.MapFrom(s => s.RouteId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => s.PathwayStatusId))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})")))
               .ForMember(d => d.Query, opts => opts.Ignore());

            CreateMap<TlevelQueryViewModel, VerifyTlevelDetails>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => (int)TlevelReviewStatus.Queried))
                .ForMember(d => d.Query, opts => opts.MapFrom(s => s.Query.Trim()))
                .ForMember(d => d.QueriedUserEmail, opts => opts.MapFrom<UserEmailResolver<TlevelQueryViewModel, VerifyTlevelDetails>>())
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom<UserNameResolver<TlevelQueryViewModel, VerifyTlevelDetails>>());

            CreateMap<PathwaySpecialisms, PathwaySpecialismsViewModel>()
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.PathwayCode))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms));

            CreateMap<SpecialismDetails, SpecialismDetailsViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.Code, opts => opts.MapFrom(s => s.Code))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.Code})"));
        }
    }
}
