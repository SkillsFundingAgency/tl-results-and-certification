using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlMandatoryAdditionalRequirementBuilder
    {
        public Domain.Models.TlMandatoryAdditionalRequirement Build() => new Domain.Models.TlMandatoryAdditionalRequirement
        {
            Name = "Surveying and design for construction and the built environment",
            IsRegulatedQualification = true,
            LarId = "11134567",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TlMandatoryAdditionalRequirement> BuildList() => new List<Domain.Models.TlMandatoryAdditionalRequirement>
        {
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Surveying and design for construction and the built environment",
                IsRegulatedQualification = true,
                LarId = "11134567",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Civil Engineering",
                IsRegulatedQualification = true,
                LarId = "11234567",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Building services design",
                IsRegulatedQualification = true,
                LarId = "11324567",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Hazardous materials analysis and surveying",
                IsRegulatedQualification = true,
                LarId = "11423567",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Early years education and childcare",
                IsRegulatedQualification = true,
                LarId = "11523467",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Assisting teaching",
                IsRegulatedQualification = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Supporting and mentoring students in further and higher education",
                IsRegulatedQualification = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlMandatoryAdditionalRequirement
            {
                Name = "Digital Production, Design and Development",
                IsRegulatedQualification = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
