using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearPost
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private ReregisterViewModel _cacheResult;
        private IList<AcademicYear> _academicYears;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = "2020".ToString();
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            AcademicYearViewModel = new ReregisterAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear, AcademicYears = _academicYears };
            _cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false },
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(_cacheResult);
            RegistrationLoader.GetCurrentAcademicYearsAsync().Returns(_academicYears);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            RegistrationLoader.Received(1).GetCurrentAcademicYearsAsync();
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x => x.ReregisterAcademicYear == AcademicYearViewModel &&
                                    x.ReregisterAcademicYear.HasSpecialismsSelected == false));
        }
    }
}
