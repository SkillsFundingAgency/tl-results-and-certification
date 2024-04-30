using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System;
using System.Collections.Generic;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using PrsLearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_Results_Is_Qpending_For_Specialism : TestSetup
    {
        private PrsLearnerDetailsViewModel _mockResult;

        public override void Given()
        {
            ProfileId = 11;

            _mockResult = new PrsLearnerDetailsViewModel
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
                    new PrsComponentExamViewModel { AssessmentSeries = "Autumn 2021", Grade = "B", PrsStatus = PrsStatus.BeingAppealed, LastUpdated = "5 June 2021", UpdatedBy = "User 2", AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 1 },
                },

                // Specialisms
                PrsSpecialismComponents = new List<PrsSpecialismComponentViewModel>
                {
                    new PrsSpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Plumbing (456789)",
                        SpecialismComponentExams = new List<PrsComponentExamViewModel>
                        {
                            new PrsComponentExamViewModel
                            {
                                AssessmentSeries = "Autumn 2021",
                                Grade = "Q - pending result",
                                GradeCode = "SCG5",
                                ComponentType = ComponentType.Specialism,
                                PrsStatus = null,
                                LastUpdated = "5 June 2021",
                                UpdatedBy = "User 2",
                                AppealEndDate = DateTime.Today.AddDays(10),
                                AssessmentId = 1
                            }
                        }
                    }
                }
            };

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsLearnerDetailsViewModel;

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
            model.SummaryUln.Title.Should().Be(PrsLearnerDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockResult.Uln.ToString());

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(PrsLearnerDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockResult.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Title.Should().Be(PrsLearnerDetailsContent.Title_Provider_Name_Text);
            model.SummaryProviderName.Value.Should().Be(_mockResult.ProviderName);

            // ProviderUkprn
            model.SummaryProviderUkprn.Title.Should().Be(PrsLearnerDetailsContent.Title_Provider_Ukprn_Text);
            model.SummaryProviderUkprn.Value.Should().Be(_mockResult.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(PrsLearnerDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockResult.TlevelTitle);

            model.HasCoreResults.Should().BeTrue();
            model.CoreComponentDisplayName.Should().Be(_mockResult.CoreComponentDisplayName);
            model.PrsCoreComponentExams.Count.Should().Be(_mockResult.PrsCoreComponentExams.Count);

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
                    exam.IsAddAppealOutcomeAllowed.Should().BeFalse();
                    exam.IsRequestChangeAllowed.Should().BeFalse();
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
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchRegistration);
            model.Breadcrumb.BreadcrumbItems[2].RouteAttributes.Should().HaveCount(1);
            model.Breadcrumb.BreadcrumbItems[2].RouteAttributes.Should().ContainEquivalentOf(new KeyValuePair<string, string>(Constants.Type, SearchRegistrationType.PostResult.ToString()));
        }
    }
}
