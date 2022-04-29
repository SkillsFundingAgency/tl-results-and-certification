using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class IndustryPlacementProvider
    {
        public static IndustryPlacement CreateIndustryPlacement(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var industryPlacement = new IndustryPlacementBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(industryPlacement);
            }
            return industryPlacement;
        }

        public static IndustryPlacement CreateIndustryPlacement(ResultsAndCertificationDbContext _dbContext, IndustryPlacement industryPlacement, bool addToDbContext = true)
        {
            if (industryPlacement == null)
            {
                industryPlacement = new IndustryPlacementBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(industryPlacement);
            }
            return industryPlacement;
        }

        public static IndustryPlacement CreateIndustryPlacement(ResultsAndCertificationDbContext _dbContext, int tqRegistrationPathwayId, IndustryPlacementStatus status, int? hours = null, bool addToDbContext = true)
        {
            var qualificationAchieved = new IndustryPlacement
            {
                TqRegistrationPathwayId = tqRegistrationPathwayId,
                Status = status,
                Hours = hours
            };

            if (addToDbContext)
            {
                _dbContext.Add(qualificationAchieved);
            }
            return qualificationAchieved;
        }

        public static List<IndustryPlacement> CreateIndustryPlacement(ResultsAndCertificationDbContext _dbContext, List<IndustryPlacement> industryPlacement, bool addToDbContext = true)
        {
            if (addToDbContext && industryPlacement != null && industryPlacement.Count > 0)
            {
                _dbContext.AddRange(industryPlacement);
            }
            return industryPlacement;
        }
    }
}
