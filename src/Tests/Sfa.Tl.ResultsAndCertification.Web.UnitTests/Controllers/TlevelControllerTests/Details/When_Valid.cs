using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SummaryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.TlevelSummary;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Details
{
    public class When_Valid : TestSetup
    {
        private TLevelConfirmedDetailsViewModel _mockResult;

        public override void Given()
        {
            _mockResult = new TLevelConfirmedDetailsViewModel 
            { 
                TlevelTitle = "T level in Health",
                PathwayDisplayName = "Health<br/>(4546415)",
                IsValid = true,
                Specialisms = new List<string> { "Specialism1<br/>(87654665)", "Specialism2<br/>(345678)" }
            };

            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(AoUkprn, id).Returns(_mockResult);
        }

        [Fact]
        public void Then_Called_GetTlevelDetailsByPathwayId()
        {
            TlevelLoader.Received().GetTlevelDetailsByPathwayIdAsync(AoUkprn, id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var actualResult = viewResult.Model as TLevelConfirmedDetailsViewModel;

            actualResult.IsValid.Should().Be(_mockResult.IsValid);
            actualResult.TlevelTitle.Should().Be(_mockResult.TlevelTitle);
            actualResult.PathwayDisplayName.Should().Be(_mockResult.PathwayDisplayName);
            actualResult.Specialisms.Should().NotBeNull();
            actualResult.Specialisms.Count().Should().Be(_mockResult.Specialisms.Count());
            actualResult.Specialisms.Should().BeEquivalentTo(_mockResult.Specialisms);

            actualResult.BackLink.Should().NotBeNull();
            actualResult.BackLink.RouteName.Should().Be(RouteConstants.ConfirmedTlevels);
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
        }
    }
}
