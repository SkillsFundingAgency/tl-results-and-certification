using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearPost
{
    public class When_AcademicYear_Invalid : TestSetup
    {
        private IList<AcademicYear> _academicYears;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = "2019";
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            AcademicYearViewModel = new ReregisterAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear, AcademicYears = _academicYears };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(new ReregisterViewModel());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
