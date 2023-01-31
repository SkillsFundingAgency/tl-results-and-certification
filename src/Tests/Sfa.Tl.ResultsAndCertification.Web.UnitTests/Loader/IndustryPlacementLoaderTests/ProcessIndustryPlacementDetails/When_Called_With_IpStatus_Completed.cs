using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessIndustryPlacementDetails
{
    public class When_Called_With_IpStatus_Completed : TestSetup
    {
        private readonly bool _expectedApiResult = true;
        private IndustryPlacementDetails _industryPlacementDetails;

        public override void Given()
        {
            CreateMapper();

            ProviderUkprn = 987654321;

            ViewModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel
                {
                    ProfileId = 1,
                    RegistrationPathwayId = 1,
                    PathwayId = 7,
                    IndustryPlacementStatus = IndustryPlacementStatus.Completed
                },
            };

            _industryPlacementDetails = new IndustryPlacementDetails
            {
                IndustryPlacementStatus = IndustryPlacementStatus.Completed.ToString(),
                HoursSpentOnPlacement = null,
                SpecialConsiderationReasons = new List<int?>(),
            };

            InternalApiClient.ProcessIndustryPlacementDetailsAsync(Arg.Is<IndustryPlacementRequest>(x =>
                                x.ProfileId == ViewModel.IpCompletion.ProfileId &&
                                x.RegistrationPathwayId == ViewModel.IpCompletion.RegistrationPathwayId &&
                                x.ProviderUkprn == ProviderUkprn &&
                                x.IndustryPlacementStatus == ViewModel.IpCompletion.IndustryPlacementStatus &&
                                x.IndustryPlacementDetails.IndustryPlacementStatus == _industryPlacementDetails.IndustryPlacementStatus &&
                                x.IndustryPlacementDetails.HoursSpentOnPlacement == _industryPlacementDetails.HoursSpentOnPlacement &&
                                x.IndustryPlacementDetails.SpecialConsiderationReasons.Count == _industryPlacementDetails.SpecialConsiderationReasons.Count &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                )).Returns(_expectedApiResult);

            Loader = new IndustryPlacementLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }       
    }
}
