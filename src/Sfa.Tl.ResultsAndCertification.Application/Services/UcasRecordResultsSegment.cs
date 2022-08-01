using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
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
                foreach (var specialism in overallResultDetails.SpecialismDetails)
                {
                    var ucasSpecialismComponent = new UcasDataComponent
                    {
                        SubjectCode = specialism.SpecialismLarId,
                        Grade = UcasDataAbbreviations.GetAbbreviatedResult(UcasResultType.SpecialismResult, specialism.SpecialismResult),
                        PreviousGrade = string.Empty
                    };

                    ucasDataComponents.Add(ucasSpecialismComponent);
                }
            }
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
    }
}
