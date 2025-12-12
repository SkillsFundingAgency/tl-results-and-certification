using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class OverallResultCalculationMapper : Profile
    {
        public OverallResultCalculationMapper()
        {
            CreateMap<OverallResult, DownloadOverallResultsData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.TqRegistrationPathway.AcademicYear))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => s.ResultAwarded))
                .ForMember(d => d.Details, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details)))
                .ForMember(d => d.SpecialismComponent, opts => opts.ConvertUsing(new SpecialismNameConverter(), s => s.TqRegistrationPathway.TqRegistrationSpecialisms.Where(w => w.EndDate == null)))
                .ForMember(d => d.SpecialismCode, opts => opts.ConvertUsing(new SpecialismCodeConverter(), s => s.TqRegistrationPathway.TqRegistrationSpecialisms.Where(w => w.EndDate == null)))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => s.SpecialismResultAwarded));

            CreateMap<OverallResult, DownloadOverallResultSlipsData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.TqRegistrationPathway.TqRegistrationProfile.Firstname} {s.TqRegistrationPathway.TqRegistrationProfile.Lastname}"))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TlProvider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.CoreAssessmentSeries, opts => opts.MapFrom(s => GetHighestResultCoreAssessmentSeries(s.TqRegistrationPathway.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null))))
                .ForMember(d => d.SpecialismAssessmentSeries, opts => opts.MapFrom(s => GetHighestResultSpecialismSeries(s.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault(r => r.IsOptedin && r.EndDate == null).TqSpecialismAssessments)))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => s.ResultAwarded))
                .ForMember(d => d.Details, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details)))
                .ForMember(d => d.SpecialismComponent, opts => opts.ConvertUsing(new SpecialismNameConverterNoDoubleQuotes(), s => s.TqRegistrationPathway.TqRegistrationSpecialisms.Where(w => w.EndDate == null)))
                .ForMember(d => d.SpecialismCode, opts => opts.ConvertUsing(new SpecialismCodeConverter(), s => s.TqRegistrationPathway.TqRegistrationSpecialisms.Where(w => w.EndDate == null)))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => s.SpecialismResultAwarded));
        }

        
        private string GetHighestResultCoreAssessmentSeries(IEnumerable<TqPathwayAssessment> assessments)
            => assessments.Where(a => a.IsOptedin)
                .SelectMany(e => e.TqPathwayResults
                    .Where(r => r.IsOptedin && r.EndDate == null))
                .OrderBy(r => r.TlLookupId)
                    .FirstOrDefault().TqPathwayAssessment.AssessmentSeries.Name;
        
        private string GetHighestResultSpecialismSeries(IEnumerable<TqSpecialismAssessment> specialismAssessments)
            => specialismAssessments.SelectMany(r => r.TqSpecialismResults)
                    .Where(r => r.IsOptedin && r.EndDate == null)
                .OrderBy(r => r.TlLookupId)
                    .FirstOrDefault().TqSpecialismAssessment.AssessmentSeries.Name;
    }
}