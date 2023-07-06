using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class DualSpecialismOverallGradeLookupBuilder
    {
        private readonly TlLookupBuilder _tlLookupBuilder = new TlLookupBuilder();

        public DualSpecialismOverallGradeLookup Build()
        {
            TlLookup specialismResult = _tlLookupBuilder.BuildSpecialismResult();

            return new DualSpecialismOverallGradeLookup
            {
                FirstTlLookupSpecialismGradeId = specialismResult.Id,
                SecondTlLookupSpecialismGradeId = specialismResult.Id,
                TlLookupOverallSpecialismGradeId = specialismResult.Id,
                FirstTlLookupSpecialismGrade = specialismResult,
                SecondTlLookupSpecialismGrade = specialismResult,
                TlLookupOverallSpecialismGrade = specialismResult,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<DualSpecialismOverallGradeLookup> BuildList(IList<TlLookup> tlLookups = null)
        {
            if (tlLookups == null)
            {
                tlLookups = new TlLookupBuilder().BuildFullTlLookupList();
            }

            return new List<DualSpecialismOverallGradeLookup>
            {
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Distinction, SpecialismComponentGradeLookup.Distinction, SpecialismComponentGradeLookup.Distinction),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Distinction, SpecialismComponentGradeLookup.Merit, SpecialismComponentGradeLookup.Distinction),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Distinction, SpecialismComponentGradeLookup.Pass, SpecialismComponentGradeLookup.Merit),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Merit, SpecialismComponentGradeLookup.Distinction, SpecialismComponentGradeLookup.Distinction),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Merit, SpecialismComponentGradeLookup.Merit, SpecialismComponentGradeLookup.Merit),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Merit, SpecialismComponentGradeLookup.Pass, SpecialismComponentGradeLookup.Pass),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Pass, SpecialismComponentGradeLookup.Distinction, SpecialismComponentGradeLookup.Merit),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Pass, SpecialismComponentGradeLookup.Merit, SpecialismComponentGradeLookup.Pass),
                NewDualSpecialismOverallGradeLookup(tlLookups, SpecialismComponentGradeLookup.Pass, SpecialismComponentGradeLookup.Pass, SpecialismComponentGradeLookup.Pass)
            };
        }

        private DualSpecialismOverallGradeLookup NewDualSpecialismOverallGradeLookup(
            IList<TlLookup> tlLookups,
            SpecialismComponentGradeLookup firstSpecialismGrade,
            SpecialismComponentGradeLookup secondSpecialismGrade,
            SpecialismComponentGradeLookup overallSpecialismGrade)
        {
            TlLookup firstSpecialismGradeTlLookup = tlLookups.FirstOrDefault(p => p.Value == firstSpecialismGrade.ToString());
            TlLookup secondSpecialismGradeTlLookup = tlLookups.FirstOrDefault(p => p.Value == secondSpecialismGrade.ToString());
            TlLookup overallSpecialismGradeTlLookup = tlLookups.FirstOrDefault(p => p.Value == overallSpecialismGrade.ToString());

            return new DualSpecialismOverallGradeLookup
            {
                FirstTlLookupSpecialismGradeId = firstSpecialismGradeTlLookup.Id,
                FirstTlLookupSpecialismGrade = firstSpecialismGradeTlLookup,
                SecondTlLookupSpecialismGradeId = secondSpecialismGradeTlLookup.Id,
                SecondTlLookupSpecialismGrade = secondSpecialismGradeTlLookup,
                TlLookupOverallSpecialismGradeId = overallSpecialismGradeTlLookup.Id,
                TlLookupOverallSpecialismGrade = overallSpecialismGradeTlLookup,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }
    }
}