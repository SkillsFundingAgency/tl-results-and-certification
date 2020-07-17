using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthGet
{
    public class Then_On_Cache_Exist_Cached_DateOfBirth_Returned : When_AddRegistrationDateofBirth_Get_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private DateofBirthViewModel _dateofBirthViewmodel;

        public override void Given()
        {
            _dateofBirthViewmodel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
            cacheResult = new RegistrationViewModel
            {
                LearnersName = new LearnersNameViewModel(),
                DateofBirth = _dateofBirthViewmodel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_DateofBirth_ViewModel_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(DateofBirthViewModel));

            var model = viewResult.Model as DateofBirthViewModel;
            model.Should().NotBeNull();
            model.Day.Should().Be(_dateofBirthViewmodel.Day);
            model.Month.Should().Be(_dateofBirthViewmodel.Month);
            model.Year.Should().Be(_dateofBirthViewmodel.Year);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationLearnersName);
        }
    }
}
