using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class QualificationGradeBuilder
    {
        public QualificationGrade Build(QualificationType qualificationType = null) => new QualificationGrade
        {
            Grade = "A",
            QualificationType = qualificationType ?? new QualificationTypeBuilder().Build(),
            IsAllowable = true,
            IsActive = true,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<QualificationGrade> BuildList(QualificationType qualificationType = null)
        {
            qualificationType ??= new QualificationTypeBuilder().Build();

            var qualificationGrades = new List<QualificationGrade>
            {
                new QualificationGrade
                {
                    Grade = "A",
                    QualificationTypeId = qualificationType.Id,
                    IsAllowable = true,
                    IsSendGrade = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new QualificationGrade
                {
                    Grade = "B",
                    QualificationTypeId = qualificationType.Id,
                    IsAllowable = true,
                    IsSendGrade = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new QualificationGrade
                {
                    Grade = "C",
                    QualificationTypeId = qualificationType.Id,
                    IsAllowable = true,
                    IsSendGrade = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new QualificationGrade
                {
                    Grade = "D",
                    QualificationTypeId = qualificationType.Id,
                    IsAllowable = false,
                    IsSendGrade = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return qualificationGrades;
        }
    }
}
