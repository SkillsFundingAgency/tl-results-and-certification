using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlSpecialismBuilder
    {
        public Domain.Models.TlSpecialism Build() => new Domain.Models.TlSpecialism
        {
            Id = 1,
            PathwayId = 1,
            Name = "Surveying and design for construction and the built environment",
            LarId = "10123456",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TlSpecialism> BuildList() => new List<Domain.Models.TlSpecialism>
        {
            new Domain.Models.TlSpecialism
            {
                Id = 1,
                PathwayId = 1,
                Name = "Surveying and design for construction and the built environment",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Id = 2,
                PathwayId = 1,
                Name = "Civil Engineering",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Id = 3,
                PathwayId = 1,
                Name = "Building services design",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Id = 4,
                PathwayId = 1,
                Name = "Hazardous materials analysis and surveying",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
             new Domain.Models.TlSpecialism
            {
                Id = 5,
                PathwayId = 2,
                Name = "Early years education and childcare",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Id = 6,
                PathwayId = 2,
                Name = "Assisting teaching",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Id = 7,
                PathwayId = 2,
                Name = "Supporting and mentoring students in further and higher education",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Id = 8,
                PathwayId = 3,
                Name = "Digital Production, Design and Development",
                LarId = "10123456",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
        };
    }
}
