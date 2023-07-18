using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction;
using Sfa.Tl.ResultsAndCertification.Models.AnalystResultsExtraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AnalystCoreResultExtractionMapper : Profile
    {
        public AnalystCoreResultExtractionMapper()
        {
            CreateMap<TqRegistrationPathway, AnalystCoreResultExtractionData>()
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
                .ForMember(d => d.CoreResult, opts => opts.MapFrom(s => GetHighestPathwayResult(s)));
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
    }
}