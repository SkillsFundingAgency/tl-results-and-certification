using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsGet
{
    public class When_Cache_Doesnt_Contain_Reasons : TestSetup
    {
        private readonly AdminChangeIpViewModel _cachedViewModel = new()
        {
            AdminIpCompletion = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 1,
                AcademicYear = 2022,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            }
        };

        private List<IpLookupDataViewModel> ReasonsList;

        public override void Given()
        {
            ReasonsList = GetReasonsList();

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cachedViewModel);
            IndustryPlacementLoader.GetSpecialConsiderationReasonsListAsync(_cachedViewModel.AdminIpCompletion.AcademicYear).Returns(ReasonsList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            IndustryPlacementLoader.Received(1).GetSpecialConsiderationReasonsListAsync(_cachedViewModel.AdminIpCompletion.AcademicYear);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminIpSpecialConsiderationReasonsViewModel>();

            model.RegistrationPathwayId.Should().Be(_cachedViewModel.AdminIpCompletion.RegistrationPathwayId);
            model.ReasonsList.Should().NotBeNullOrEmpty();
            model.ReasonsList.Should().BeEquivalentTo(ReasonsList);
        }
    }
}
