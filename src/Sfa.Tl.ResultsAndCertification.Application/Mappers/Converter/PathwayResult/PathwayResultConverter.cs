using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult
{
    public class PathwayResultConverter : PathwayResultConverterBase, IPathwayResultConverter
    {
        public TqPathwayResult Convert(IEnumerable<TqPathwayAssessment> sourceMember, ResolutionContext context)
        {
            return Convert(sourceMember);

        }
    }
}