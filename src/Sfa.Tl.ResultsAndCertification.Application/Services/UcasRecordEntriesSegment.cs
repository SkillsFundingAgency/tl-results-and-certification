using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasRecordEntriesSegment : IUcasRecordSegment<UcasRecordEntriesSegment>
    {
        private readonly SpecialismCodeConverter _specialismCodeConverter;

        public UcasDataType UcasDataType => UcasDataType.Entries;

        public UcasRecordEntriesSegment(SpecialismCodeConverter specialismCodeConverter)
        {
            _specialismCodeConverter = specialismCodeConverter;
        }

        public void AddCoreSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            if (!pathway.TqPathwayAssessments.Any())
                return;

            AddDataComponent(ucasDataComponents, pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId);
        }

        public void AddSpecialismSegment(IList<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            string larId = _specialismCodeConverter.Convert(pathway.TqRegistrationSpecialisms);

            if (string.IsNullOrEmpty(larId))
                return;

            AddDataComponent(ucasDataComponents, larId);
        }

        public void AddOverallResultSegment(IList<UcasDataComponent> ucasDataComponents, string overallSubjectCode, string resultAwarded = "")
        {
            if (ucasDataComponents.IsNullOrEmpty())
                return;

            AddDataComponent(ucasDataComponents, overallSubjectCode);
        }

        public void AddIndustryPlacementResultSegment(IList<UcasDataComponent> ucasDataComponents, string industryPlacementCode, TqRegistrationPathway pathway)
        {
            if (ucasDataComponents.IsNullOrEmpty())
                return;

            AddDataComponent(ucasDataComponents, industryPlacementCode);
        }

        private void AddDataComponent(IList<UcasDataComponent> ucasDataComponents, string subjectCode)
            => ucasDataComponents.Add(new UcasDataComponent
            {
                SubjectCode = subjectCode,
                Grade = string.Empty,
                PreviousGrade = string.Empty
            });
    }
}
