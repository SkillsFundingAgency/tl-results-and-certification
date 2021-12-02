using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AddSpecialismAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddSpecialismAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddSpecialismAssessmentSeriesGet
{
    public class When_Called_With_Couplets_Valid_Data : TestSetup
    {
        private AddSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        Assessments = new List<SpecialismAssessmentViewModel>()                        
                    },
                    new SpecialismViewModel
                    {
                        Id = 6,
                        LarId = "ZT2158999",
                        Name = "Specialism Name2",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            AssessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism).Returns(_mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddSpecialismAssessmentEntryViewModel));

            var model = viewResult.Model as AddSpecialismAssessmentEntryViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_mockresult.ProfileId);
            model.AssessmentSeriesId.Should().Be(_mockresult.AssessmentSeriesId);
            model.AssessmentSeriesName.Should().Be(_mockresult.AssessmentSeriesName);
            model.SpecialismDetails.Should().BeEquivalentTo(_mockresult.SpecialismDetails);
            model.IsResitForSpecialism.Should().BeFalse();
            model.DisplayMultipleSpecialismsCombined.Should().BeTrue();

            var specialismDisplayName = string.Join(Constants.AndSeperator, _mockresult.SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})"));
            model.SpecialismDisplayName.Should().Be(specialismDisplayName);
            model.IsOpted.Should().BeNull();

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(AddSpecialismAssessmentEntryContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(AddSpecialismAssessmentEntryContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(AddSpecialismAssessmentEntryContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(AddSpecialismAssessmentEntryContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(AddSpecialismAssessmentEntryContent.Title_TLevel_Text);
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
