using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
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
                .ForMember(d => d.SpecialismComponent, opts => opts.MapFrom(s => GetSpecialismName(s)))
                .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => GetSpecialismCode(s)))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => s.SpecialismResultAwarded));
        }

        private static string GetSpecialismName(OverallResult overallResult)
        {
            string specialismName = overallResult.TqRegistrationPathway.TqRegistrationSpecialisms.Count switch
            {
                1 => GetSingleSpecialismName(overallResult),
                2 => GetDualSpecialismName(overallResult),
                _ => string.Empty
            };

            return $"\"{specialismName}\"";
        }

        private static string GetSpecialismCode(OverallResult overallResult)
        {
            return overallResult.TqRegistrationPathway.TqRegistrationSpecialisms.Count switch
            {
                1 => GetSingleSpecialismCode(overallResult),
                2 => GetDualSpecialismCode(overallResult),
                _ => string.Empty
            };
        }

        private static string GetSingleSpecialismName(OverallResult overallResult)
        {
            return GetSingleSpecialismProperty(overallResult, rs => rs.TlSpecialism.Name);
        }

        private static string GetDualSpecialismName(OverallResult overallResult)
        {
            return GetDualSpecialismProperty(overallResult, ds => ds.Name);
        }

        private static string GetSingleSpecialismCode(OverallResult overallResult)
        {
            return GetSingleSpecialismProperty(overallResult, rs => rs.TlSpecialism.LarId);
        }

        private static string GetDualSpecialismCode(OverallResult overallResult)
        {
            return GetDualSpecialismProperty(overallResult, ds => ds.LarId);
        }

        private static string GetSingleSpecialismProperty(OverallResult overallResult, Func<TqRegistrationSpecialism, string> getPropertyValue)
        {
            TqRegistrationSpecialism registrationSpecialism = overallResult.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault();
            return registrationSpecialism != null ? getPropertyValue(registrationSpecialism) : string.Empty;
        }

        private static string GetDualSpecialismProperty(OverallResult overallResult, Func<TlDualSpecialism, string> getPropertyValue)
        {
            IEnumerable<TlDualSpecialism> dualSpecialisms = overallResult.TqRegistrationPathway.TqRegistrationSpecialisms
                                                                        .Select(p => p.TlSpecialism)
                                                                        .SelectMany(s => s.TlDualSpecialismToSpecialisms)
                                                                        .Select(p => p.DualSpecialism);

            IGrouping<int, TlDualSpecialism> dualSpecialism = dualSpecialisms.GroupBy(p => p.Id).FirstOrDefault(p => p.Count() == 2);
            return dualSpecialism != null ? getPropertyValue(dualSpecialism.First()) : string.Empty;
        }
    }
}