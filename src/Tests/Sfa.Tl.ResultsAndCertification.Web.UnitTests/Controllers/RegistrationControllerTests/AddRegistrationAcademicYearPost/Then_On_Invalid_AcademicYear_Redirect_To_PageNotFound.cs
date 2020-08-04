using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearPost
{
    public class Then_On_Invalid_AcademicYear_Redirect_To_PageNotFound : When_AddRegistrationAcademicYear_Action_Is_Called
    {
        private string _selectedAcademicYear;
        public override void Given()
        {
            _selectedAcademicYear = "2019";
            SelectAcademicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(new RegistrationViewModel());
        }

        [Fact]
        public void Then_On_Invalid_Academic_Year_Redirect_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
