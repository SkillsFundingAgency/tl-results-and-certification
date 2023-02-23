using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIndustryPlacementViewModel
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private LearnerRecordDetails _mockLearnerRecord;
        public List<SummaryItemModel> _expectedSummaryDetails;

        public override void Given() 
        {
            _mockLearnerRecord = null;
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, null)
                .Returns(_mockLearnerRecord);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
