using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
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

        private readonly SpecialismCodeConverter _specialismCodeConverter;

        public UcasRecordEntriesSegment(SpecialismCodeConverter specialismCodeConverter)
        {
            _specialismCodeConverter = specialismCodeConverter;
        }

        public void AddCoreSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
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
            string larId = _specialismCodeConverter.Convert(pathway.TqRegistrationSpecialisms);

            if (string.IsNullOrEmpty(larId))
            {
                return;
            }

            var ucasSpecialismComponent = new UcasDataComponent
            {
                SubjectCode = larId,
                Grade = string.Empty,
                PreviousGrade = string.Empty
            };

            ucasDataComponents.Add(ucasSpecialismComponent);
        }

        //public string ReplaceDualSpecialismCodes(TqRegistrationPathway tqRegistrationPathway)
        //{
        //    var dualSpecialismCode = string.Empty;

        //    if (tqRegistrationPathway.TqRegistrationSpecialisms.Count > 1)
        //    {
        //        var pathwayRegistrationSpecialism = tqRegistrationPathway.TqRegistrationSpecialisms.Select(t => t.TlSpecialism.LarId).ToList();
        //        if (pathwayRegistrationSpecialism.Any())
        //        {
        //            dualSpecialismCode = _ucasRepository.GetDualSpecialismLarId(pathwayRegistrationSpecialism);
        //        }
        //    }

        //    return dualSpecialismCode;
        //}

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
