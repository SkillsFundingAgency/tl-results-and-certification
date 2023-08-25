using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasRecordResultsSegment : IUcasRecordSegment<UcasRecordResultsSegment>
    {
        public UcasDataType UcasDataType => UcasDataType.Results;
        private readonly IUcasRepository _ucasRepository;

        public UcasRecordResultsSegment(IUcasRepository ucasRepository)
        {
            _ucasRepository = ucasRepository;
        }

        public void AddCoreSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            var overallResults = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            var overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResults.Details);

            var ucasCoreComponent = new UcasDataComponent
            {
                SubjectCode = overallResultDetails.PathwayLarId,
                Grade = UcasDataAbbreviations.GetAbbreviatedResult(UcasResultType.PathwayResult, overallResultDetails.PathwayResult),
                PreviousGrade = string.Empty
            };

            ucasDataComponents.Add(ucasCoreComponent);
        }

        public void AddSpecialismSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            var overallResults = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            var overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResults.Details);

            if (overallResultDetails.SpecialismDetails != null)
            {
                var isDualSpecialism = overallResultDetails.SpecialismDetails.Count > 1;
                overallResultDetails = isDualSpecialism ? ReplaceDualSpecialismCode(overallResultDetails) : overallResultDetails;
                foreach (var specialism in overallResultDetails.SpecialismDetails)
                {
                    var specialismResult = isDualSpecialism ? overallResults.SpecialismResultAwarded : specialism.SpecialismResult;
                    var ucasSpecialismComponent = new UcasDataComponent
                    {
                        SubjectCode = specialism.SpecialismLarId,
                        Grade = UcasDataAbbreviations.GetAbbreviatedResult(UcasResultType.SpecialismResult, specialismResult),
                        PreviousGrade = string.Empty
                    };

                    ucasDataComponents.Add(ucasSpecialismComponent);
                }
            }
        }

        public OverallResultDetail ReplaceDualSpecialismCode(OverallResultDetail overallResultDetails)
        {
           var specialismDetails = overallResultDetails.SpecialismDetails.Select(t => t.SpecialismLarId).ToList();

            if (specialismDetails.Count >1)
            {
                var dualCombinationCode = _ucasRepository.GetDualSpecialismLarId(specialismDetails);  
                
                if (!string.IsNullOrEmpty(dualCombinationCode))
                {
                    overallResultDetails.SpecialismDetails.ToList().ForEach(f => f.SpecialismLarId = dualCombinationCode);
                    overallResultDetails.SpecialismDetails = new List<OverallSpecialismDetail>() { overallResultDetails.SpecialismDetails.First() };
                }
            }

            return overallResultDetails;
        }

        public void AddOverallResultSegment(IList<UcasDataComponent> ucasDataComponents, string overallSubjectCode, string resultAwarded)
        {
            ucasDataComponents.Add(new UcasDataComponent
            {
                SubjectCode = overallSubjectCode,
                Grade = UcasDataAbbreviations.GetAbbreviatedResult(UcasResultType.OverallResult, resultAwarded),
                PreviousGrade = string.Empty
            });
        }

        public void AddIndustryPlacementResultSegment(IList<UcasDataComponent> ucasDataComponents, string industryPlacementCode, TqRegistrationPathway pathway)
        {
            var overallResults = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            var overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResults.Details);

            if (overallResultDetails.IndustryPlacementStatus !=null)
            {
                ucasDataComponents.Add(new UcasDataComponent
                {
                    SubjectCode = industryPlacementCode,
                    Grade = UcasDataAbbreviations.GetAbbreviatedResult(UcasResultType.IndustryPlacement,overallResultDetails.IndustryPlacementStatus),
                    PreviousGrade = string.Empty
                });
            }
        }

    }
}
