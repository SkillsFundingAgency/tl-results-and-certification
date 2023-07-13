using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystResultsExtraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AnalystResultExtractionMapper : Profile
    {
        public AnalystResultExtractionMapper()
        {
            CreateMap<TqRegistrationPathway, AnalystOverallResultExtractionData>()
                .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.Gender, opts => opts.MapFrom(s => s.TqRegistrationProfile.Gender))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.StartYear))
                .ForMember(d => d.CoreComponent, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.CoreResult, opts => opts.MapFrom(s => GetHighestPathwayResult(s)))
                .ForMember(d => d.OccupationalSpecialism, opts => opts.MapFrom(s => GetSpecialismName(s)))
                .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => GetSpecialismCode(s)))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => GetSpecialismResult(s)))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => GetIndustryPlacementStatus(s)))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => GetOverallResult(s)));
        }

        private IndustryPlacementStatus GetIndustryPlacementStatus(TqRegistrationPathway registrationPathway)
        {
            return registrationPathway.IndustryPlacements.Any() ? registrationPathway.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified;
        }

        private string GetSpecialismResult(TqRegistrationPathway registrationPathway)
        {
            return GetOverallResultProperty(registrationPathway, p => p.SpecialismResultAwarded);
        }

        private string GetOverallResult(TqRegistrationPathway registrationPathway)
        {
            return GetOverallResultProperty(registrationPathway, p => p.ResultAwarded);
        }

        private string GetOverallResultProperty(TqRegistrationPathway registrationPathway, Func<OverallResult, string> getPropertyValue)
        {
            if (registrationPathway.OverallResults.Count == 0)
                return string.Empty;

            OverallResult overallResult = registrationPathway.OverallResults.OrderByDescending(r => r.Id).First();

            return overallResult != null ? getPropertyValue(overallResult) : string.Empty;
        }

        private static string GetHighestPathwayResult(TqRegistrationPathway learnerPathway)
        {
            if (!learnerPathway.TqPathwayAssessments.Any())
                return null;

            var pathwayResults = learnerPathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).ToList();

            var qPendingGrade = pathwayResults.FirstOrDefault(x => x.TlLookup.Code.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));
            var pathwayHigherResult = qPendingGrade ?? pathwayResults.OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return pathwayHigherResult.TlLookup?.Value;
        }

        private static string GetSpecialismName(TqRegistrationPathway registrationPathway)
        {
            string specialismName = registrationPathway.TqRegistrationSpecialisms.Count switch
            {
                1 => GetSingleSpecialismName(registrationPathway),
                2 => GetDualSpecialismName(registrationPathway),
                _ => string.Empty
            };

            return $"\"{specialismName}\"";
        }

        private static string GetSpecialismCode(TqRegistrationPathway registrationPathway)
        {
            return registrationPathway.TqRegistrationSpecialisms.Count switch
            {
                1 => GetSingleSpecialismCode(registrationPathway),
                2 => GetDualSpecialismCode(registrationPathway),
                _ => string.Empty
            };
        }

        private static string GetSingleSpecialismName(TqRegistrationPathway registrationPathway)
        {
            return GetSingleSpecialismProperty(registrationPathway, rs => rs.TlSpecialism.Name);
        }

        private static string GetDualSpecialismName(TqRegistrationPathway registrationPathway)
        {
            return GetDualSpecialismProperty(registrationPathway, ds => ds.Name);
        }

        private static string GetSingleSpecialismCode(TqRegistrationPathway registrationPathway)
        {
            return GetSingleSpecialismProperty(registrationPathway, rs => rs.TlSpecialism.LarId);
        }

        private static string GetDualSpecialismCode(TqRegistrationPathway registrationPathway)
        {
            return GetDualSpecialismProperty(registrationPathway, ds => ds.LarId);
        }

        private static string GetSingleSpecialismProperty(TqRegistrationPathway registrationPathway, Func<TqRegistrationSpecialism, string> getPropertyValue)
        {
            TqRegistrationSpecialism registrationSpecialism = registrationPathway.TqRegistrationSpecialisms.FirstOrDefault();
            return registrationSpecialism != null ? getPropertyValue(registrationSpecialism) : string.Empty;
        }

        private static string GetDualSpecialismProperty(TqRegistrationPathway registrationPathway, Func<TlDualSpecialism, string> getPropertyValue)
        {
            IEnumerable<TlDualSpecialism> dualSpecialisms = registrationPathway.TqRegistrationSpecialisms
                                                                .Select(p => p.TlSpecialism)
                                                                .SelectMany(s => s.TlDualSpecialismToSpecialisms)
                                                                .Select(p => p.DualSpecialism);

            IGrouping<int, TlDualSpecialism> dualSpecialism = dualSpecialisms.GroupBy(p => p.Id).FirstOrDefault(p => p.Count() == 2);
            return dualSpecialism != null ? getPropertyValue(dualSpecialism.First()) : string.Empty;
        }



        /*
        public string Status { get; set; }
        public long UkPrn { get; set; }
        public string ProviderName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public string TlevelTitle { get; set; }
        public int StartYear { get; set; }
        public string CoreComponent { get; set; }
        public string LarId { get; set; }
        public string CoreCode { get; set; }
        public string CoreResult { get; set; }
        public string OccupationalSpecialism { get; set; }
        public string SpecialismCode { get; set; }
        public string SpecialismResult { get; set; }
        public string IndustryPlacementStatus { get; set; }
        public string OverallResult { get; set; }
         */
    }
}