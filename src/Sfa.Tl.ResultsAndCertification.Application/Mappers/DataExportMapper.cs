using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class DataExportMapper : Profile
    {
        public DataExportMapper()
        {
            CreateMap<OverallResult, DownloadOverallResultsData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.TqRegistrationPathway.AcademicYear))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => s.ResultAwarded))
                .ForMember(d => d.Details, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details)));
        }
    }
}
