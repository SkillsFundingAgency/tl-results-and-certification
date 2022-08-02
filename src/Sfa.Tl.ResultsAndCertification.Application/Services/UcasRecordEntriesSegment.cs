using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasRecordEntriesSegment : IUcasRecordSegment<UcasRecordEntriesSegment>
    {
        public UcasDataType UcasDataType => UcasDataType.Entries;

        public void AddCoreSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            if (!pathway.TqPathwayAssessments.Any())
                return;

            var ucasCoreComponent = new UcasDataComponent
            {
                SubjectCode = pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                Grade = string.Empty,
                PreviousGrade = string.Empty
            };

            ucasDataComponents.Add(ucasCoreComponent);
        }

        public void AddSpecialismSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {

            foreach (var specialism in pathway.TqRegistrationSpecialisms)
            {
                if (!specialism.TqSpecialismAssessments.Any())
                    return;

                var ucasSpecialismComponent = new UcasDataComponent
                {
                    SubjectCode = specialism.TlSpecialism.LarId,
                    Grade = string.Empty,
                    PreviousGrade = string.Empty
                };

                ucasDataComponents.Add(ucasSpecialismComponent);
            }
        }

        public void AddOverallResultSegment(IList<UcasDataComponent> ucasDataComponents, string overallSubjectCode, string resultAwarded = "")
        {
            if (ucasDataComponents.Any())
            {
                ucasDataComponents.Add(new UcasDataComponent
                {
                    SubjectCode = overallSubjectCode,
                    Grade = string.Empty,
                    PreviousGrade = string.Empty
                });
            }
        }
    }
}
