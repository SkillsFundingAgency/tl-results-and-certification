using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using SummaryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.TlevelSummary;
using SummaryContentQry = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.QueriedDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.QueriedDetails
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private TlevelQueriedDetailsViewModel _mockResult;

        public override void Given()
        {
            _mockResult = new TlevelQueriedDetailsViewModel
            {
                PathwayDisplayName = "Education",
                TlevelTitle = "T level in Education",
                Specialisms = new List<string> { "Specialism1<br/>(87654665)", "Specialism2<br/>(345678)" },
                QueriedBy = "Test User",
                QueriedOn = "31 Aug 2021",
                IsValid = true,
            };

            TlevelLoader.GetQueriedTlevelDetailsAsync(AoUkprn, Id)
                .Returns(_mockResult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetQueriedTlevelDetailsAsync(AoUkprn, Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var actualResult = viewResult.Model as TlevelQueriedDetailsViewModel;

            actualResult.IsValid.Should().Be(_mockResult.IsValid);
            actualResult.TlevelTitle.Should().Be(_mockResult.TlevelTitle);
            actualResult.PathwayDisplayName.Should().Be(_mockResult.PathwayDisplayName);
            actualResult.Specialisms.Should().NotBeNull();
            actualResult.Specialisms.Count().Should().Be(_mockResult.Specialisms.Count());
            actualResult.Specialisms.Should().BeEquivalentTo(_mockResult.Specialisms);
            actualResult.QueriedOn.Should().Be(_mockResult.QueriedOn);
            actualResult.QueriedBy.Should().Be(_mockResult.QueriedBy);

            actualResult.BackLink.Should().NotBeNull();
            actualResult.BackLink.RouteName.Should().Be(RouteConstants.QueriedTlevels);
            actualResult.BackLink.RouteAttributes.Should().BeEmpty();

            // Summary SummaryTlevelTitle            
            actualResult.SummaryTlevelTitle.Should().NotBeNull();
            actualResult.SummaryTlevelTitle.Title.Should().Be(SummaryContent.Title_TLevel_Text);
            actualResult.SummaryTlevelTitle.Value.Should().Be(_mockResult.TlevelTitle);

            // Summary SummaryCore            
            actualResult.SummaryCore.Should().NotBeNull();
            actualResult.SummaryCore.Title.Should().Be(SummaryContent.Title_Core_Code_Text);
            actualResult.SummaryCore.Value.Should().Be(_mockResult.PathwayDisplayName);

            // Summary SummarySpecialisms            
            actualResult.SummarySpecialisms.Should().NotBeNull();
            actualResult.SummarySpecialisms.Title.Should().Be(SummaryContent.Title_Occupational_Specialism_Text);
            actualResult.SummarySpecialisms.Value.Should().BeEquivalentTo(_mockResult.Specialisms);

            // Summary Queried On          
            actualResult.SummaryQueriedOn.Should().NotBeNull();
            actualResult.SummaryQueriedOn.Title.Should().Be(SummaryContentQry.Title_Date_Queried);
            actualResult.SummaryQueriedOn.Value.Should().BeEquivalentTo(_mockResult.QueriedOn);

            // Summary Queried By          
            actualResult.SummaryQueriedBy.Should().NotBeNull();
            actualResult.SummaryQueriedBy.Title.Should().Be(SummaryContentQry.Title_Queried_By);
            actualResult.SummaryQueriedBy.Value.Should().BeEquivalentTo(_mockResult.QueriedBy);
        }
    }
}
