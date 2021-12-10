using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using RemoveSpecialismAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveSpecialismAssessmentEntries;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                AssessmentSeriesName = "Summer 2022",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        Assessments = new List<SpecialismAssessmentViewModel>{ new SpecialismAssessmentViewModel { AssessmentId = 11 } }
                    }
                }
            };

            SpecialismAssessmentIds = _mockresult.SpecialismDetails[0].Assessments.FirstOrDefault().AssessmentId.ToString();
            _mockresult.SpecialismAssessmentIds = SpecialismAssessmentIds;
            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ProfileId, SpecialismAssessmentIds).Returns(_mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RemoveSpecialismAssessmentEntryViewModel));

            var model = viewResult.Model as RemoveSpecialismAssessmentEntryViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_mockresult.ProfileId);
            model.AssessmentSeriesName.Should().Be(_mockresult.AssessmentSeriesName);
            model.SpecialismDetails.Should().BeEquivalentTo(_mockresult.SpecialismDetails);
            model.SpecialismDisplayName.Should().Be($"{_mockresult.SpecialismDetails[0].Name} ({_mockresult.SpecialismDetails[0].LarId})");
            model.CanRemoveAssessmentEntry.Should().BeNull();
            model.IsValidToRemove.Should().BeTrue();

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(RemoveSpecialismAssessmentEntryContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(RemoveSpecialismAssessmentEntryContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(RemoveSpecialismAssessmentEntryContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(RemoveSpecialismAssessmentEntryContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(RemoveSpecialismAssessmentEntryContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockresult.TlevelTitle);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(_mockresult.ProfileId.ToString());
        }
    }
}
