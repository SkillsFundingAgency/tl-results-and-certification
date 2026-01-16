using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_Dual_Specialism_With_NoSpecialismResult : TestSetup
    {
        public override void Given()
        {
            ProfileId = 10;
            Mockresult = new LearnerRecordDetailsViewModel
            {
                ProfileId = 10,
                RegistrationPathwayId = 15,
                Uln = 1235469874,
                LearnerName = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 58794528,
                TlevelTitle = "Tlevel in Test Pathway Name",
                AcademicYear = 2020,
                AwardingOrganisationName = "Pearson",
                IsLearnerRegistered = true,
                RegistrationPathwayStatus = RegistrationPathwayStatus.Withdrawn,
                IsPendingWithdrawal = false,
                IndustryPlacementId = 10,
                IndustryPlacementStatus = IndustryPlacementStatus.NotSpecified,
                OverallResultDetails = new Models.OverallResults.OverallResultDetail
                {
                    PathwayName = "Pathway 1",
                    PathwayResult = "Distinction",
                    SpecialismDetails = new List<Models.OverallResults.OverallSpecialismDetail>
                    {
                        new Models.OverallResults.OverallSpecialismDetail
                        {
                            SpecialismName = "Specialism 1"
                        },
                         new Models.OverallResults.OverallSpecialismDetail
                        {
                            SpecialismName = "Specialism 2"
                        }
                    },
                    OverallResult = "Distinction"
                },
                OverallResultPublishDate = DateTime.UtcNow,
                LastDocumentRequestedDate = "01/01/2022".ToDateTime(),
                IsReprint = false
            };

            TrainingProviderLoader.GetLearnerRecordDetailsViewModel(ProviderUkprn, ProfileId, ResultsAndCertificationConfiguration.DocumentRerequestInDays).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsViewModel(ProviderUkprn, ProfileId, ResultsAndCertificationConfiguration.DocumentRerequestInDays);
        }

        [Fact]
        public void Then_Expect_OneSpecialism_Result()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as LearnerRecordDetailsViewModel;
            model.OverallResultDetails.SpecialismDetails.ForEach(x => x.SpecialismResult.Should().BeNullOrEmpty());
            model.OverallResultDetails.SpecialismDetails.ForEach(x => x.SpecialismName.Should().NotBeNullOrEmpty());
            model.Specialisms.Should().BeNullOrEmpty();
        }
    }
}
