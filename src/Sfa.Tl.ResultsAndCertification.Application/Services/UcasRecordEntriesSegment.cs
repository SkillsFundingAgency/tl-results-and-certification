using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
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
        private readonly IUcasRepository _ucasRepository;

        public UcasRecordEntriesSegment(IUcasRepository ucasRepository)
        {
            _ucasRepository = ucasRepository;
        }
            

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
            string dualSpecialismCode = ReplaceDualSpecialismCodes(pathway);

            var ucasSpecialismComponent = new UcasDataComponent
            {
                SubjectCode = !string.IsNullOrEmpty(dualSpecialismCode) ? dualSpecialismCode :
                pathway.TqRegistrationSpecialisms.Any() ?
                pathway.TqRegistrationSpecialisms.First().TlSpecialism.LarId : string.Empty,
                Grade = string.Empty,
                PreviousGrade = string.Empty
            };

            ucasDataComponents.Add(ucasSpecialismComponent);

        }

        public string ReplaceDualSpecialismCodes(TqRegistrationPathway tqRegistrationPathway)
        {
            var dualSpecialismCode = string.Empty;

            if (tqRegistrationPathway.TqRegistrationSpecialisms.Count > 1)
            {
               var pathwayRegistrationSpecialism = tqRegistrationPathway.TqRegistrationSpecialisms.Select(t => t.TlSpecialism.LarId).ToList();
                if (pathwayRegistrationSpecialism.Any())
                {
                   dualSpecialismCode = _ucasRepository.GetDualSpecialismLarId(pathwayRegistrationSpecialism);
                }
            }

            return dualSpecialismCode;
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
                    SubjectCode = industryPlacementCode,
                    Grade = string.Empty,
                    PreviousGrade = string.Empty
                });
            }
        }
    }
}
