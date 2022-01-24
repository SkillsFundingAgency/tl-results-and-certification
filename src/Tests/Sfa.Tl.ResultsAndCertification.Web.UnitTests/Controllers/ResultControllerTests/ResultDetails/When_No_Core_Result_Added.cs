using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_No_Core_Result_Added : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;
        private Dictionary<string, string> _routeAttributes;
        private string _routeName;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.Now.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567891,
                TlevelTitle = "Tlevel title",
                CoreComponentExams = new List<ComponentExamViewModel>
                {
                    new ComponentExamViewModel
                    {
                        ProfileId = ProfileId,
                        AssessmentId = 1,
                        ComponentType = ComponentType.Core,
                        Grade = null
                    }
                }
            };
            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, _mockResult.ProfileId.ToString() }, { Constants.AssessmentId, _mockResult.CoreComponentExams[0].AssessmentId.ToString() } };
            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            foreach (var examComponent in _mockResult.CoreComponentExams)
            {
                examComponent.ProfileId.Should().Be(_mockResult.ProfileId);
                examComponent.AssessmentId.Should().Be(_mockResult.CoreComponentExams[0].AssessmentId);
                examComponent.ResultRouteName.Should().Be(RouteConstants.AddCoreResult);
                examComponent.ResultRouteAttributes.Should().BeEquivalentTo(_routeAttributes);
            }
        }
    }
}