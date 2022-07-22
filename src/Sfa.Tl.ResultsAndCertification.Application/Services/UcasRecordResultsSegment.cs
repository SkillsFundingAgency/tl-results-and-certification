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
            var overallResults = pathway.OverallResults.FirstOrDefault(); // risk?
            var overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResults.Details);

            var ucasCoreComponent = new UcasDataComponent
            {
                SubjectCode = overallResultDetails.PathwayLarId,
                Grade = overallResultDetails.PathwayResult,
                PreviousGrade = string.Empty
            };

            ucasDataComponents.Add(ucasCoreComponent);
        }

        public void AddSpecialismSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            var overallResults = pathway.OverallResults.FirstOrDefault(); // risk?
            var overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResults.Details);

            foreach (var specialism in overallResultDetails.SpecialismDetails)
            {
                var ucasSpecialismComponent = new UcasDataComponent
                {
                    SubjectCode = specialism.SpecialismLarId,
                    Grade = specialism.SpecialismResult,
                    PreviousGrade = string.Empty
                };

                ucasDataComponents.Add(ucasSpecialismComponent);
            }
        }

        public void AddOverallResultSegment(IList<UcasDataComponent> ucasDataComponents, string overallSubjectCode, string resultAwarded)
        {
            if (ucasDataComponents.Any())
            {
                ucasDataComponents.Add(new UcasDataComponent
                {
                    SubjectCode = overallSubjectCode,
                    Grade = resultAwarded,
                    PreviousGrade = string.Empty
                });
            }
        }
    }
}
