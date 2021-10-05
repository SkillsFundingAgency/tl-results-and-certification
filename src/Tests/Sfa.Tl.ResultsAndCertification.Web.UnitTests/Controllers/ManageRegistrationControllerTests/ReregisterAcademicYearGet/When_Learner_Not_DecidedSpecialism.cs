using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearGet
{
    public class When_Learner_Not_DecidedSpecialism : TestSetup
    {
        private ReregisterViewModel _cacheResult;
        private ReregisterSpecialismQuestionViewModel _specialismQuestionViewModel;
        private IList<AcademicYear> _academicYears;

        public override void Given()
        {
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            _specialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(_cacheResult);
            
            RegistrationLoader
                .GetRegistrationDetailsAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Withdrawn)
                .Returns(new RegistrationDetailsViewModel { Status = RegistrationPathwayStatus.Withdrawn });
            RegistrationLoader.GetCurrentAcademicYearsAsync().Returns(_academicYears);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterAcademicYearViewModel));

            var model = viewResult.Model as ReregisterAcademicYearViewModel;
            model.Should().NotBeNull();

            model.SelectedAcademicYear.Should().BeNull();
            model.IsValidAcademicYear.Should().BeFalse();
            model.AcademicYears.Should().BeEquivalentTo(_academicYears);
            model.AcademicYearSelectList.Should().BeEquivalentTo(_academicYears.Select(a => new SelectListItem { Text = a.Name, Value = a.Year.ToString() }));

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ReregisterSpecialismQuestion);
        }
    }
}
