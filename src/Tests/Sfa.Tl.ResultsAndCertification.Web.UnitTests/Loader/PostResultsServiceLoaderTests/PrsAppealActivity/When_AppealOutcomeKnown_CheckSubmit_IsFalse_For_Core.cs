using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsAppealActivity
{
    public class When_AppealOutcomeKnown_CheckSubmit_IsFalse_For_Core : TestSetup
    {
        private bool _expectedApiResult;
        private PrsAppealCheckAndSubmitViewModel _model;

        public override void Given()
        {
            _model = null;
            _expectedApiResult = false;
            InternalApiClient.PrsActivityAsync(Arg.Any<PrsActivityRequest>()).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.PrsAppealActivityAsync(AoUkprn, _model);
        }

        [Fact]
        public void Then_False_Returned()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
