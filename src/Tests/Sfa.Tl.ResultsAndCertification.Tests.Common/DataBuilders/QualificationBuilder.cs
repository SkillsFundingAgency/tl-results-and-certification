using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class QualificationBuilder
    {
        public Qualification Build(QualificationType qualificationType = null, TlLookup subjectLookup = null) => new Qualification
        {
            TlLookup = subjectLookup ?? new TlLookupBuilder().BuildSubjectType(),
            QualificationType = qualificationType ?? new QualificationTypeBuilder().Build(),
            Code = "100/1976/5",
            Title = "EDEXCEL Level 1/Level 2 GCSE in English Literature",
            IsSendQualification = false,
            IsActive = true,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Qualification> BuildList(QualificationType qualificationType = null)
        {
            qualificationType ??= new QualificationTypeBuilder().Build();
            var subjectLookupList = new TlLookupBuilder().BuildSubjectTypeList();
            var englishSubject = subjectLookupList.FirstOrDefault(x => x.Code.Equals("Eng", System.StringComparison.InvariantCultureIgnoreCase));
            var mathsSubject = subjectLookupList.FirstOrDefault(x => x.Code.Equals("Math", System.StringComparison.InvariantCultureIgnoreCase));

            var qualificationList = new List<Qualification> {
                new Qualification
                {
                    TlLookupId = mathsSubject.Id,
                    TlLookup = mathsSubject,
                    QualificationTypeId = qualificationType.Id,
                    QualificationType = qualificationType,
                    Code = "500/7856/2",
                    Title = "AQA Level 1/Level 2 GCSE in Mathematics",
                    IsSendQualification = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Qualification
                {                    
                    TlLookupId = englishSubject.Id,
                    TlLookup = englishSubject,
                    QualificationTypeId = qualificationType.Id,
                    QualificationType = qualificationType,
                    Code = "601/4292/3",
                    Title = "AQA Level 1/Level 2 GCSE (9-1) in English Language",
                    IsSendQualification = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Qualification
                {
                    TlLookupId = mathsSubject.Id,
                    TlLookup = mathsSubject,
                    QualificationTypeId = qualificationType.Id,
                    QualificationType = qualificationType,
                    Code = "100/3432/8",
                    Title = "CCEA Advanced GCE in Mathematics",
                    IsSendQualification = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Qualification
                {
                    TlLookupId = englishSubject.Id,
                    TlLookup = englishSubject,
                    QualificationTypeId = qualificationType.Id,
                    QualificationType = qualificationType,
                    Code = "500/8464/1",
                    Title = "Pearson Edexcel Functional Skills Qualification in English at Entry 3",
                    IsSendQualification = true,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return qualificationList;
        }
    }
}
