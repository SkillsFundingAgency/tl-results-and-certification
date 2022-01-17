using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class RegistrationMapper : Profile
    {
        public RegistrationMapper()
        {
            CreateMap<TqRegistrationPathway, RegistrationDetails>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.TqRegistrationProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.PathwayLarId, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms
                                    .Select(x => new SpecialismDetails { Id = x.Id, Code = x.TlSpecialism.LarId, Name = x.TlSpecialism.Name })))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.IsActiveWithOtherAo, opts => opts.MapFrom((src, dest, destMember, context) => (bool)context.Items["IsActiveWithOtherAo"]))
                .ForMember(d => d.HasActiveAssessmentEntriesForSpecialisms, opts => opts.MapFrom((src, dest, destMember, context) => (bool)context.Items["HasActiveAssessmentEntriesForSpecialisms"]));

            CreateMap<ManageRegistration, TqRegistrationProfile>()
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.FirstName))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.LastName))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateOfBirth))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(s => s.PerformedBy))
                .ForMember(d => d.ModifiedOn, opts => opts.MapFrom<DateTimeResolver<ManageRegistration, TqRegistrationProfile>>());

            CreateMap<TqProvider, TechnicalQualificationDetails>()
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.TlProvider.UkPrn))
                .ForMember(d => d.TlPathwayId, opts => opts.MapFrom(s => s.TqAwardingOrganisation.TlPathway.Id))
                .ForMember(d => d.PathwayLarId, opts => opts.MapFrom(s => s.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.TqProviderId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.TlAwardingOrganisatonId, opts => opts.MapFrom(s => s.TqAwardingOrganisation.TlAwardingOrganisatonId))
                .ForMember(d => d.TlSpecialismLarIds, opts => opts.MapFrom(s => s.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => new KeyValuePair<int, string>(s.Id, s.LarId))))
                .ForMember(d => d.TlSpecialismCombinations, opts => opts.MapFrom(s => s.TqAwardingOrganisation.TlPathway.TlPathwaySpecialismCombinations.GroupBy(g => g.GroupId).Select(c => new KeyValuePair<int, string>(c.Key, string.Join(Constants.PipeSeperator, c.Select(i => i.TlSpecialism.LarId))))));
        }
    }
}