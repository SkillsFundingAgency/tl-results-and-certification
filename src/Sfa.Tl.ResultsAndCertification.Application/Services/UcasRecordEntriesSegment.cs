using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections;
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
            pathway = ReplaceDualSpecialismCodes(pathway);

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

        public TqRegistrationPathway ReplaceDualSpecialismCodes(TqRegistrationPathway pathway)
        {
            if (pathway.TqRegistrationSpecialisms.Count > 1)
            {
                var dualSpecialims = UcasDataAbbreviations._dualSpecialisms.SelectMany(t => t.Value).ToList();
                var pathwayRegistrationSpecialism = pathway.TqRegistrationSpecialisms.Select(t => t.TlSpecialism.LarId).ToList();


                if (dualSpecialims.Any(b => pathwayRegistrationSpecialism.Any(a => b.Contains(a))))
                {
                    var dualCode = UcasDataAbbreviations._dualSpecialisms.Where(f => f.Value.Intersect(pathwayRegistrationSpecialism, StringComparer.OrdinalIgnoreCase).Count() == 2).FirstOrDefault().Key;
                    if (!string.IsNullOrEmpty(dualCode))
                    {
                        pathway.TqRegistrationSpecialisms.ToList().ForEach(f => f.TlSpecialism.LarId = dualCode);
                        pathway.TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>() { pathway.TqRegistrationSpecialisms.First() };
                    }
                }
            }

            return pathway;
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

        public void AddIndustryPlacementResultSegment(IList<UcasDataComponent> ucasDataComponents, string industryPlacementCode, TqRegistrationPathway pathway)
        {
            if (ucasDataComponents.Any())
            {
                ucasDataComponents.Add(new UcasDataComponent
                {
                    SubjectCode = string.IsNullOrEmpty(industryPlacementCode) ? "INDUSTRYP" : industryPlacementCode,
                    Grade = string.Empty,
                    PreviousGrade = string.Empty
                });
            }
        }
    }
}
