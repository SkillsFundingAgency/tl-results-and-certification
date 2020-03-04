using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsToReviewByUkprnAsync
{
    public class Then_On_More_To_Review_Expected_Results_Are_Returned : When_GetTlevelsToReviewByUkprnAsync__Is_Called
    {
        [Fact]
        public void Then_Two_Tlevels_AwaitingReview_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.TlevelsToReview.Should().NotBeNull();
            ActualResult.TlevelsToReview.Count().Should().Be(2);
        }

        [Fact]
        public void Then_IsOnlyOneTlevelReviewPending_Is_False()
        {
            ActualResult.IsOnlyOneTlevelReviewPending.Should().BeFalse();
        }

        [Fact]
        public void Then_ShowViewReviewedTlevelsLink_Is_True()
        {
            ActualResult.ShowViewReviewedTlevelsLink.Should().BeTrue();   
        }
    }
}
