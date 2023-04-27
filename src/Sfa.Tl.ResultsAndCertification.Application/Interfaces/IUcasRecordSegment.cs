using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IUcasRecordSegment<T> 
    {
        UcasDataType UcasDataType { get; }
        void AddCoreSegment(IList<UcasDataComponent> ucasDataComponent, TqRegistrationPathway pathway);
        void AddSpecialismSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway);
        void AddOverallResultSegment(IList<UcasDataComponent> ucasDataComponents, string overallSubjectCode, string resultAwarded = "");
        void AddIndustryPlacementResultSegment(IList<UcasDataComponent> ucasDataComponents, string industryPlacementCode, TqRegistrationPathway pathway);
    }
}
