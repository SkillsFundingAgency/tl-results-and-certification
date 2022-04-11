﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsRommActivity
{
    public class When_Romm_Withdrawn_IsNotSuccess_For_Core : TestSetup
    {
        private bool _expectedApiResult;
        private PrsAddRommOutcomeViewModel _model;

        public override void Given()
        {
            _model = null;
            _expectedApiResult = false;
            InternalApiClient.PrsActivityAsync(Arg.Any<PrsActivityRequest>()).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.PrsRommActivityAsync(AoUkprn, _model);
        }

        [Fact]
        public void Then_False_Returned()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
