using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminDashboardMapper : Profile
    {
        public AdminDashboardMapper()
        {
            CreateMap<TqRegistrationPathway, AdminLearnerRecord>()
                .ForMember(p => p.ProfileId, opts => opts.MapFrom(p => p.TqRegistrationProfile.Id))
                .ForMember(p => p.Uln, opts => opts.MapFrom(p => p.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(p => p.FirstName, opts => opts.MapFrom(p => p.TqRegistrationProfile.Firstname))
                .ForMember(p => p.LastName, opts => opts.MapFrom(p => p.TqRegistrationProfile.Lastname))
                .ForMember(p => p.Provider, opts => opts.MapFrom(p => p.TqProvider.TlProvider.Name))
                .ForMember(p => p.Ukprn, opts => opts.MapFrom(p => p.TqProvider.TlProvider.UkPrn))
                .ForMember(p => p.AcademicYear, opts => opts.MapFrom(p => p.AcademicYear))
                .ForMember(p => p.DisplayAcademicYear, opts => opts.ConvertUsing(new AcademicYearConverter(), p => p.AcademicYear))
                .ForMember(p => p.TLevel, opts => opts.MapFrom(p => p.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForMember(p => p.TLevelStartYear, opts => opts.MapFrom(p => p.TqProvider.TqAwardingOrganisation.TlPathway.StartYear));
        }
    }
}
