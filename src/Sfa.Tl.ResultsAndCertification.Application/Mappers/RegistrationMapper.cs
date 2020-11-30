using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status));

            CreateMap<ManageRegistration, TqRegistrationProfile>()
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.FirstName))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.LastName))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateOfBirth))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(s => s.PerformedBy))
                .ForMember(d => d.ModifiedOn, opts => opts.MapFrom<DateTimeResolver<ManageRegistration, TqRegistrationProfile>>());

            CreateMap<AssessmentSeries, AvailableAssessmentSeries>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["profileId"]))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.Name));
        }
    }
}
