using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_Result_Is_Final : TestSetup
    {
        private PrsLearnerDetailsViewModel1 _mockResult;

        public override void Given()
        {
            ProfileId = 11;

            _mockResult = new PrsLearnerDetailsViewModel1
            {
                Firstname = "John",
                Lastname = "Smith",
                Uln = 5647382910,
                DateofBirth = DateTime.Today,
                ProviderName = "Barnsley College",
                ProviderUkprn = 100656,
                TlevelTitle = "Design, Surveying and Planning for Construction",

                // Core
                CoreComponentDisplayName = "Design, Surveying and Planning (123456)",
                PrsCoreComponentExams = new List<PrsComponentExamViewModel>
                {
                    new PrsComponentExamViewModel { AssessmentSeries = "Autumn 2021", Grade = "B", PrsStatus = PrsStatus.Final, LastUpdated = "5 June 2021", UpdatedBy = "User 2", RommEndDate = DateTime.Today.AddDays(-1), AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 1 },
                },

                // Specialisms
                PrsSpecialismComponents = new List<PrsSpecialismComponentViewModel>
                {
                    new PrsSpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Plumbing (456789)",
                        SpecialismComponentExams = new List<PrsComponentExamViewModel>
                        {
                            new PrsComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "Merit", PrsStatus = null, LastUpdated = "6 June 2022", UpdatedBy = "User 1", RommEndDate = DateTime.Today.AddDays(-1), AppealEndDate = DateTime.Today.AddDays(-10), AssessmentId = 7 }
                        }
                    },

                    new PrsSpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Heating (123658)",
                        SpecialismComponentExams = new List<PrsComponentExamViewModel>
                        {
                            new PrsComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "Merit", PrsStatus = null, LastUpdated = "6 June 2022", UpdatedBy = "User 1", RommEndDate = DateTime.Today.AddDays(-1), AppealEndDate = DateTime.Today.AddDays(-10), AssessmentId = 9 }
                        }
                    }
                }
            };           

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(AoUkprn, ProfileId).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsLearnerDetailsViewModel1;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_mockResult.ProfileId);
            model.Uln.Should().Be(_mockResult.Uln);
            model.Firstname.Should().Be(_mockResult.Firstname);
            model.Lastname.Should().Be(_mockResult.Lastname);
            model.LearnerName.Should().Be($"{_mockResult.Firstname} {_mockResult.Lastname}");
            model.DateofBirth.Should().Be(_mockResult.DateofBirth);
            model.ProviderName.Should().Be(_mockResult.ProviderName);
            model.ProviderUkprn.Should().Be(_mockResult.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockResult.TlevelTitle);
            
            // Uln
            model.SummaryUln.Title.Should().Be(LearnerDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockResult.Uln.ToString());

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(LearnerDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockResult.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Title.Should().Be(LearnerDetailsContent.Title_Provider_Name_Text);
            model.SummaryProviderName.Value.Should().Be(_mockResult.ProviderName);

            // ProviderUkprn
            model.SummaryProviderUkprn.Title.Should().Be(LearnerDetailsContent.Title_Provider_Ukprn_Text);
            model.SummaryProviderUkprn.Value.Should().Be(_mockResult.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(LearnerDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockResult.TlevelTitle);
            model.SuccessBanner.Should().BeNull();

            model.HasCoreResults.Should().BeTrue();
            model.CoreComponentDisplayName.Should().Be(_mockResult.CoreComponentDisplayName);
            model.PrsCoreComponentExams.Count.Should().Be(_mockResult.PrsCoreComponentExams.Count);

            foreach(var exam in model.PrsCoreComponentExams)
            {
                exam.IsAddRommAllowed.Should().BeFalse();
                exam.IsAddRommOutcomeAllowed.Should().BeFalse();
                exam.IsAddAppealAllowed.Should().BeFalse();
                exam.IsRequestChangeAllowed.Should().BeTrue();
            }

            model.PrsSpecialismComponents.Count.Should().Be(_mockResult.PrsSpecialismComponents.Count);

            foreach (var specialism in model.PrsSpecialismComponents)
            {
                specialism.SpecialismComponentExams.Count.Should().Be(1);
                specialism.HasSpecialismResults.Should().BeTrue();

                foreach (var exam in specialism.SpecialismComponentExams)
                {
                    exam.IsAddRommAllowed.Should().BeFalse();
                    exam.IsAddRommOutcomeAllowed.Should().BeFalse();
                    exam.IsAddAppealAllowed.Should().BeFalse();
                    exam.IsRequestChangeAllowed.Should().BeTrue();
                }
            }

            // Breadcrumb 
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.StartPostResultsService);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.StartReviewsAndAppeals);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.PrsSearchLearner);
        }
    }
}
