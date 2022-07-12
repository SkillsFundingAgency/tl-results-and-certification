using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver
{
    public class DataExportMapper : Profile
    {
        public DataExportMapper()
        {
            // TODO: simply of JsonConvert required. 
            CreateMap<OverallResult, DownloadOverallResultsData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.TqRegistrationPathway.AcademicYear))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => s.ResultAwarded));
                //.ForMember(d => d.Tlevel, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).TlevelTitle))
                //.ForMember(d => d.CoreComponent, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).PathwayName))
                //.ForMember(d => d.CoreCode, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).PathwayLarId))
                //.ForMember(d => d.CoreResult, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).PathwayResult))
                //.ForMember(d => d.SpecialismComponent, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).SpecialismDetails[0].SpecialismName))//TODO
                //.ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).SpecialismDetails[0].SpecialismLarId))
                //.ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).SpecialismDetails[0].SpecialismResult))  
                //.ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details).IndustryPlacementStatus));
        }
    }
}
