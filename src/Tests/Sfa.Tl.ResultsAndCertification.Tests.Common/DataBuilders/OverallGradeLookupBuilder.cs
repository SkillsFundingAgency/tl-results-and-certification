using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class OverallGradeLookupBuilder
    {
        public OverallGradeLookup Build(TlPathway tlPathway = null)
        {
            tlPathway ??= new TlPathwayBuilder().Build(EnumAwardingOrganisation.Pearson);
            var tlLookupCoreResult = new TlLookupBuilder().Build();
            var tlLookupSpecialismResult = new TlLookupBuilder().BuildSpecialismResult();
            var tlLookupOverallResult = new TlLookupBuilder().BuildOverallResult();

            return new OverallGradeLookup
            {
                TlPathwayId = tlPathway.Id,
                TlPathway = tlPathway,
                TlLookupCoreGradeId = tlLookupCoreResult.Id,
                TlLookupSpecialismGradeId = tlLookupSpecialismResult.Id,
                TlLookupOverallGradeId = tlLookupOverallResult.Id,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<OverallGradeLookup> BuildList(IList<TlPathway> tlPathways = null)
        {
            tlPathways ??= new TlPathwayBuilder().BuildList(EnumAwardingOrganisation.Pearson);

            var tlLookupCoreResults = new TlLookupBuilder().BuildList();
            var tlLookupSpecialismResults = new TlLookupBuilder().BuildSpecialismResultList();
            var tlLookupOverallResults = new TlLookupBuilder().BuildOverallResultList();

            return new List<OverallGradeLookup>
            {
                new OverallGradeLookup
                {
                    TlPathwayId = tlPathways[0].Id,
                    TlPathway = tlPathways[0],
                    TlLookupCoreGradeId = tlLookupCoreResults[0].Id,
                    TlLookupSpecialismGradeId = tlLookupSpecialismResults[0].Id,
                    TlLookupOverallGradeId = tlLookupOverallResults[0].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new OverallGradeLookup
                {
                    TlPathwayId = tlPathways[0].Id,
                    TlPathway = tlPathways[0],
                    TlLookupCoreGradeId = tlLookupCoreResults[1].Id,
                    TlLookupSpecialismGradeId = tlLookupSpecialismResults[1].Id,
                    TlLookupOverallGradeId = tlLookupOverallResults[1].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new OverallGradeLookup
                {
                    TlPathwayId = tlPathways[0].Id,
                    TlPathway = tlPathways[0],
                    TlLookupCoreGradeId = tlLookupCoreResults[2].Id,
                    TlLookupSpecialismGradeId = tlLookupSpecialismResults[2].Id,
                    TlLookupOverallGradeId = tlLookupOverallResults[2].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
            };
        }

        public IList<OverallGradeLookup> BuildList(IList<TlLookup> tlLookups, IList<Tuple<int,int,int,int>> seedData)
        {
            var returnList = new List<OverallGradeLookup>();

            foreach(var data in seedData)
            {
                var coreGradeTlLookup = tlLookups.FirstOrDefault(x => x.Id == data.Item2);
                var specialismGradeTlLookup = tlLookups.FirstOrDefault(x => x.Id == data.Item3);
                var overallGradeTlLookup = tlLookups.FirstOrDefault(x => x.Id == data.Item4);

                returnList.Add(new OverallGradeLookup
                {
                    TlPathwayId = data.Item1,
                    TlLookupCoreGradeId = coreGradeTlLookup.Id,
                    TlLookupCoreGrade = coreGradeTlLookup,
                    TlLookupSpecialismGradeId = specialismGradeTlLookup.Id,
                    TlLookupSpecialismGrade = specialismGradeTlLookup,
                    TlLookupOverallGradeId = overallGradeTlLookup.Id,
                    TlLookupOverallGrade = overallGradeTlLookup,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                });
            }
            return returnList;
        }
    }
}
