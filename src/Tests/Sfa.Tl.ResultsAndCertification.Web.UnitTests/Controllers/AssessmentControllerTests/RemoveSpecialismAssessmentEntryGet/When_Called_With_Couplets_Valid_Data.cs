using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using RemoveEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveSpecialismAssessmentEntries;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryGet
{
    public class When_Called_With_Couplets_Valid_Data : TestSetup
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
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>{ new SpecialismAssessmentViewModel { AssessmentId = 11 } }
                    },
                    new SpecialismViewModel
                    {
                        Id = 6,
                        LarId = "ZT2158999",
                        Name = "Specialism Name2",
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>{new SpecialismAssessmentViewModel { AssessmentId = 12 } }
                    }
                }
            };
            SpecialismAssessmentIds = string.Join(Constants.PipeSeperator, _mockresult.SpecialismDetails.SelectMany(s => s.Assessments).Select(a => a.AssessmentId));
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

            var specialismDisplayName = string.Join(Constants.AndSeperator, _mockresult.SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})"));
            model.SpecialismDisplayName.Should().Be(specialismDisplayName);
            model.CanRemoveAssessmentEntry.Should().BeNull();

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(RemoveEntryContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(RemoveEntryContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(RemoveEntryContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(RemoveEntryContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(RemoveEntryContent.Title_TLevel_Text);
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
