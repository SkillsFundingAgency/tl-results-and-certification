using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqRegistrationProfileBuilder
    {
        public Domain.Models.TqRegistrationProfile Build() => new Domain.Models.TqRegistrationProfile
        {
            UniqueLearnerNumber = 1111111111,
            Firstname = "First 1",
            Lastname = "Last 1",
            DateofBirth = "10/10/1980".ParseStringToDateTimeWithFormat(),
            Gender = "Male",
            IsLearnerVerified = true,
            IsEnglishAndMathsAchieved = true,
            IsSendLearner = true,
            IsRcFeed = true,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TqRegistrationProfile> BuildList() => new List<Domain.Models.TqRegistrationProfile>
        {
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111111,
                Firstname = "First 1",
                Lastname = "Last 1",
                DateofBirth = "10/10/1980".ParseStringToDateTimeWithFormat(),
                Gender = "Male",
                IsLearnerVerified = true,
                IsEnglishAndMathsAchieved = true,
                IsSendLearner = true,
                IsRcFeed = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111112,
                Firstname = "First 2",
                Lastname = "Last 2",
                DateofBirth = "07/05/1981".ParseStringToDateTimeWithFormat(),
                Gender = "Female",
                IsLearnerVerified = false,
                IsEnglishAndMathsAchieved = false,
                IsSendLearner = false,
                IsRcFeed = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111113,
                Firstname = "First 3",
                Lastname = "Last 3",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                Gender = "Female",
                IsLearnerVerified = true,
                IsEnglishAndMathsAchieved = true,
                IsSendLearner = false,
                IsRcFeed = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111114,
                Firstname = "First 4",
                Lastname = "Last 4",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                Gender = "Male",
                IsLearnerVerified = false,
                IsEnglishAndMathsAchieved = true,
                IsSendLearner = false,
                IsRcFeed = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111115,
                Firstname = "First 5",
                Lastname = "Last 5",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111116,
                Firstname = "First 6",
                Lastname = "Last 6",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };

        public IList<Domain.Models.TqRegistrationProfile> BuildListWithoutLrsData() => new List<Domain.Models.TqRegistrationProfile>
        {
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111111,
                Firstname = "First 1",
                Lastname = "Last 1",
                DateofBirth = "10/10/1980".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111112,
                Firstname = "First 2",
                Lastname = "Last 2",
                DateofBirth = "07/05/1981".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111113,
                Firstname = "First 3",
                Lastname = "Last 3",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111114,
                Firstname = "First 4",
                Lastname = "Last 4",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111115,
                Firstname = "First 5",
                Lastname = "Last 5",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };

        public IList<Domain.Models.TqRegistrationProfile> BuildLrsVerificationLearningEventsList() => new List<Domain.Models.TqRegistrationProfile>
        {
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111111,
                Firstname = "First 1",
                Lastname = "Last 1",
                DateofBirth = "10/10/1980".ParseStringToDateTimeWithFormat(),
                Gender = "Male",
                IsLearnerVerified = null,
                IsEnglishAndMathsAchieved = null,
                IsSendLearner = null,
                IsRcFeed = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111112,
                Firstname = "First 2",
                Lastname = "Last 2",
                DateofBirth = "07/05/1981".ParseStringToDateTimeWithFormat(),
                Gender = "Female",
                IsLearnerVerified = false,
                IsEnglishAndMathsAchieved = null,
                IsSendLearner = null,
                IsRcFeed = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111113,
                Firstname = "First 3",
                Lastname = "Last 3",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                Gender = "Female",
                IsLearnerVerified = true,
                IsEnglishAndMathsAchieved = null,
                IsSendLearner = null,
                IsRcFeed = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111114,
                Firstname = "First 4",
                Lastname = "Last 4",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                Gender = "Male",
                IsLearnerVerified = true,
                IsEnglishAndMathsAchieved = false,
                IsSendLearner = null,
                IsRcFeed = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111115,
                Firstname = "First 5",
                Lastname = "Last 5",
                DateofBirth = "03/07/1982".ParseStringToDateTimeWithFormat(),
                Gender = "Male",
                IsLearnerVerified = true,
                IsEnglishAndMathsAchieved = true,
                IsSendLearner = null,
                IsRcFeed = false,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
