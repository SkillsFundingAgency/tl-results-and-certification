﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AssessmentMapper : Profile
    {
        public AssessmentMapper()
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

            CreateMap<TqRegistrationPathway, AssessmentDetails>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.TqRegistrationProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))

                .ForMember(d => d.PathwayLarId, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.TqPathwayAssessments.Any() ? s.TqPathwayAssessments.FirstOrDefault().AssessmentSeries.Name : null))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.TqPathwayAssessments.Any() ? s.TqPathwayAssessments.FirstOrDefault().Id : (int?)null))

                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TlSpecialism.LarId : null))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TlSpecialism.Name : null))
                .ForMember(d => d.SpecialismAssessmentSeries, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.FirstOrDefault().AssessmentSeries.Name : null : null))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.FirstOrDefault().Id : (int?)null : null))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status));
        }
    }
}
