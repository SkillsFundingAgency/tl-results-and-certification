using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearPost
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = ((int)AcademicYear.Year2020).ToString();
            AcademicYearViewModel = new ReregisterAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };
            cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false },
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_SubmitReregisterAcademicYear()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SubmitReregisterAcademicYear);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x => x.ReregisterAcademicYear == AcademicYearViewModel &&
                                    x.ReregisterAcademicYear.HasSpecialismsSelected == false));
        }
    }
}
