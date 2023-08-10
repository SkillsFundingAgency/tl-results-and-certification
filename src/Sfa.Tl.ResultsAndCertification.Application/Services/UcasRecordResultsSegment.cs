using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
                overallResultDetails = ReplaceDualSpecialismCode(overallResultDetails);

                if (overallResultDetails.SpecialismDetails.Any())
                {
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
                else
                {
                    var ucasSpecialismComponent = new UcasDataComponent
                    {
                        SubjectCode = string.Empty,
                        Grade = string.Empty,
                        PreviousGrade = string.Empty
                    };

                    ucasDataComponents.Add(ucasSpecialismComponent);
                }
            }
        }

        public OverallResultDetail ReplaceDualSpecialismCode(OverallResultDetail overallResultDetails)
        {
            var dualSpecialims = UcasDataAbbreviations._dualSpecialisms.SelectMany(t => t.Value).ToList();
            var specialismDetails = overallResultDetails.SpecialismDetails.Select(t => t.SpecialismLarId).ToList();

            if (dualSpecialims.Any(b => specialismDetails.Any(a => b.Contains(a))))
            {
                var dualCombinationCode = UcasDataAbbreviations._dualSpecialisms.Where(f => f.Value.Intersect(specialismDetails, StringComparer.OrdinalIgnoreCase).Count() == 2).FirstOrDefault().Key;
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

            if (overallResultDetails.IndustryPlacementStatus != null)
            {
                ucasDataComponents.Add(new UcasDataComponent
                {
                    SubjectCode = industryPlacementCode,
                    Grade = UcasDataAbbreviations.GetAbbreviatedResult(UcasResultType.IndustryPlacement, overallResultDetails.IndustryPlacementStatus),
                    PreviousGrade = string.Empty
                });
            }
        }

    }
}
