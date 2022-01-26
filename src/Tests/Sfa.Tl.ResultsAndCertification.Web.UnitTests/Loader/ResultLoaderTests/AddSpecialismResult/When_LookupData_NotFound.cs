using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.AddSpecialismResult
{
    public class When_LookupData_NotFound : TestSetup
    {
        public override void Given()
        {
            var lookupApiClientResponse = new List<LookupData>();

            ViewModel = new ManageSpecialismResultViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 1,
                SelectedGradeCode = "SCG1",
                LookupId = 1
            };

            InternalApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(lookupApiClientResponse);            
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
