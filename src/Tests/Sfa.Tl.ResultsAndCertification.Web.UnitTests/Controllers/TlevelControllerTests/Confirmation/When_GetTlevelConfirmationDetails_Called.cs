using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Confirmation
{
    public class When_GetTlevelConfirmationDetails_Called : TestSetup
    {
        private TlevelConfirmationViewModel _expectedResult;
        private string _cacheKey;

        public override void Given()
        {
            Id = 1;
            _cacheKey = string.Concat(CacheKey, Constants.TlevelConfirmation);
            _expectedResult = new TlevelConfirmationViewModel { PathwayId = 1, ShowMoreTlevelsToReview = true, TlevelConfirmationText = "Success", TlevelTitle = "Title", IsQueried = true };

            TlevelLoader.GetTlevelConfirmationDetailsAsync(AoUkprn, Id).Returns(_expectedResult);
            CacheService.GetAndRemoveAsync<bool>(_cacheKey).Returns(true);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received(1).GetTlevelConfirmationDetailsAsync(AoUkprn, Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelConfirmationViewModel;

            model.Should().NotBeNull();
            model.PathwayId.Should().Be(_expectedResult.PathwayId);
            model.ShowMoreTlevelsToReview.Should().Be(_expectedResult.ShowMoreTlevelsToReview);
            model.IsQueried.Should().Be(_expectedResult.IsQueried);
            model.TlevelConfirmationText.Should().Be(_expectedResult.TlevelConfirmationText);
            model.TlevelTitle.Should().Be(_expectedResult.TlevelTitle);
        }
    }
}
