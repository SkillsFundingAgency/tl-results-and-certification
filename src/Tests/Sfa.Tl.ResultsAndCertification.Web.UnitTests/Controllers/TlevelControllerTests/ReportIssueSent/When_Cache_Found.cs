using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueSent
{
    public class When_Cache_Found : TestSetup
    {
        private TlevelQuerySentViewModel _querySentViewModel;

        public override void Given()
        {
            _querySentViewModel = new TlevelQuerySentViewModel { TlevelTitle = "T Level in Education" };
            CacheService.GetAndRemoveAsync<TlevelQuerySentViewModel>(CacheKey).Returns(_querySentViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<TlevelQuerySentViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelQuerySentViewModel;

            model.Should().NotBeNull();
            model.TlevelTitle.Should().Be(_querySentViewModel.TlevelTitle);
        }
    }
}
