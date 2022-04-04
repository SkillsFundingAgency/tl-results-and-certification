using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
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
                CoreComponentExams = new List<ComponentExamViewModel>
                {
                    new ComponentExamViewModel { AssessmentSeries = "Autumn 2022", Grade = null, PrsStatus = null, LastUpdated = null, UpdatedBy = null, AppealEndDate = System.DateTime.Today.AddDays(10), AssessmentId = 1 },
                    new ComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "B", PrsStatus = null, LastUpdated = "6 June 2021", UpdatedBy = "User 3", AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 2 },
                    new ComponentExamViewModel { AssessmentSeries = "Autumn 2021", Grade = "B", PrsStatus = PrsStatus.BeingAppealed, LastUpdated = "5 June 2021", UpdatedBy = "User 2", AppealEndDate = System.DateTime.Today.AddDays(10), AssessmentId = 3 },
                    new ComponentExamViewModel { AssessmentSeries = "Summer 2021", Grade = "A", PrsStatus = PrsStatus.Final, LastUpdated = "4 June 2021", UpdatedBy = "User 1", AppealEndDate = System.DateTime.Today.AddDays(10), AssessmentId = 4 },
                    new ComponentExamViewModel { AssessmentSeries = "Autumn 2020", Grade = "D", PrsStatus = null, LastUpdated = "34 June 2021", UpdatedBy = "User 1", AppealEndDate = System.DateTime.Today.AddDays(-365), AssessmentId = 5 }
                },

                // Specialisms
                SpecialismComponents = new List<SpecialismComponentViewModel>
                {
                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Plumbing",
                        LarId = "S111",
                        SpecialismComponentExams = new List<ComponentExamViewModel>
                        {
                            new ComponentExamViewModel { AssessmentSeries = "Autumn 2022", Grade = null, PrsStatus = null, LastUpdated = null, UpdatedBy = null, AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 6 },
                            new ComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "Merit", PrsStatus = null, LastUpdated = "6 June 2021", UpdatedBy = "User 1",AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 7 }
                        }
                    },

                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Heating",
                        LarId = "S222",
                        SpecialismComponentExams = new List<ComponentExamViewModel>
                        {
                            new ComponentExamViewModel { AssessmentSeries = "Autumn 2022", Grade = null, PrsStatus = null, LastUpdated = "7 June 2021", UpdatedBy = "User 2", AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 8 },
                            new ComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "Merit", PrsStatus = null, LastUpdated = "6 June 2021", UpdatedBy = "User 1", AppealEndDate = DateTime.Today.AddDays(10), AssessmentId = 9 }
                        }
                    }
                }
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
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

            model.IsCoreAssessmentEntryRegistered.Should().BeTrue();
            foreach (var specialism in model.SpecialismComponents)
            {
                specialism.IsSpecialismAssessmentEntryRegistered.Should().BeTrue();
                specialism.IsCouplet.Should().BeFalse();
            }

            // Uln
            model.SummaryUln.Title.Should().Be(ResultDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockResult.Uln.ToString());

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(ResultDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockResult.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Title.Should().Be(ResultDetailsContent.Title_Provider_Name_Text);
            model.SummaryProviderName.Value.Should().Be(_mockResult.ProviderName);

            // ProviderUkprn
            model.SummaryProviderUkprn.Title.Should().Be(ResultDetailsContent.Title_Provider_Ukprn_Text);
            model.SummaryProviderUkprn.Value.Should().Be(_mockResult.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(ResultDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockResult.TlevelTitle);


            // Breadcrumbs
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultsDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Result_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchResults);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Results);
        }
    }
}
