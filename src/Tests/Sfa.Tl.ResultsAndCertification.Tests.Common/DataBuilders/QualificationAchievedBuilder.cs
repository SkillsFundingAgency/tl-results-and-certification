using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class QualificationAchievedBuilder
    {
        public QualificationAchieved Build(TqRegistrationProfile tqRegistrationProfile = null, Qualification qualification = null, QualificationGrade qualificationGrade = null)
        {
            tqRegistrationProfile ??= new TqRegistrationProfileBuilder().Build();
            qualification ??= new QualificationBuilder().Build();
            qualificationGrade ??= new QualificationGradeBuilder().Build();

            return new QualificationAchieved
            {
                TqRegistrationProfileId = tqRegistrationProfile.Id,
                TqRegistrationProfile = tqRegistrationProfile,
                QualificationId = qualification.Id,
                Qualification = qualification,
                QualificationGradeId = qualificationGrade.Id,
                QualificationGrade = qualificationGrade,
                IsAchieved = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<QualificationAchieved> BuildList(TqRegistrationProfile tqRegistrationProfile = null)
        {
            tqRegistrationProfile ??= new TqRegistrationProfileBuilder().Build();
            var qualifications = new QualificationBuilder().BuildList();
            var qualificationGrades = new QualificationGradeBuilder().BuildList();

            var qualificationAchievedList = new List<QualificationAchieved> {
                new QualificationAchieved
                {
                    TqRegistrationProfileId = tqRegistrationProfile.Id,
                    TqRegistrationProfile = tqRegistrationProfile,
                    QualificationId = qualifications[0].Id,
                    Qualification = qualifications[0],
                    QualificationGradeId = qualificationGrades[0].Id,
                    QualificationGrade = qualificationGrades[0],
                    IsAchieved = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new QualificationAchieved
                {
                    TqRegistrationProfileId = tqRegistrationProfile.Id,
                    TqRegistrationProfile = tqRegistrationProfile,
                    QualificationId = qualifications[1].Id,
                    Qualification = qualifications[1],
                    QualificationGradeId = qualificationGrades[1].Id,
                    QualificationGrade = qualificationGrades[1],
                    IsAchieved = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return qualificationAchievedList;
        }
    }
}