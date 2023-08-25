using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class CoreRommExtractMapper : Profile
    {
        public CoreRommExtractMapper() 
        {
            CreateMap<TqRegistrationPathway, CoreRommExtractData>()
               .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
               .ForMember(d => d.StudentStartYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.AoName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name))
               .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
               .ForMember(d => d.CurrentCoreGrade, opts => opts.ConvertUsing(new PathwayResultStringConverter(), p => p.TqPathwayAssessments));

        }
    }
}